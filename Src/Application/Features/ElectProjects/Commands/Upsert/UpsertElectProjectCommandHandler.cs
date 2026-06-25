using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ErtTariffAgg;
using Coreapi.Domain.SeedWork;
using Coreapi.Application.Features.ElectProjects.Commands.CreateBigProjectChildren;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Upsert;

public class UpsertElectProjectCommandHandler(
    IElectProjectRepository electProjectRepository,
    IElectProjectErtFormRepository projectErtFormRepository, 
    IBuildingTariffRepository buildingTariffRepository, 
    ICurrentUser currentUser,
    IClientRepository clientRepository,
    IInvoiceRepository invoiceRepository, 
    ISmsService smsService, 
    IErtTariffRepository ertTariffRepository,
    IMediator mediator
    ):
    IRequestHandler<UpsertElectProjectCommand,string>
{

    
    public async Task<string> Handle(UpsertElectProjectCommand request, CancellationToken cancellationToken)
    {
        var electRequestNumber = request.ElectRequestNumber;
        // throw new NotFoundException("سیستم در حال به روز رسانی می باشد و پرونده قابل ثبت نیست");

        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        long payWithSms = 0;
        long sumAmountService = 0;
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }
        if (request.LandlordName == null || request.LandlordName.Length < 6) throw new NotFoundException("نام مالک نباید کمتر از 6 حرف باشد");

        if (request.NumberOfFloor <= 0) throw new NotFoundException("تعداد طبقات نمی تواند صفر باشد");
        if (String.IsNullOrEmpty(request.Address) || request.Address.Length <= 10) throw new NotFoundException("آدرس نمیتواند کمتر از 10 کاراکتر باشد");


        if (request.IsTestAndDelivery &&
            (request.IsBuildingInspection ||
             request.IsEarthSystem ||
             request.IsErtTest))
            throw new NotFoundException("پرونده تست و تحویل نمیتواند بازرسی یا مجری ارت فعال داشته باشد");


        if (request.IsTestAndDelivery && (request.ChildInspectionCount + request.ChildErtCount) > 0)
        {
            throw new NotFoundException("در پرونده های بزرگ تست و تحویل فعال نشده است");
        }
        if(request.ChildInspectionCount>0 && request.IsBuildingInspection is false)
            throw new NotFoundException("برای پرونده های بزرگ بازرسی تیک بازرسی باید زده شود");

        if (request.ChildErtCount > 0 && request.IsEarthSystem is false)
            throw new NotFoundException("برای پرونده های بزرگ ارت تیک ارت باید زده شود");


        if (request.ChildInspectionCount > 0 && request.ChildErtCount == 0 && request.IsEarthSystem is true)
            throw new NotFoundException("وقتی که پرونده بزرگ می باشد و تعداد ارت صفر زدید نمیتوانید تیک ارت را بزنید");

        if (request.ChildErtCount > 0 && request.ChildInspectionCount == 0 && request.IsBuildingInspection is true)
            throw new NotFoundException("وقتی که پرونده بزرگ می باشد و تعداد بازرسی صفر زدید نمیتوانید تیک بازرسی را بزنید");


        var fileNumber = await electProjectRepository.GetLastFileNumber(Guid.Parse(currentUser.ClientId),1405) + 1;
        var fileNumberChild = fileNumber + 1;

        var buildingTariff =
            await buildingTariffRepository.GetByType(request.BuildingGroupTypeEnum,
                request.BuildingGroupParameterTypeEnum);

        var ertTariff = await ertTariffRepository.GetByType(request.ErtSystemTypeEnum);
        if (request.IsEarthSystem && ertTariff == null)
            throw new NotFoundException("برای پرونده ارت تعرفه ارت وجود ندارد");

        if (electRequestNumber != 0)
        {
            if (!currentUser.Role.Contains("ElectAdmin"))
                throw new NotFoundException("کاربری شما مجوز وارد کردن شماره تقاضا را ندارد لطفا ");
            if (electRequestNumber.ToString().Length != 10)
                throw new NotFoundException("شماره تقاضا باید 10 رقمی باشد");
        }
        else
        {
            electRequestNumber = fileNumber;
        }

        var isExist = await electProjectRepository.CheckIndexElectRequestNumber(client.Id, electRequestNumber);
        if (isExist > 0)
            throw new NotFoundException($"برای این شماره تقاضا قبلا برای شماره تقاضا {isExist} ثبت شده است");

        var electProject = new ElectProject(
                fileNumber, currentUser.UserId, 
                client, request.Area, request.NumberOfFloor, 
                request.DesNumberOfFloor, request.Address, 
                request.PostalCode, request.LandlordName, 
                request.LandlordNaCode, request.LandlordPhoneNumber, 
                request.CompanyName, request.LicenseNumber, 
                request.Description, request.IdSection,
                request.IdCity,request.IdProvince, 
                request.Lat, request.Lng, request.ProjectCreatedTypeEnum,
                request.ProjectTypeRequestEnum, buildingTariff,request.IsEarthSystem? ertTariff:null,
                ProjectLevelEnum.NullStage, DateTime.Now.Date,
                Helper.MiladiToShamsi(DateTime.Now.Date),
                request.IsEarthSystem,request.IsErtTest,
                request.IsBuildingInspection,request.IsTestAndDelivery, 
                request.PanelNeed, request.FoundationElectrodeArea,
                request.IsNeedEb,electRequestNumber,
                request.HasRelatedPermit,
                request.HasSupervision,
                request.AreaAsBuilt
                );
                electProjectRepository.Add(electProject);

                var ertForm = new ElectProjectErtForm(electProject, "", 0, 0, "", ElectrodeUsageTypeEnum.None, "", ElectrodeExecuteTypeEnum.None, "", ElectrodeTypeEnum.None, "",
                    ElectrodeMaterialTypeEnum.None, "", "", "", "", "", "", "", "", "", "", "", "", "");
                projectErtFormRepository.Add(ertForm);

        // اگر تعرفه ساختمان مشخص باشد
        if (buildingTariff is not null)
        {
            if (request.Area <= 0) throw new NotFoundException("مساحت نمی تواند صفر باشد");


            // اگر پرونده انشعاب دائم /موقت باشد
            if (request.ProjectTypeRequestEnum == ProjectTypeRequestEnum.Pr0)
            {
                // اگر پرونده بازرسی داشته باشد
                if (request.IsBuildingInspection)
                {
                    // دریافت هزینه بازرسی
                    var amountPayInspection = Helper.GetAmountInspection(buildingTariff.Tariff,request.Area,buildingTariff.Factor,buildingTariff.MinTariff);


					// دریافت تعرفه نظارت
					var amountPaySupervision = Helper.GetAmountSupervision(buildingTariff.SupervisionTariff,
						request.AreaAsBuilt, buildingTariff.SupervisionFactor);
                    if (request.HasSupervision)
                    {
                        if (request.AreaAsBuilt is 0)
                            throw new NotFoundException("مساحت ازبیلت نمیتواند صفر باشد باید وارد شود");
                    }
                    else {
                        amountPaySupervision = 0;
					}


					// دریافت هزینه خدمات بازرسی 
					sumAmountService += Helper.GetAmountProjectServices(amountPayInspection + amountPaySupervision);


                    // اگر پرونده فرزند نداشته باشد باشد **همچنین هزینه خدمات در آخر سر محاسبه میشود
                    if (request.ChildInspectionCount == 0 && request.ChildErtCount == 0)
                    {
                        // ایجاد فاکتور برای ایجاد پرونده بازرسی
                        var invoiceInspection = new Invoice(client, electProject, amountPayInspection + amountPaySupervision, InvoiceStatusEnum.Pending,
                            InvoicePayTypeEnum.CreateProjectStage);
                        invoiceRepository.Add(invoiceInspection);

                        // ایجاد تراکنش برداشت مرحله ایجاد پرونده برای بازرسی
                        // اگر پرونده بزرگ باشد مقدا مبلغ تراکنش صفر میشود چون در تراکنش فرزندانش کسر میشود
                        var transaction = new Transaction(invoiceInspection.Amount, client, client.Id.ToString(),
                            GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                            Helper.MiladiToShamsi(DateTime.Now.Date), electProject.FileNumber.ToString(),
                            $"برداشت ایجاد پرونده بازرسی:{electProject.FileNumber}", electProject.Id.ToString());
                        invoiceInspection.Done(transaction);
                        payWithSms += invoiceInspection.Amount;

					}

                }

                // اگر پرونده ارت داشته باشد
                if (request.IsEarthSystem)
                {
                   

                    var amountPayErtSystem = Helper.GetAmountErtSystemType(ertTariff.ErtSystemTypeEnum, ertTariff.Tariff, ertTariff.Factor, electProject.FoundationElectrodeArea);

					// دریافت هزینه خدمات ارت 
					sumAmountService += Helper.GetAmountProjectServices(amountPayErtSystem);


					// اگر پرونده فرزند نداشته باشد باشد هزینه خدمات بعدا از کلش محاسبه میشه
					if (request.ChildErtCount == 0 && request.ChildInspectionCount == 0 )
                    {
                        // ایجاد فاکتور مرحله 7 درصد نظام
                        var invoiceErtSystem = new Invoice(client, electProject, amountPayErtSystem, InvoiceStatusEnum.Pending,
                            InvoicePayTypeEnum.NezamStage);
                        invoiceRepository.Add(invoiceErtSystem);

                        // ایجاد تراکنش برداشت مرحله ایجاد پرونده ارت 
                        var transaction = new Transaction(amountPayErtSystem, client, client.Id.ToString(),
                            GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                            Helper.MiladiToShamsi(DateTime.Now.Date), electProject.FileNumber.ToString(),
                            $"برداشت 9 درصد نظام:ارت:{electProject.FileNumber}", electProject.Id.ToString());
                        invoiceErtSystem.Done(transaction);
                        payWithSms += amountPayErtSystem;
                    }


                }

                // -- ایجاد زیرپرونده های پرونده بزرگ (بازرسی و/یا ارت) --
                if (request.ChildInspectionCount > 0 || request.ChildErtCount > 0)
                {
                    var childrenPay = await mediator.Send(new CreateBigProjectChildrenCommand(
                        electProject, client, buildingTariff,
                        request.IsEarthSystem ? ertTariff : null,
                        request.ChildInspectionCount, request.ChildErtCount,
                        fileNumberChild,
                        request.Area, request.AreaAsBuilt, request.NumberOfFloor,
                        request.Address, request.PostalCode, request.LandlordName,
                        request.LandlordNaCode, request.LandlordPhoneNumber, request.LicenseNumber,
                        request.IdSection, request.HasSupervision, request.IsEarthSystem,
                        request.IsTestAndDelivery, request.PanelNeed, request.FoundationElectrodeArea,
                        request.IsNeedEb, request.HasRelatedPermit
                    ), cancellationToken);
                    payWithSms += childrenPay;
                }

            }




            // اگر پرونده تست و تحویل باشد
            if (request.ProjectTypeRequestEnum == ProjectTypeRequestEnum.Pr2 && request.IsTestAndDelivery)
            {
                var amountPayTestAndDelivery = Helper.GetAmountTestAndDelivery(buildingTariff.Tariff, request.Area, buildingTariff.TestAndDeliveryFactor);

                electProject.UpdateSupervisor(request.SupervisorName,request.SupervisorPhoneNumber);

                var invoiceTestAndDelivery = new Invoice(client, electProject, amountPayTestAndDelivery, InvoiceStatusEnum.Pending,
                    InvoicePayTypeEnum.CreateProjectStage);
                invoiceRepository.Add(invoiceTestAndDelivery);

                // ایجاد تراکنش برداشت مرحله ایجاد پرونده در هنگام تخصیص
                var transaction = new Transaction(amountPayTestAndDelivery, client, client.Id.ToString(),
                    GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                    Helper.MiladiToShamsi(DateTime.Now.Date), electProject.FileNumber.ToString(),
                    $"برداشت ایجاد پرونده تست و تحویل:{electProject.FileNumber}", electProject.Id.ToString());
                invoiceTestAndDelivery.Done(transaction);
                //payWithSms += amountPayTestAndDelivery;
            }




            if (request.ChildInspectionCount == 0 && request.ChildErtCount == 0 && !request.IsTestAndDelivery) {

                if(sumAmountService <= 0) throw new NotFoundException("هزینه خدمات نمی تواند صفر باشد");
				// ایجاد کردن فاکتور خدمات
				var invoiceServices = new Invoice(
					client, electProject, sumAmountService,
					InvoiceStatusEnum.Pending,
					InvoicePayTypeEnum.ProjectServices);
				invoiceRepository.Add(invoiceServices);
				// ایجاد تراکنش برداشت خدمات پرونده 
				var transactionServices = new Transaction(sumAmountService, client, client.Id.ToString(),
					GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
					Helper.MiladiToShamsi(DateTime.Now.Date), electProject.FileNumber.ToString(),
					$"برداشت هزینه خدمات:{electProject.FileNumber}", electProject.Id.ToString());
				invoiceServices.Done(transactionServices);

				payWithSms += sumAmountService;
			}
        }



        if (!currentUser.Role.Contains("ElectAdmin"))
        {
            if (buildingTariff is null) throw new NotFoundException("گروه ساختمانی مشخص نیست");
            if (payWithSms <= 0 && (!request.IsTestAndDelivery)) throw new NotFoundException("خطا در محاسبه پرداختی");
        }

        await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        
        // ارسال پرونده به مالک
        if (electProject.IsTestAndDelivery)
        {
            //await smsService.SendSms3Params(electProject.LandlordPhoneNumber, 9385,electProject.SupervisorName+"-"+electProject.SupervisorPhoneNumber, electProject.SolarRegisterDate,
            //    electProject.FileNumber.ToString());

        }
        else
        {
            await smsService.SendSms3Params(electProject.LandlordPhoneNumber, 10558, electProject.SolarRegisterDate,
                electProject.FileNumber.ToString(),electProject.ElectRequestNumber.ToString());

        }

        // ارسال بارگذاری مدارک توسط مالک
        var encodeGuid = Helper.EncodeGuid(electProject.Id);

        var param2 = $"e={encodeGuid}";

        await smsService.SendSms3Params(
            electProject.LandlordPhoneNumber,
            14646,
            electProject.FileNumber.ToString(),
            electProject.ElectRequestNumber + " پشتیبانی:" + "09372180164",
            param2);
        // ارسال پیام واریزی به مالک
        if (payWithSms <= 0) return electProject.Id.ToString();

        var param = $"e={encodeGuid}&a={payWithSms}";
        await smsService.SendSms4Params(electProject.LandlordPhoneNumber, 9593, electProject.FileNumber.ToString(), Helper.MiladiToShamsi(DateTime.Now),
            payWithSms.ToString("N0") + "ریال", param);



        return electProject.Id.ToString();

    }
}