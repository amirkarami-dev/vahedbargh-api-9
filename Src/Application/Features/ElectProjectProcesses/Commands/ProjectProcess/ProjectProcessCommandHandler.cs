using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.ProjectProcess
{
    public class ProjectProcessCommandHandler(
        IClientRepository clientRepository,
        ICurrentUser currentUser,
        IElectProjectRepository electProjectRepository,
        IElectProjectProcessRepository electProjectProcessRepository,
        ITransactionRepository transactionRepository,
        IInvoiceRepository invoiceRepository,
        IQuarterTariffRepository quarterTariffRepository,
        IEngineerRepository engineerRepository,
        ISmsService smsService,
        IElectProjectFileRepository electProjectFileRepository)
        : IRequestHandler<ProjectProcessCommand, int>
    {
        public async Task<int> Handle(ProjectProcessCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var engineer = await engineerRepository.GetById(request.IdEngineer);
            if (engineer is null) throw new NotFoundException("مهندس وجود ندارد");


            var lastQuarterTariff = await quarterTariffRepository.GetLatest();
            if (lastQuarterTariff is null) throw new NotFoundException("دوره تخصیص وجود ندارد");


            foreach (var guid in request.IdElectProjects)
            {
                var electProject = await electProjectRepository.GetElectProjectById(guid);
                if (electProject is null) throw new NotFoundException("پرونده وجود ندارد");
                if (electProject.IsStop) throw new NotFoundException("پرونده استاپ شده و  قابل تخصیص نیست");

                if (electProject.IsTestAndDelivery && 
                    (electProject.IsBuildingInspection ||
                     electProject.IsEarthSystem ||
                     electProject.IsErtTest))
                    throw new NotFoundException("پرونده تست و تحویل نمیتواند بازرسی یا مجری ارت فعال داشته باشد");



      

                var projectFiles = await electProjectFileRepository.GetByIdElectProject(electProject.Id);
                var relatedPermit = projectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.RelatedPermit);
                var electPlan = projectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.ElectPlan);





                // اگر تست و تحویل نباشد گروه شرایط زیر چک میشود
                if (!electProject.IsTestAndDelivery)
                {

                    if (electProject.ProjectLevelEnum == ProjectLevelEnum.ApproveStage) throw new NotFoundException("مرحله تایید قابل تخصیص نیست");
                    if (electProject.BuildingTariff is null)
                        throw new NotFoundException("گروه ساختمانی هنوز مشخص نشده است");
                    var canDo = await engineerRepository.CanDoThisProcess(engineer.Id, electProject.Id);
                    if (canDo is false) throw new NotFoundException("نوع گروه ساختمان با پایه کارشناس قابل تخصیص نیست");

                    if (relatedPermit is null) throw new NotFoundException("جواز آپلود نشده است");
                    if (electPlan is null && request.ProjectProcessTypeEnum == ProjectProcessTypeEnum.EngProcess ) throw new NotFoundException("نقشه برق آپلود نشده است");

                }


                var projectProcess = await electProjectProcessRepository.GetElectProjectProcessByEpId(guid);
                var projectBalance = await transactionRepository.GetProjectBalance(electProject.Id);




                // جهت تخصیص اولیه به کارشناس بازرسی/تست وتحویل و یا فقط مجری ارت
                if (electProject.ProjectLevelEnum == ProjectLevelEnum.NullStage)
                {

                    // اگر تخصیص به کارشناس باشد
                    if (request.ProjectProcessTypeEnum == ProjectProcessTypeEnum.EngProcess )
                    {

                        if (electProject.IsTestAndDelivery is false
                                                                    && electProject.IsBuildingInspection is false) 
                            throw new NotFoundException("این پرونده بازرسی ندارد و قابل تخصیصی به کارشناس نیست");

                        // دریافت فاکتور مربوط به مرحله ایجاد  فقط برای کارشناسی یا تست و تحویل این پرونده
                        var invoiceCreatedProject =
                            await invoiceRepository.GetInvoiceByProjectId(electProject.Id, InvoicePayTypeEnum.CreateProjectStage);

                        //var invoiceTestAndDelivery =
                        //    await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
                        //        InvoicePayTypeEnum.TestAndDelivery);

                        // اگر پرونده بازرسی باشد
                        if (electProject.IsBuildingInspection)
                        {
                            // چک کردن مجوز بازرسی کارشناس
                            if (engineer.CertOfInspection is false)
                                throw new NotFoundException("این کارشناس مجوز بازرسی ندارد");
                            if (invoiceCreatedProject is null) throw new NotFoundException("فاکتور مرحله ایجاد پرونده کارشناسی ایجاد نشده است");

                        }
                        // اگر تست و تحویل باشد شرایط زیر چک میشود
                        if (electProject.IsTestAndDelivery)
                        {
                            //if (invoiceTestAndDelivery is null) throw new NotFoundException("فاکتور مرحله تست و تحویل ایجاد نشده است");


                            if (electProject.ProjectLevelEnum == ProjectLevelEnum.ApproveTestDeliveryStage) throw new NotFoundException("پرونده تست و تحویل تایید شده قابل تخصیص نیست");

                            if (!engineer.CertOfTest) throw new NotFoundException("این کارشناس مجوز تست و تحویل را ندارد");
                        }




                        if (projectBalance < 0) throw new NotFoundException("بالانس این پرونده در مرحله تخصیص کارشناسی حداقل باید صفر باشد");

                        // چک کردن اینکه این پرونده از قبل پروسس تایید نشد ای از نوع ارت داشته باشد بعد تخصیص باید دوباره بره به مرحله ارت
                        // چون این زمانی پیش اومده که قبلا در مرحله ارت بوده و تخصیص کارشناس را حذف کرده ایم که برگشته به مرحله نال استیج
                        var hasErtProcess = projectProcess.Any(a => a.ProjectLevelEnum is ProjectLevelEnum.ErtStage or ProjectLevelEnum.ApproveErtStage && a.Accepted && a.InspectionStatusEnum == InspectionStatusEnum.Undefined);

                        if (hasErtProcess)
                        {
                            var findProcess = projectProcess.FirstOrDefault(w =>
                                w.ProjectLevelEnum is ProjectLevelEnum.ErtStage or ProjectLevelEnum.ApproveErtStage);

                            if (findProcess!.Engineer.Id == engineer.Id)
                                throw new NotFoundException("کارشناس و مجری نمیتواند یکی باشد");
                        }


                        var electProjectProcess = new ElectProjectProcess(
                            client, currentUser.UserId, 
                            electProject, 
                            engineer, 
                            electProject.IsTestAndDelivery ? ProjectLevelEnum.TestDeliveryStage : ProjectLevelEnum.ExpertStage, 
                            electProject.BuildingTariff, 
                            InspectionStatusEnum.Undefined, false,
                            electProject.IsTestAndDelivery ? 0:invoiceCreatedProject.Amount, 
                            lastQuarterTariff, 
                            electProject.JulianRegisterDate, 
                            electProject.SolarRegisterDate, 
                            DateTime.Now.Date, 
                            Helper.MiladiToShamsi(DateTime.Now.Date), 
                            null, null, null);
                        electProjectProcessRepository.Add(electProjectProcess);


                        // آپدیت کردن پرونده به مرحله کارشناسی یا تست و تحویل
                        electProject.UpdateProjectLevel(electProject.IsTestAndDelivery ?
                            ProjectLevelEnum.TestDeliveryStage : hasErtProcess ? ProjectLevelEnum.ErtStage : ProjectLevelEnum.ExpertStage);

                        var levelSms = electProject.IsTestAndDelivery ?$"تست وتحویل-ناظر:{electProject.SupervisorName + electProject.SupervisorPhoneNumber}": "-مرحله بازرسی-" ;

                        await smsService.SendSms4Params(engineer.CellPhone, 9311, electProject.LandlordName,electProject.LandlordPhoneNumber,
                            electProject.FileNumber + levelSms, Helper.MiladiToShamsiForSms(DateTime.Now.Date));

                        // ارسال پیام به ناظر
                        if (electProject.IsTestAndDelivery)
                        {

                            await smsService.SendSms5Params(electProject.SupervisorPhoneNumber, 9382,
                                electProject.FileNumber.ToString(), electProject.LandlordName, engineer.FullName,
                                engineer.CellPhone, Helper.MiladiToShamsiForSms(DateTime.Now.Date));


                            await smsService.SendSms5Params(electProject.LandlordPhoneNumber, 9312, electProject.FileNumber.ToString(),
                                "مجری تست و تحویل", engineer.FullName, engineer.CellPhone,
                                Helper.MiladiToShamsiForSms(DateTime.Now.Date));


                        }
                        else
                        {
                            await smsService.SendSms5Params(electProject.LandlordPhoneNumber, 9312, electProject.FileNumber.ToString(),
                                "بازرسی", engineer.FullName, engineer.CellPhone,
                                Helper.MiladiToShamsiForSms(DateTime.Now.Date));

                        }
                    }

                    // اگر  اولین تخصیص به مجری ارت باشد
                    if (request.ProjectProcessTypeEnum == ProjectProcessTypeEnum.ErtProcess)
                    {
                        if (electProject.IsEarthSystem is false) throw new NotFoundException("این پرونده مجری ارت لازم ندارد");

                        if (electProject.IsBuildingInspection) throw new NotFoundException("ابتدا باید به کارشناس تخصیص داده شود");
                        if (electProject.IsTestAndDelivery) throw new NotFoundException("این پرونده تست و تحویل می باشد و قابل تخصیص به مجری نیست");

                        if (projectBalance < 0) throw new NotFoundException("بالانس این پرونده در مرحله تخصیص به مجری منفی می باشد");

                        if (projectProcess.Any(a => a.Engineer.Id == engineer.Id && a.ProjectLevelEnum == ProjectLevelEnum.ExpertStage))
                            throw new NotFoundException("مجری ارت نمیتواند کارشناس این پرونده باشد");

                        if (engineer.CertOfEarth is false) throw new NotFoundException("این کارشناس مجوز ارت ندارد");


                            // دریافت فاکتور مربوط به دفتر اجرایی ارت 
                            var invoiceNezam =
                                await invoiceRepository.GetInvoiceByProjectId(electProject.Id, InvoicePayTypeEnum.NezamStage);
                            if (invoiceNezam == null)
                                throw new NotFoundException("فاکتور برداشت دفتر اجرایی-ارت ایجاد نشده");
          


                            var electProjectProcess = new ElectProjectProcess(
                                client, currentUser.UserId, electProject, engineer,
                                ProjectLevelEnum.ErtStage, electProject.BuildingTariff,
                                InspectionStatusEnum.Undefined, false, 0, lastQuarterTariff,
                                electProject.JulianRegisterDate, electProject.SolarRegisterDate, 
                                DateTime.Now, Helper.MiladiToShamsi(DateTime.Now), 
                                null, null, null);
                            electProjectProcessRepository.Add(electProjectProcess);
                            // آپدیت کردن مرحله پرونده به مرحله ارت
                            electProject.UpdateProjectLevel(ProjectLevelEnum.ErtStage);


                            // ارسال اسمس  به کارشناس

                            await smsService.SendSms4Params(engineer.CellPhone, 9311, electProject.LandlordName,electProject.LandlordPhoneNumber,
                                electProject.FileNumber + "-مرحله ارت-", Helper.MiladiToShamsiForSms(DateTime.Now.Date));

                            // ارسال اسمس به مالک
                        await smsService.SendSms5Params(electProject.LandlordPhoneNumber, 9312, electProject.FileNumber.ToString(),
                                "مجری ارت", engineer.FullName, engineer.CellPhone,
                                Helper.MiladiToShamsiForSms(DateTime.Now.Date));


                    }

                }

                // اگر مرحله نال استیج نباشد
                else
                {

                    // چک کردن اینکه اگر تست و تحویل باشد دیگر مرحله تخصیص دیگری ندارد

                    if (electProject.IsTestAndDelivery)
                        throw new NotFoundException("این پرونده تست و تحویل می باشد و قبلا تخصیص داده شده");
                    if (electProject.ProjectLevelEnum is ProjectLevelEnum.ErtStage or ProjectLevelEnum.ApproveErtStage)
                        throw new NotFoundException("پرونده در مرحله ارت  نمیتواند به مجری ارت تخصیص داده شود");

                    // جهت تخصیص به مجری ارت
                    if (electProject.ProjectLevelEnum == ProjectLevelEnum.ExpertStage)
                    {
                        if (electProject.IsEarthSystem is false) throw new NotFoundException("این پرونده مجری ارت لازم ندارد");

                        if (request.ProjectProcessTypeEnum is not ProjectProcessTypeEnum.ErtProcess)
                            throw new NotFoundException("در مرحله کارشناسی فقط میتوان به مجری ارت تخصیص داد");

                        if (projectBalance < 0)
                            throw new NotFoundException("بالانس این پرونده در مرحله تخصیص به مجری صفر/منفی می باشد");


                        if (projectBalance - request.ErtAmount < 0)
                            throw new NotFoundException("بالانس پرونده منفی میشود");


                        if (projectProcess.Any(a => a.Engineer.Id == engineer.Id && a.ProjectLevelEnum == ProjectLevelEnum.ExpertStage))
                            throw new NotFoundException("مجری ارت نمیتواند کارشناس این پرونده باشد");

                        if (engineer.CertOfEarth is false) throw new NotFoundException("این کارشناس مجوز ارت ندارد");

                        // دریافت فاکتور مربوط به دفتر اجرایی ارت 
                        var invoiceNezam =
                            await invoiceRepository.GetInvoiceByProjectId(electProject.Id, InvoicePayTypeEnum.NezamStage);
                        if (invoiceNezam == null)
                            throw new NotFoundException("فاکتور برداشت دفتر اجرایی-ارت ایجاد نشده");

                  

                        var electProjectProcess = new ElectProjectProcess(client, currentUser.UserId, electProject, engineer,
                                ProjectLevelEnum.ErtStage, electProject.BuildingTariff, InspectionStatusEnum.Undefined,
                                false, 0, lastQuarterTariff, electProject.JulianRegisterDate,
                                electProject.SolarRegisterDate, DateTime.Now, Helper.MiladiToShamsi(DateTime.Now), null, null, null);
                            electProjectProcessRepository.Add(electProjectProcess);
                            electProject.UpdateProjectLevel(ProjectLevelEnum.ErtStage);


                            // ارسال اسمس به کارشناس
                            await smsService.SendSms4Params(engineer.CellPhone, 9311, electProject.LandlordName,electProject.LandlordPhoneNumber,
                                electProject.FileNumber + "-مرحله ارت-", Helper.MiladiToShamsiForSms(DateTime.Now.Date));

                            // ارسال اسمس به مالک 
                            await smsService.SendSms5Params(electProject.LandlordPhoneNumber, 9312, electProject.FileNumber.ToString(),
                                "مجری ارت", engineer.FullName, engineer.CellPhone,
                                Helper.MiladiToShamsiForSms(DateTime.Now.Date));
                    }




                }
                electProject.UpdateStatus(ElectProjectStatusEnum.InProgress);

                await electProjectProcessRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            }


            return 1;

        }
    }
}
