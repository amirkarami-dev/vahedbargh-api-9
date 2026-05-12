using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using MediatR;

namespace Coreapi.Application.Features.Engineers.Queries.GetListEngWork
{
    internal class GetListEngQuotaWorkHandler:IRequestHandler<GetListEngWorkQuery,IEnumerable<ClientEngWorkViewModel>>
    {
        private readonly IEngineerRepository engineerRepository;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;
        private readonly IQuarterTariffRepository quarterTariffRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IElectProjectProcessRepository electProjectProcessRepository;

        public GetListEngQuotaWorkHandler(IEngineerRepository engineerRepository, IEngineerHistoryRepository engineerHistoryRepository, ICurrentUser currentUser, IClientRepository clientRepository, IUserManager userManager, IQuarterTariffRepository quarterTariffRepository, ITransactionRepository transactionRepository, IInvoiceRepository invoiceRepository, IElectProjectProcessRepository electProjectProcessRepository)
        {
            this.engineerRepository = engineerRepository;
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
            this.quarterTariffRepository = quarterTariffRepository;
            this.transactionRepository = transactionRepository;
            this.electProjectProcessRepository = electProjectProcessRepository;
        }
        public async Task<IEnumerable<ClientEngWorkViewModel>> Handle(GetListEngWorkQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

    
            var quarterTariffs = await quarterTariffRepository.GetAll();

            var enumerable = quarterTariffs.ToList();
            var quarterTariff = enumerable.FirstOrDefault(w => w.Id == Guid.Parse(request.QtId));

         if (quarterTariff is null) throw new NotFoundException("تخصیص سه ماهه وجود ندارد");

         var quarterTariffsB = enumerable.Where(w => w.Period < quarterTariff.Period).ToList();
    

            var quarterToMont = new int[4, 3] { { 1,2,3}, { 4,5,6}, {7,8,9}, {10,11,12} };

        var whatQuarter = (int) quarterTariff.QuarterTypeEnum-1;

        var engineers = await engineerRepository.GetEngWorkViewModel(client.Id, string.IsNullOrEmpty(request.EngId) ? null : Guid.Parse(request.EngId), quarterTariff.Id,quarterTariff.Period, quarterToMont[whatQuarter, 0], quarterToMont[whatQuarter, 1], quarterToMont[whatQuarter, 2]);

            var engWorkViewModel = engineers as ClientEngWorkViewModel[] ?? engineers.ToArray();

            var usersBalances = await transactionRepository.GetBalances();
            // لیست تمام تخصیص های این دوره بجز کنسل و حذف شده ها
            var processFees = await electProjectProcessRepository.GetElectProjectProcessFee(quarterTariff.Id);
            // لیست تمام تخصیص های قبل این دوره بجز کنسل و حذف شده ها

            var processFeesBeforePeriod =
                await electProjectProcessRepository.GetElectProjectProcessFeeBeforePeriod(quarterTariff.Period);

            foreach (var engWork in engWorkViewModel)
            {
                var engHistory = engWork?.EngineerHistoryViewModel
                    .Where(w => w.JulianValidityDate >= quarterTariff.JulianAllotmentDate)
                    .Where(w => w.JulianIssueDate <= quarterTariff.JulianAllotmentDate.AddDays(quarterTariff.AddDifDays))
                    .OrderByDescending(a => a.JulianIssueDate).FirstOrDefault();

                if (engWork is null) continue;
                if (engHistory is null) continue;

                // محاسبه جمع تخصیص این دوره با ضریب
                engWork.SumAmountEngQuota = engineerRepository.GetSumAmountQuotaSingle(engWork.DefaultQuota,engHistory.EngineerGradeTypeEnum,engHistory.JulianIssueDate, quarterTariff, engHistory.WorkPermitTypeEnum);
                // در اینجا کوارتر های قبل این کوارتر ورودی به تابع زیر ارسال میشد
                // محاسبه جمع تخصیص های قبلی با ضریب های مربوطه به همان دوره
                engWork.SumAmountEngQuotaBeforePeriod = engineerRepository.GetSumAmountQuota(engWork.DefaultQuota,engWork.EngineerHistoryViewModel, quarterTariffsB);


                // کل مبغ پرداختی
                engWork.SumEngBalance = usersBalances.FirstOrDefault(b => b.UserId == engWork.UserId)?.Balance ?? 0;

                // // لیست تمام تخصیص های این دوره که به مهندسی داده شده بجز کنسل و حذف شده ها
                engWork.SumAmountEngProcessFee = processFees.FirstOrDefault(c => c.EngId == engWork.Id)?.Fee ?? 0;
                // // لیست تمام تخصیص های دوره های قبل که به مهندسی داده شده بجز کنسل و حذف شده ها
                engWork.SumAmountEngProcessFeeBeforePeriod = processFeesBeforePeriod.FirstOrDefault(c => c.EngId == engWork.Id)?.Fee ?? 0;



                 engWork.TotalQuotaBalance = engWork.SumAmountEngQuota - engWork.SumAmountEngProcessFee;



                engWork.Year = quarterTariff.Year;


            }

            return engWorkViewModel;
        }
    }
}
