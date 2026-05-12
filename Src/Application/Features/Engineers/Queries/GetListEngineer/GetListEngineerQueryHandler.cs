using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Models;
using Coreapi.Common.Utility;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.QuarterIncrease;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using MediatR;
using static System.String;

namespace Coreapi.Application.Features.Engineers.Queries.GetListEngineer
{
    public class GetListEngineerQueryHandler:IRequestHandler<GetListEngineerQuery,IEnumerable<ClientEngineersViewModel>>
    {
        private readonly IEngineerRepository engineerRepository;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;
        private readonly IUserManager userManager;
        private readonly IQuarterTariffRepository quarterTariffRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IElectProjectProcessRepository electProjectProcessRepository;
        private IOrderedEnumerable<ClientEngineersViewModel> engineersViewModels;
        private readonly IEngQuotaBurnRepository engQuotaBurnRepository;
        private readonly IQuarterIncreaseRepository quarterIncreaseRepository;


        public GetListEngineerQueryHandler(
            IEngineerRepository engineerRepository, 
            IEngineerHistoryRepository engineerHistoryRepository, 
            ICurrentUser currentUser, 
            IClientRepository clientRepository, 
            IUserManager userManager, 
            IQuarterTariffRepository quarterTariffRepository, 
            ITransactionRepository transactionRepository, 
            IInvoiceRepository invoiceRepository, 
            IElectProjectProcessRepository electProjectProcessRepository, 
            IEngQuotaBurnRepository engQuotaBurnRepository,
            IQuarterIncreaseRepository quarterIncreaseRepository
            )
        {
            this.engineerRepository = engineerRepository;
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
            this.userManager = userManager;
            this.quarterTariffRepository = quarterTariffRepository;
            this.transactionRepository = transactionRepository;
            this.electProjectProcessRepository = electProjectProcessRepository;
            this.engQuotaBurnRepository = engQuotaBurnRepository;
            this.quarterIncreaseRepository = quarterIncreaseRepository;
        }

