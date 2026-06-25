using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Features.ElectProjects.Commands.CreateBigProjectChildren;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.ErtTariffAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Update
{
    public class UpdateProjectCommandHandler(
        ICurrentUser currentUser,
        IClientRepository clientRepository,
        IElectProjectRepository electProjectRepository,
        IBuildingTariffRepository buildingTariffRepository,
        IInvoiceRepository invoiceRepository,
        IElectProjectProcessRepository electProjectProcessRepository,
        IErtTariffRepository ertTariffRepository,
        IEngPaymentTaskRepository paymentTaskRepository,
        IMediator mediator
    ) :
        IRequestHandler<UpdateProjectCommand, string>
    {


        public async Task<string> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var electProject = await electProjectRepository.GetElectProjectById(request.Id);
            if (electProject is null) throw new NotFoundException("پرونده وجود ندارد");

            if (request.LandlordName == null || request.LandlordName.Length < 6)
                throw new NotFoundException("نام مالک نباید کمتر از 6 حرف باشد");


            if (request.NumberOfFloor <= 0) throw new NotAllowedException("واحد نمی تواند صفر باشد");


            if (!request.HasSupervision && request.AreaAsBuilt > 0)
                throw new NotFoundException("وقتی تیک نظارت غیر فعال باشد نمیتوانید مساحت ازبیلت وارد کنید");

            if (request.HasSupervision && request.AreaAsBuilt <= 0)
                throw new NotFoundException("وقتی تیک نظارت  فعال باشد  مساحت ازبیلت نمیتواند صفر باشد");


            var epp = await electProjectProcessRepository.GetElectProjectProcessByEpId(electProject.Id);

            long amountPay = 0;

            // تغییرات از سمت شرکت توزیع
            if (currentUser.Role.Contains("ElectAdmin"))
            {
                electProject.UpdateWithElectCompany(
                    request.LandlordName,
                    request.LandlordName, request.LandlordNaCode,
                    request.LandlordPhoneNumber, request.PostalCode,
                    request.Address, request.NumberOfFloor, request.IdSection,
                    request.ElectRequestNumber,
                    request.HasRelatedPermit
                );

            }
			// change from not ElectAdmin
			else
			{
                if (request.IsTestAndDelivery && (request.IsBuildingInspection || request.IsEarthSystem))
                    throw new NotFoundException("پرونده تست و تحویل نمیتواند بازرسی یا مجری ارت فعال داشته باشد");

                // جلوگیری از تغییر تعداد زیر پرونده در پرونده های بزرگ از قبل ثبت شده
                if (electProject.IsBigProject && (request.ChildInspectionCount > 0 || request.ChildErtCount > 0))
                    throw new NotFoundException("این پرونده قبلا به عنوان پرونده بزرگ ثبت شده است و تعداد زیرپرونده قابل تغییر نیست");


                // دریافت تعرفه بر اساس گروه ساختمانی و گروه طبقات
                var buildingTariff =
                    await buildingTariffRepository.GetByType(request.BuildingGroupTypeEnum,
                        request.BuildingGroupParameterTypeEnum);
                if (buildingTariff is null) throw new NotFoundException("تعرفه گروه ساختمانی وجود ندارد");

                // دریافت تعرفه ارت
                var ertTariff = await ertTariffRepository.GetByType(request.ErtSystemTypeEnum);
                if (request.IsEarthSystem && ertTariff is null)
                    throw new NotFoundException("برای پرونده ارت تعرفه ارت وجود ندارد");



                // چک کردن اینکه از قبل تعرفه مشخص شده یا نه
                if (electProject.BuildingTariff is null && electProject.ProjectTypeRequestEnum is ProjectTypeRequestEnum.Pr0)
                {

                    // اگر پرونده تست و تحویل باشد
                    if (request.IsTestAndDelivery)
                    {
                        throw new NotFoundException(
                            "پرونده های ایجاد شده از سمت شرکت توزیع نمیتوان تست و تحویل ایجاد کرد");
                    }

                    if (request.BuildingGroupTypeEnum == BuildingGroupTypeEnum.G0 ||
                        request.BuildingGroupParameterTypeEnum == BuildingGroupParameterTypeEnum.B0)
                        throw new NotFoundException("گروه طبقات یا گروه ساختمانی انتخاب نشده است");

                    if (request.IsBuildingInspection)
                    {
                        var getInvoice = await invoiceRepository.GetInvoiceByProjectId(electProject.Id, InvoicePayTypeEnum.CreateProjectStage);
                        if (getInvoice is not null)
                            throw new NotFoundException("قبلا بدون تعرفه ساختمان فاکتور ایجاد پرونده شده ");

                        //دریافت تعرفه بازرسی
                        var amountPayInspection = Helper.GetAmountInspection(buildingTariff.Tariff, request.Area,
                            buildingTariff.Factor, buildingTariff.MinTariff);

                        // دریافت تعرفه نظارت
                        var amountPaySupervision = Helper.GetAmountSupervision(buildingTariff.SupervisionTariff,
                            request.AreaAsBuilt, buildingTariff.SupervisionFactor);


                        var invoiceInspection = new Invoice(client, electProject, amountPayInspection,
                            InvoiceStatusEnum.Pending,
                            InvoicePayTypeEnum.CreateProjectStage);
                        invoiceRepository.Add(invoiceInspection);

                        // اضافه کردن مبلغ نظارت به فاکتور بازرسی
                        if (request.HasSupervision)
                        {
                            if (request.AreaAsBuilt is 0)
                                throw new NotFoundException("مساحت ازبیلت نمیتواند صفر باشد باید وارد شود");
                            invoiceInspection.UpdateAmountSupervision(amountPaySupervision);
                            invoiceInspection.UpdateAmount(amountPayInspection + amountPaySupervision);
                        }

                        // ایجاد تراکنش برداشت مرحله ایجاد پرونده برای بازرسی
                        var transaction = new Transaction(
                            invoiceInspection.Amount,
                            client,
                            client.Id.ToString(),
                            GatewayTypeEnum.System,
                            TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                            Helper.MiladiToShamsi(DateTime.Now.Date),
                            electProject.FileNumber.ToString(),
                            $"برداشت ایجاد پرونده {(request.HasSupervision ? "بازرسی" : "بازرسی و نظارت")}: {electProject.FileNumber}",
                            electProject.Id.ToString());
                        invoiceInspection.Done(transaction);

                        amountPay += invoiceInspection.Amount;
                    }

                    if (request.IsEarthSystem)
                    {
                        var getInvoice = await
                            invoiceRepository.GetInvoiceByProjectId(electProject.Id, InvoicePayTypeEnum.NezamStage);
                        if (getInvoice is not null)
                            throw new NotFoundException("قبلا بدون تعرفه ساختمان فاکتور 9 درصد نظام درج شده ");

                        var amountPayErtSystem = Helper.GetAmountErtSystemType(ertTariff.ErtSystemTypeEnum,
                            ertTariff.Tariff, ertTariff.Factor, electProject.FoundationElectrodeArea);
                        var invoiceErtSystem = new Invoice(client, electProject, amountPayErtSystem,
                            InvoiceStatusEnum.Pending,
                            InvoicePayTypeEnum.NezamStage);
                        invoiceRepository.Add(invoiceErtSystem);

                        // ایجاد تراکنش برداشت مرحله ایجاد پرونده در هنگام تخصیص
                        var transaction = new Transaction(amountPayErtSystem, client, client.Id.ToString(),
                            GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                            Helper.MiladiToShamsi(DateTime.Now.Date), electProject.FileNumber.ToString(),
                            $"برداشت 9 درصد نظام:ارت:{electProject.FileNumber}", electProject.Id.ToString());
                        invoiceErtSystem.Done(transaction);

                        amountPay += amountPayErtSystem;

                    }

                    // آپدیت تعرفه ساختمان
                    electProject.UpdateBuildingTariff(buildingTariff);
                }
                // اگر تعرقه داشته باشد پس فاکتور خورده و باید آپدیت شود
                else
                {
                    // چک کردن اگر تایید شده باشد دیگر قابل تغییر نیست
                    if (electProject.IsOk)
                        throw new NotFoundException("این پرونده قبلا تایید شده است و تعرفه آن قابل ویرایش نیست");
                    
                    //  اگر از قبل تعرفه داشته باشد و  ادمین آن را  تغییر دهد باید مبالغ آن فاکتور اصلاح شوند
                    if (
                        electProject.BuildingTariff.BuildingGroupTypeEnum != request.BuildingGroupTypeEnum ||
                        electProject.BuildingTariff.BuildingGroupParameterTypeEnum !=
                        request.BuildingGroupParameterTypeEnum ||
                        electProject.Area != request.Area ||
                        electProject.AreaAsBuilt != request.AreaAsBuilt ||
                        electProject.IsEarthSystem != request.IsEarthSystem ||
                        // اگر از قبل ارت داشته باشد و تیک بازرسی را بزند
                        (request.IsEarthSystem && electProject.IsBuildingInspection != request.IsBuildingInspection) ||
                        // اگر تعرفه ارت تغییر کند
                        (request.IsEarthSystem && electProject.ErtTariff.Id != ertTariff.Id)
                    )
                    {

                        // اگر پرونده انشعاب موقت/دائم باشد
                        if (electProject.ProjectTypeRequestEnum is ProjectTypeRequestEnum.Pr0)
                        {
                            // اینجا یعنی قبلا تیک ارت داشته و الان کاربر میخواد غیر فعال کنه
                            if (electProject.IsEarthSystem && !request.IsEarthSystem)
                            {
								if (epp.Any(a =>a.Accepted && a.ProjectLevelEnum ==	ProjectLevelEnum.ApproveErtStage))
									throw new NotFoundException("برای این پرونده ارت تایید شده دارد");

								if (electProject.IsBuildingInspection) electProject.UpdateProjectLevel(ProjectLevelEnum.ExpertStage);
								if (!electProject.IsBuildingInspection) electProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);

								var ertProcess=	epp.Where(w=> w.ProjectLevelEnum is ProjectLevelEnum.ErtStage).ToList();
                                foreach (var process in ertProcess) { 
                                    process.SoftDelete();
                                }

								electProject.CancelErtSystem();

                               }

                            if (electProject.IsBuildingInspection && !request.IsBuildingInspection)
                                throw new NotFoundException("پرونده کارشناسی ثبت شده و قابل تغییر نیست");

                            if (electProject.HasSupervision && !request.HasSupervision)
                                throw new NotFoundException("پرونده نظارت ثبت شده و قابل تغییر نیست");

                            // اگر تیک بازرسی زده شود
                            if (request.IsBuildingInspection)
                            {
                                // دریافت فاکتور مربوط به مرحله ایجاد پرونده این پرونده
                                var invoiceCreatedProject =
                                    await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
                                        InvoicePayTypeEnum.CreateProjectStage);

                                // دریافت فاکتور مربوط به مرحله ای که کارشناس قبول میکنم زده و فاکتور به مرحله کارشناسی تغییر کرده
                                var invoiceExpertStage =
                                    await invoiceRepository.GetInvoiceByProjectIdWithProcess(electProject.Id,
                                        InvoicePayTypeEnum.ExpertStage);

                                // چک کردن اینکه فاکتور بازرسی در دوره قرار نگرفته باشه
                                if (invoiceExpertStage != null)
                                {
                                    var engProcess = invoiceExpertStage.ElectProjectProcess;
                                    var latestPaymentTask = await paymentTaskRepository.GetLastByOrderPeriod();
                              
                                    if (engProcess.JulianDateAccepted is null)
                                        throw new NotFoundException("برای فاکتور بازرسی تارخ قبول کردن کارشناس وجود ندارد");
                                    if (engProcess.JulianDateAccepted <= latestPaymentTask.ToJulian) 
                                        throw new NotFoundException("برای این پرونده دوره پرداخت برای کارشناس ایجاد شده و قابل تغییر نیست");
                                }

                                // دریافت تعرفه بازرسی
                                var amountPayInspection = Helper.GetAmountInspection(buildingTariff.Tariff,
                                    request.Area, buildingTariff.Factor, buildingTariff.MinTariff);

                                // دریافت تعرفه نظارت
                                var amountPaySupervision = Helper.GetAmountSupervision(buildingTariff.SupervisionTariff,
                                    request.AreaAsBuilt, buildingTariff.SupervisionFactor);



                                // اگر برای این پرونده فاکتور ایجاد نشده باشد: مرحله ایجاد پرونده کارشناسی
                                if (invoiceCreatedProject is null && invoiceExpertStage is null)
                                {
                                    var invoiceInspection = new Invoice(client, electProject, amountPayInspection,
                                        InvoiceStatusEnum.Pending,
                                        InvoicePayTypeEnum.CreateProjectStage);
                                    invoiceRepository.Add(invoiceInspection);

                                    // اضافه کردن مبلغ نظارت به فاکتور بازرسی
                                    if (request.HasSupervision)
                                    {
                                        if (request.AreaAsBuilt is 0)
                                            throw new NotFoundException("مساحت ازبیلت نمیتواند صفر باشد باید وارد شود");
                                        invoiceInspection.UpdateAmountSupervision(amountPaySupervision);
                                        invoiceInspection.UpdateAmount(amountPayInspection + amountPaySupervision);
                                    }

                                    // ایجاد تراکنش برداشت مرحله ایجاد پرونده برای کارشناسی
                                    var transaction = new Transaction(
                                        invoiceInspection.Amount, client, client.Id.ToString(),
                                        GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out,
                                        DateTime.Now,
                                        Helper.MiladiToShamsi(DateTime.Now.Date), electProject.FileNumber.ToString(),
                                        $"برداشت ایجاد پرونده کارشناسی:{electProject.FileNumber}",
                                        electProject.Id.ToString());
                                    invoiceInspection.Done(transaction);

                                    amountPay += invoiceInspection.Amount;

                                }
                                // اگر فاکتور ایجاد شده باشد آپدیت میشود
                                else
                                {

                                    if (invoiceCreatedProject is not null)
                                    {
                                        if (amountPayInspection != invoiceCreatedProject.Amount ||
                                            amountPaySupervision != invoiceCreatedProject.AmountSupervision)
                                        {
                                            if (epp.Any(a => a.Accepted && a.ProjectLevelEnum == ProjectLevelEnum.ExpertStage))
                                                throw new NotFoundException("به دلیل قبول کردن این پرونده توسط بازرس قابل تغییر نیست");

                                            invoiceCreatedProject.UpdateAmount(amountPayInspection);
                                            invoiceCreatedProject.Transaction.UpdateAmount(amountPayInspection);

                                            // اضافه کردن مبلغ نظارت به فاکتور بازرسی
                                            if (request.HasSupervision)
                                            {
                                                if (request.AreaAsBuilt is 0)
                                                    throw new NotFoundException(
                                                        "مساحت ازبیلت نمیتواند صفر باشد باید وارد شود");
                                                invoiceCreatedProject.UpdateAmountSupervision(amountPaySupervision);
                                                invoiceCreatedProject.UpdateAmount(amountPayInspection +
                                                    amountPaySupervision);
                                                invoiceCreatedProject.Transaction.UpdateAmount(amountPayInspection +
                                                    amountPaySupervision);

                                            }


                                            var oldDes = invoiceCreatedProject.Transaction.Des;
                                            invoiceCreatedProject.Transaction.UpdateDes(oldDes +
                                                $"-اصلاحیه-تایخ:   {Helper.MiladiToShamsi(DateTime.Now.Date)}-");
                                        }
                                        amountPay += invoiceCreatedProject.Amount;

                                    }

                                    if (invoiceExpertStage is not null)
                                    {
                                        if (amountPayInspection != invoiceExpertStage.Amount ||
                                            amountPaySupervision != invoiceExpertStage.AmountSupervision)
                                        {

                                            invoiceExpertStage.UpdateAmount(amountPayInspection);
                                            invoiceExpertStage.Transaction.UpdateAmount(amountPayInspection);

                                            // اضافه کردن مبلغ نظارت به فاکتور بازرسی
                                            if (request.HasSupervision)
                                            {
                                                if (request.AreaAsBuilt is 0)
                                                    throw new NotFoundException(
                                                        "مساحت ازبیلت نمیتواند صفر باشد باید وارد شود");
                                                invoiceExpertStage.UpdateAmountSupervision(amountPaySupervision);
                                                invoiceExpertStage.UpdateAmount(amountPayInspection +
                                                    amountPaySupervision);
                                                invoiceExpertStage.Transaction.UpdateAmount(amountPayInspection +
                                                    amountPaySupervision);
                                            }
                                        }

                                        var oldDes = invoiceExpertStage.Transaction.Des;
                                        invoiceExpertStage.Transaction.UpdateDes(oldDes +
                                            $"-اصلاحیه-تایخ:   {Helper.MiladiToShamsi(DateTime.Now.Date)}-");

                                        amountPay += invoiceExpertStage.Amount;

                                    }

                                }

                            }

                                // اگر تیک ارت زده شود
                            if (request.IsEarthSystem)
                            {
                                    var amountPayErtSystem = Helper.GetAmountErtSystemType(ertTariff.ErtSystemTypeEnum,
                                        ertTariff.Tariff, ertTariff.Factor, electProject.FoundationElectrodeArea);

                                    // دریافت فاکتور مربوط به مرحله ارت این پرونده
                                    var invoiceErtSystem =
                                        await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
                                            InvoicePayTypeEnum.NezamStage);

                                        amountPay += amountPayErtSystem;


                                    if (invoiceErtSystem is null)
                                    {
                                        var invoiceErtSystemNew = new Invoice(client, electProject, amountPayErtSystem,
                                            InvoiceStatusEnum.Pending,
                                            InvoicePayTypeEnum.NezamStage);
                                        invoiceRepository.Add(invoiceErtSystemNew);

                                        // ایجاد تراکنش برداشت مرحله ایجاد پرونده در هنگام تخصیص
                                        var transaction = new Transaction(amountPayErtSystem, client,
                                            client.Id.ToString(),
                                            GatewayTypeEnum.System, TransactionTypeEnum.Client,
                                            TransactionStatusEnum.Out, DateTime.Now,
                                            Helper.MiladiToShamsi(DateTime.Now.Date),
                                            electProject.FileNumber.ToString(),
                                            $"برداشت 7 درصد نظام:ارت:{electProject.FileNumber}",
                                            electProject.Id.ToString());
                                        invoiceErtSystemNew.Done(transaction);


                                    }
                                    else
                                    {
                                        if (amountPayErtSystem != invoiceErtSystem.Amount)
                                        {
                                            if (epp.Any(a =>
                                                    a.Accepted && a.ProjectLevelEnum ==
                                                    ProjectLevelEnum.ApproveErtStage))
                                                throw new NotFoundException(
                                                    "به دلیل قبول کردن این پرونده توسط مجری ارت قابل تغییر نیست");

                                            invoiceErtSystem.UpdateAmount(amountPayErtSystem);
                                            invoiceErtSystem.Transaction.UpdateAmount(amountPayErtSystem);
                                            var oldDes = invoiceErtSystem.Transaction.Des;
                                            invoiceErtSystem.Transaction.UpdateDes(oldDes +
                                                $"-اصلاحیه-تایخ:{Helper.MiladiToShamsi(DateTime.Now.Date)}-");

                                        } 

                                    }
                            }
                        }

                        // اگر پرونده تست و تحویل باشد
                        if (electProject.ProjectTypeRequestEnum is ProjectTypeRequestEnum.Pr2 &&
                            electProject.IsTestAndDelivery)
                        {
                            // دریافت فاکتور مربوط به مرحله ایجاد تست و تحویل این پرونده
                            var invoiceCreatedProject =
                                await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
                                    InvoicePayTypeEnum.TestAndDelivery);

                            if (invoiceCreatedProject is null)
                                throw new NotFoundException("فاکتور مرحله ایجاد پرونده ایجاد نشده است");
                            var amountPayTestAndDelivery = Helper.GetAmountTestAndDelivery(buildingTariff.Tariff,
                                request.Area, buildingTariff.TestAndDeliveryFactor);

                            invoiceCreatedProject.UpdateAmount(amountPayTestAndDelivery);
                            invoiceCreatedProject.Transaction.UpdateAmount(amountPayTestAndDelivery);
                            var oldDes = invoiceCreatedProject.Transaction.Des;
                            invoiceCreatedProject.Transaction.UpdateDes(
                                oldDes + $"-اصلاحیه-تایخ:{Helper.MiladiToShamsi(DateTime.Now.Date)}-");

                            amountPay += amountPayTestAndDelivery;

                        }


                        electProject.UpdateBuildingTariff(buildingTariff);

                    }
                }



                electProject.Update(
                        request.LicenseNumber,
                        request.LandlordName,
                        null,
                        request.LandlordNaCode,
                        request.LandlordPhoneNumber,
                        request.PostalCode,
                        request.Area,
                        request.Address,
                        request.NumberOfFloor,
                        request.IdSection,
                        request.IdCity,
                        request.IdProvince,
                        request.IsEarthSystem,
                        request.IsBuildingInspection,
                        request.PanelNeed,
                        ertTariff,
                        request.IsNeedEb,
                        request.HasRelatedPermit,
                        request.HasSupervision,
                        request.AreaAsBuilt
                    );

                

                if (amountPay > 0)
                {
                    // دریافت هزینه خدمات
                    var amountServices = Helper.GetAmountProjectServices(amountPay);
                    // دریافت فاکتور مربوط خدمات پرونده
                    var invoiceServices =
                        await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
                            InvoicePayTypeEnum.ProjectServices);

                    if (invoiceServices is null)
                    {
                        // ایجاد کردن فاکتور خدمات

                        var invoiceServicesNew = new Invoice(
                            client, electProject, amountServices,
                            InvoiceStatusEnum.Pending,
                            InvoicePayTypeEnum.ProjectServices);

                        invoiceRepository.Add(invoiceServicesNew);
                        // ایجاد تراکنش برداشت خدمات پرونده 
                        var transactionServices = new Transaction(
                            amountServices, client, client.Id.ToString(),
                            GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                            Helper.MiladiToShamsi(DateTime.Now.Date), electProject.FileNumber.ToString(),
                            $"برداشت هزینه خدمات:{electProject.FileNumber}", electProject.Id.ToString());
                        invoiceServicesNew.Done(transactionServices);
                    }
                    else
                    {

                        invoiceServices.UpdateAmount(amountServices);
                        invoiceServices.Transaction.UpdateAmount(amountServices);
                        var oldDes = invoiceServices.Transaction.Des;
                        invoiceServices.Transaction.UpdateDes(oldDes + $"-اصلاحیه-تایخ:{Helper.MiladiToShamsi(DateTime.Now.Date)}-");
                    }

                }


                // -- Administrator: ایجاد زیرپرونده های پرونده بزرگ
                // شرط: ادمین سیستم + تعرفه ساختمان تعیین شده + پرونده هنوز بزرگ نشده +
                //       تعداد زیرپرونده بازرسی یا ارت بیشتر از صفر
                if (currentUser.Role.Contains("Administrator") &&
                    electProject.BuildingTariff is not null &&
                    !electProject.IsBigProject &&
                    (request.ChildInspectionCount > 0 || request.ChildErtCount > 0))
                {
                    var startFileNumber = await electProjectRepository
                        .GetLastFileNumber(Guid.Parse(currentUser.ClientId), 1405) + 1;

                    await mediator.Send(new CreateBigProjectChildrenCommand(
                        electProject,
                        client,
                        buildingTariff,
                        request.IsEarthSystem ? ertTariff : null,
                        request.ChildInspectionCount,
                        request.ChildErtCount,
                        startFileNumber,
                        request.Area,
                        request.AreaAsBuilt,
                        request.NumberOfFloor,
                        request.Address,
                        request.PostalCode,
                        request.LandlordName,
                        request.LandlordNaCode,
                        request.LandlordPhoneNumber,
                        request.LicenseNumber,
                        request.IdSection,
                        request.HasSupervision,
                        request.IsEarthSystem,
                        false, // IsTestAndDelivery - همیشه false برای زیرپرونده های Update
                        request.PanelNeed,
                        request.FoundationElectrodeArea,
                        request.IsNeedEb,
                        request.HasRelatedPermit
                    ), cancellationToken);
                }

            }
            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return "success";
        }
    }
}