        public async Task<IEnumerable<ClientEngineersViewModel>> Handle(GetListEngineerQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var engineers = await engineerRepository.GetByClientIdViewModel(client.Id, IsNullOrEmpty(request.EngId)?null:Guid.Parse(request.EngId), request.FilterCertEnum);

            var clientEngineersViewModels = engineers as ClientEngineersViewModel[] ?? engineers.ToArray();
            var quarterTariffs = await quarterTariffRepository.GetGreateEqualThanPeriod(4);
            var usersBalances = await transactionRepository.GetBalances();
            var processFees = await electProjectProcessRepository.GetElectProjectProcessFee(4);
            var allUser = await userManager.GetUsers("Engineer");

            var getAllQuarterIncrease = await quarterIncreaseRepository.GetAllQuarterIncrease(4);

            var engQuotaBurns = await engQuotaBurnRepository.GetAllEngQuotaBurn(true,4);



            foreach (var engineer in clientEngineersViewModels)
            {
                if(engineer is null) continue;



                if (engineer.FullName == "حسن رشیدی زاده")
                {
                    var dd = 1;

                }


                var engBurn = engQuotaBurns.Where(w => w.Engineer.Id == engineer.Id).Sum(s => s.AmountBurning);
                var engSumInspectionDelayFactor = engQuotaBurns.Where(w => w.Engineer.Id == engineer.Id).Sum(s => s.InspectionDelayFactor);
                engSumInspectionDelayFactor = engSumInspectionDelayFactor == 0 ? 1 : engSumInspectionDelayFactor;


                var engCountErtBurn = engQuotaBurns.Where(w => w.Engineer.Id == engineer.Id).Sum(s => s.ErtCountBurning);
                var engSumErtDelayFactor = engQuotaBurns.Where(w => w.Engineer.Id == engineer.Id).Sum(s => s.ErtDelayFactor);
                engSumErtDelayFactor = engSumErtDelayFactor == 0 ? 1 : engSumErtDelayFactor;


                var engHistory = engineer.EngineerHistoryViewModel.MaxBy(a => a.JulianValidityDate);

                engineer.SolarIssueDate = engineer.SolarIssueDate;
                engineer.UserName = allUser.FirstOrDefault(f => f.Id == engineer.UserId)?.UserName;
                engineer.SumEngBalance = usersBalances.FirstOrDefault(b => b.UserId == engineer.UserId)?.Balance ?? 0;


                // calculate Increases
                var ertIncrease = (int)Math.Ceiling(getAllQuarterIncrease
                    .Where(w => w.Engineer.Id == engineer.Id)
                    .Sum(s => s.QuarterTariff.CountErt * s.QuarterTariff.PercentIncrease * (Helper.GetEngGradeFactor(engineer?.EngineerHistoryViewModel, s.QuarterTariff.JulianAllotmentDate,s.QuarterTariff.AddDifDays))));

                var expertIncrease = (long)getAllQuarterIncrease
                    .Where(w => w.Engineer.Id == engineer.Id)
                    .Sum(s => s.QuarterTariff.Fee * s.QuarterTariff.PercentIncrease * (Helper.GetEngGradeFactor(engineer?.EngineerHistoryViewModel, s.QuarterTariff.JulianAllotmentDate, s.QuarterTariff.AddDifDays)));

                // Get Sum Amount eng quata
                engineer.SumAmountEngQuota = engineerRepository.GetSumAmountQuota(engineer.DefaultQuota,
                    engineer.EngineerHistoryViewModel, quarterTariffs) + expertIncrease;

                // Get Count ert quata
                engineer.CountErtQuota = engineerRepository.
                    GetCountErtQuota(engineer.DefaultQuota, engineer?.EngineerHistoryViewModel, quarterTariffs) + ertIncrease;

                engineer.SumAmountEngProcessFee = processFees.FirstOrDefault(c => c.EngId == engineer.Id)?.Fee ?? 0;

                engineer.CountCancel = processFees.FirstOrDefault(c => c.EngId == engineer.Id)?.CountCancel ?? 0;
                engineer.CountExpertCancel = processFees.FirstOrDefault(c => c.EngId == engineer.Id)?.CountExpertCancel ?? 0;
                engineer.CountErtCancel = processFees.FirstOrDefault(c => c.EngId == engineer.Id)?.CountErtCancel ?? 0;
                engineer.SumAreaTestAndDelivery = processFees.FirstOrDefault(c => c.EngId == engineer.Id)?.SumAreaTestAndDelivery ?? 0;

                var CountErtProcessE6 = processFees.FirstOrDefault(c => c.EngId == engineer.Id)?.CountErtProcessE6 ?? 0;
                var CountErtProcessAll = processFees.FirstOrDefault(c => c.EngId == engineer.Id)?.CountErtProcess ?? 0;

                engineer.CountErtProcess = CountErtProcessE6 + CountErtProcessAll;



           

                engineer.TotalQuotaBalance = engineer.SumAmountEngQuota - engineer.SumAmountEngProcessFee - engBurn;


                engineer.TotalQuotaBalancePercent = engineer.SumAmountEngQuota != 0
                    ? Math.Round
                    (
                        ((double)(engineer.SumAmountEngProcessFee * engSumInspectionDelayFactor) / (engineer.SumAmountEngQuota - engBurn)) * 100, 
                        digits:3,MidpointRounding.ToZero
                    )
                    : 0;


                // Percentage of ert balance
                engineer.CountErtBalancePercent =
                    (engineer.CountErtQuota != 0)
                        ?Math.Round
                        (
                            ((double)(engineer.CountErtProcess * engSumErtDelayFactor) / (engineer.CountErtQuota- engCountErtBurn)) *100,
                            digits:3,MidpointRounding.ToZero
                        )
                        : 0; // or some other appropriate value

                engineer.Factor = engineerRepository.GetEngFactor(engHistory);


                if (engHistory is null)
                {
                    engineer.FullDescription = " اطلاعات پروانه کارشناس را وارد کنید " + " " + engineer.FullName + " ";
                }
                else
                {
                    var txtExpert = request.FilterCertEnum is FilterCertEnum.Fc0 or FilterCertEnum.Fc1
                    ?Join(" / ",
                            $"درصد بازرسی انجام شده: {engineer.TotalQuotaBalancePercent}%",
                            $"مانده بازرسی: {engineer.TotalQuotaBalance:N0}",
                            $"بازرسی انجام شده: {engineer.SumAmountEngProcessFee:N0}",
                            $"کنسلی بازرسی: {engineer.CountExpertCancel}",
                            $"کل تخصیص بازرسی: {engineer.SumAmountEngQuota:N0}",
                            $"سوخت آخر سال بازرسی: {engBurn:N0}",
                            //currentUser.Role.Contains("Administrator")
                            //    ? $"سوخت آخر سال بازرسی: {engBurn:N0}"
                            //    : "-",
                            $"ضریب تاخیر بازرسی: {engSumInspectionDelayFactor}"
                        )
                        : "";

                    var txtErt = request.FilterCertEnum is FilterCertEnum.Fc0 or FilterCertEnum.Fc2
                        ? string.Join(" / ",
                            $"درصد ارت انجام شده: {engineer.CountErtBalancePercent}%",
                            $"مانده ارت: {engineer.CountErtQuota - engineer.CountErtProcess - engCountErtBurn}",
                            $"ارت انجام شده: {engineer.CountErtProcess}",
                            $"کنسلی ارت: {engineer.CountErtCancel}",
                            $"کل تخصیص ارت: {engineer.CountErtQuota}",
                            $"سوخت آخر سال ارت: {engCountErtBurn}",
                            $"ضریب تاخیر ارت: {engSumErtDelayFactor}"
                        )
                        : "";

                    var txtTest = request.FilterCertEnum is FilterCertEnum.Fc0 or FilterCertEnum.Fc3
                        ? " / " + "تست:" + engineer.SumAreaTestAndDelivery
                        : "";

                    engineer.FullDescription = 
                        engineer.FullName
                        + " / " + txtExpert
                        //+ " / " + "ت اعتبار:" + (engHistory.JulianValidityDate < DateTime.Now ? "اعتبارندارد" : engHistory.SolarValidityDate.Replace("/", "-")) 
                        //+ " / " + "تعداد ارت:" + engineer.CountErtProcess
                        + " / " + "ضریب:" + engineer.Factor
                        + txtErt
                        + txtTest
                        
                        //+ " / " + "ارت تخصیصی:" + engineer.CountErtQuota 
                        
                        + " / ";
                    engineer.ExpiredDateWork = engHistory.JulianValidityDate < DateTime.Now;
                    engineer.EngineerGradeTypeName = engHistory.EngineerGradeTypeName;
                    engineer.CanDoGroupBuilding = Helper.GetDoGroupBuilding(engineer.EngineerGradeTypeEnum);
                    engineer.EngineerGradeTypeEnum = engHistory.EngineerGradeTypeEnum;

                    engineer.SolarValidityDate = engHistory.SolarValidityDate;
                }




            }

            engineersViewModels = request.FilterCertEnum switch
            {
                FilterCertEnum.Fc2 => clientEngineersViewModels
                    .Where(w => request.EngineerGradeTypeEnum == EngineerGradeTypeEnum.None ||
                                w.EngineerGradeTypeEnum == request.EngineerGradeTypeEnum)
                    .OrderBy(c => c.CountErtBalancePercent)
                    .ThenBy(o => o.ExpiredDateWork),
                FilterCertEnum.Fc3 => clientEngineersViewModels.Where(w => request.EngineerGradeTypeEnum == EngineerGradeTypeEnum.None ||
                                                                           w.EngineerGradeTypeEnum == request.EngineerGradeTypeEnum)
                    .OrderBy(o => o.ExpiredDateWork).
                    ThenBy(c => c.SumAreaTestAndDelivery),
                _ => clientEngineersViewModels.Where(w => request.EngineerGradeTypeEnum == EngineerGradeTypeEnum.None ||
                                                          w.EngineerGradeTypeEnum == request.EngineerGradeTypeEnum)
                    .OrderBy(o => o.ExpiredDateWork).
                    ThenBy(c => c.TotalQuotaBalancePercent)
            };








          
            //var clientEngineersViewModels = engineers.ToList();
            //foreach (var engineer in clientEngineersViewModels)
            //{
            //    var user = await userManager.GetUserAsync(engineer.SearchUserId);
            //    engineer.UserData = new UserDataModel
            //    {
            //        UserName = user.UserName,
            //        Active = user.Active,
            //        ExpiryDate = user.ExpiryDate,
            //        PhoneNumber = user.PhoneNumber
            //    } ;
            //}


            return engineersViewModels;
        }
    }
}
