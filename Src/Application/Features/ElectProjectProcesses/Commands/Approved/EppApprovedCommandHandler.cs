using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Features.ElectProjectProcesses.Commands.DeleteProjectProcess;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.Approved;

public class EppApprovedCommandHandler(
    IElectProjectProcessRepository electProjectProcessRepository,
    IElectProjectRepository electProjectRepository,
    IInvoiceRepository invoiceRepository,
    ICurrentUser currentUser,
    IClientRepository clientRepository,
    IEngineerRepository engineerRepository,
    IQuarterTariffRepository quarterTariffRepository,
    IBuildingTariffRepository buildingTariffRepository,
    IElectProjectFileRepository electProjectFileRepository,
    IS3Service s3Service,
    IReportService reportService,
    ICommentEngFormRepository commentEngFormRepository,
    IElectProjectErtFormRepository electProjectErtFormRepository,
    ICheckListFormRepository checkListFormRepository,
    ISmsService smsService
    )
    : IRequestHandler<EppApprovedCommand, string>
{

    public async Task<string> Handle(EppApprovedCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        var projectProcess = await electProjectProcessRepository.GetElectProjectProcessById(Guid.Parse(request.EppId));
        if (projectProcess is null) throw new NotFoundException("این تخصیص وجود ندارد");

        //if (projectProcess.ProjectLevelEnum == ProjectLevelEnum.ApproveStage)
        //    throw new NotFoundException("قبلا اعلام نظر کارشناسی انجام شده است");

        //if (projectProcess.InspectionStatusEnum is not InspectionStatusEnum.Undefined)
        //    throw new NotFoundException("قبلا اعلام نظر شده است");

      


        // مشخص کردن کاربر جاری به عنوان کارشناس
        var engineer = await engineerRepository.getByUserId(currentUser.UserId);
        if (engineer is null) throw new NotFoundException("شما به عنوان کارشناس شناخته نشدید");
        if (projectProcess.Engineer.UserId != currentUser.UserId) throw new NotFoundException("این تخصیص مربوط به شما نمی باشد");



        var quarterTariff = await quarterTariffRepository.GetById(projectProcess.QuarterTariff.Id);
        if (quarterTariff is null) throw new NotFoundException("تخصیص های سه ماهه وجود ندارد");

        var electProject = await electProjectRepository.GetElectProjectById(projectProcess.ElectProject.Id);
        if (electProject is null) throw new NotFoundException("این پرونده وجود ندارد");

        var buildingTariff = await buildingTariffRepository.GetById(electProject.BuildingTariff.Id);

        var allProjectProcess = await electProjectProcessRepository.GetElectProjectProcessByEpId(electProject.Id);


        //if (electProject.ProjectLevelEnum == ProjectLevelEnum.ApproveStage || electProject.ProjectLevelEnum == ProjectLevelEnum.ApproveTestDeliveryStage)
        //    throw new NotFoundException("این پرونده قبلا تایید شده است");


        if (!electProject.SolvedDefectEng && electProject.IsDefectEng)
            throw new NotFoundException("ابتدا جهت رفع نقص تعیین وضعیت را مشخص کنید");

        if (electProject.IsOk)
            throw new NotFoundException("این پرونده به شرکت توزیع ارسال شده و اعلام نظر قابل ویرایش نیست");

        // اگر کارشناس کنسل را بزند
        if (request.InspectionStatusEnum is InspectionStatusEnum.Canceled)
        {

            if (electProject.ProjectLevelEnum == ProjectLevelEnum.Defect)
                throw new NotFoundException("پرونده که دارای نقص می باشد قابل کنسل نیست");
            var getProjectBalance = await electProjectRepository.GetProjectBalance(electProject.Id);
            if (getProjectBalance < 0)
            {
                throw new NotFoundException("بالانس پرونده منفی است و امکان کنسل وجود ندارد");
            }

            switch (projectProcess.ProjectLevelEnum)
            {

                case ProjectLevelEnum.ExpertStage:
                    {
                        if (projectProcess.Accepted) throw new NotFoundException("این پرونده را قبلا قبول کرده اید و قابل کنسل نیست ");

                        if (electProject.ProjectLevelEnum is ProjectLevelEnum.ApproveStage)
                            throw new NotFoundException("این تخصیص توسط مهندس تایید شده و قابل کنسل نیست");
                        electProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);

                        break;
                    }
                case ProjectLevelEnum.ErtStage:
                    {
                        if (projectProcess.Accepted && Helper.GetDiffDay(projectProcess.JulianDateAccepted, DateTime.Now) <= 30) throw new NotFoundException("این پرونده را قبلا قبول کرده اید و تا یک ماه قابل کنسل نیست ");


                        if (electProject.ProjectLevelEnum is ProjectLevelEnum.ApproveErtStage)
                            throw new NotFoundException("این تخصیص توسط مجری ارت تایید شده و قابل کنسل نیست");
                        // اگر کارشناس ارت از قبل این پرونده را کنسل کرده باشد نباید دوباره کنسل کند
                        if (projectProcess.InspectionStatusEnum == InspectionStatusEnum.Canceled)
                            throw new NotFoundException("این پرونده را قبلا کنسل کرده اید");
                        if (electProject.IsBuildingInspection) electProject.UpdateProjectLevel(ProjectLevelEnum.ExpertStage);
                        if (electProject.IsEarthSystem && !electProject.IsBuildingInspection) electProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);
                        break;
                    }
                case ProjectLevelEnum.ElectPanelStage:
                    {
                        if (electProject.PanelMakerSubmit)
                            throw new NotFoundException("تابلوی برق تایید شده و ابتدا باید تابلوساز تایید را بردارد");
                        electProject.UpdateProjectLevel(ProjectLevelEnum.ErtStage);
                        break;
                    }
                case ProjectLevelEnum.TestDeliveryStage:
                    {
                        if (electProject.ProjectLevelEnum is ProjectLevelEnum.ApproveTestDeliveryStage)
                            throw new NotFoundException("این تخصیص توسط مجری تست وتحویل تایید شده و قابل کنسل نیست");
                        electProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);
                        break;
                    }

                default:
                    throw new NotFoundException("مرحله پرونده مشکل دارد");
            }

        

            //await mediator.Send(new DeleteProjectProcessCommand()
            //{
            //    EppId = projectProcess.Id
            //}, cancellationToken);

            projectProcess.UpdateCancelExpertStage(DateTime.Now, Helper.MiladiToShamsi(DateTime.Now), request.Des, projectProcess.ProjectLevelEnum);

            await electProjectProcessRepository.UnitOfWork.SaveChangesAsync(cancellationToken);



            await smsService.SendSms4Params(electProject.LandlordPhoneNumber, 7396,
                "کنسل شدن پرونده شما توسط کارشناس آقای:", projectProcess.Engineer.FullName,
                "وضعیت پرونده:در انتظار مجدد تخصیص", Helper.MiladiToShamsiForSms(DateTime.Now.Date));

            return "ok";

        }

        if (!projectProcess.Accepted) throw new NotFoundException("ابتدا باید پرونده را قبول نمایید");

        // اگر از قبل کنسل ارت زده باشد نباید بتواند تایید کند
        if (projectProcess.InspectionStatusEnum == InspectionStatusEnum.Canceled)
        {
            throw new NotFoundException("پرونده ای که کنسل شده است قابل تایید نیست");
        }

        var projectFiles = await electProjectFileRepository.GetByIdElectProject(electProject.Id);
        var electPlanFile = projectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.ElectPlan);
        var relatedPermit = projectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.RelatedPermit);
        var checkListBoardFile = projectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.CheckListBoard);

        if (!electProject.IsTestAndDelivery)
        {
            if (electPlanFile is null || relatedPermit is null) throw new NotFoundException("نقشه برق یا جواز آپلود نشده ");

        }

        // دریافت امضای کارشناس
        var signEngineerPath = $"Upload\\UserFiles\\{engineer.UserId}\\F2.png";
        var signEngineerPathS3 = await s3Service.GetFile(signEngineerPath.Replace(@"\", "/"));
        if (signEngineerPathS3 is null) throw new NotFoundException("لطفا مهر و امضای خود را در قسمت فایل های من بارگذاری کنید");

        const string borderFilePath = $"Upload\\Nezam\\border1.png";
        var borderFileS3 = await s3Service.GetFile(borderFilePath.Replace(@"\", "/"));

        // دریافت فایل های مربوط به این پرونده
        var electProjectFiles = await electProjectFileRepository.GetByIdElectProject(electProject.Id);
        if (electProjectFiles is null) throw new NotFoundException("فایل های این پرونده وجود ندارد");

        var approvedFilesExist = electProjectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.ApprovedComment);
        var approvedCheckListFilesExist = electProjectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.ApprovedCheckList);
        var approvedErtFileExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.ApprovedErtExecutor);
        var electNetworkFileExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.ElectNetwork);
        var testAndDeliveryFileExist = electProjectFiles.FirstOrDefault(f=>f.FileTypeEnum == FileTypeEnum.TestAndDelivery);

        // فایل های لازم جهت تایید کارشناسی
        var expertDocumentExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum is FileTypeEnum.ExpertDocument);
        var supervisorApproveFormExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum is FileTypeEnum.SupervisorApproveForm);
        var crookyExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.Crooky);
        var supervisionDocumentExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum is FileTypeEnum.SupervisionDocument);
        var azbuiltMapExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum is FileTypeEnum.AzbuiltMap);

        // فایلها لازم جهت تایید مجری ارت
        var ertDocumentExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum is FileTypeEnum.ErtDocument);


        // تایید کردن مرحله کارشناسی اصلی
        if (projectProcess.ProjectLevelEnum == ProjectLevelEnum.ExpertStage)
        {
                // چک کردن اینکه اگه پرونده نیاز به مجری ارت داشته باشد ابتدا باید مجری آن را تایید کند
                var hasDoneErtStage = allProjectProcess.Any(a => a.ProjectLevelEnum == ProjectLevelEnum.ErtStage &&
                                           a.InspectionStatusEnum == InspectionStatusEnum.Done);
                if (electProject.IsEarthSystem)
                {
                    if(!hasDoneErtStage) throw new NotFoundException("این پرونده ابتدا باید مجری ارت آن را تایید کند");
                }


                if (!electProject.IsBuildingInspection)
                {
                    throw new NotFoundException("برای این پرونده بازرسی ساختمان وجود ندارد");
                }



                // چک کردن تابلو ساز
                //if (electProject.PanelNeed)
                //{
                //    if (!electProject.PanelMakerSubmit) throw new NotFoundException("این پرونده نیاز به تایید تابلو ساز دارد");
                //}

                // چک کردن فایل های لازم جهت تایید بازرسی


                if (expertDocumentExist is null)
                {
                    throw new NotFoundException("فایل مستندات بازرس برق آپلود نشده است");
                }

                if (crookyExist is null)
                {
                    throw new NotFoundException("فایل کروکی آپلود نشده است");
                }

                if (supervisorApproveFormExist is null)
                {
                    throw new NotFoundException("فایل فرم تایید ناظر آپلود نشده است");
                }

                if (supervisionDocumentExist is null && electProject.HasSupervision)
                {
                    throw new NotFoundException("قایل مستندات نظارت آپلود نشده است");
                }


                if (azbuiltMapExist is null && electProject.HasSupervision)
                {
                    throw new NotFoundException("فایل نقشه ازبیلت آپلود نشده است");
                }

                // آپدیت کردن نیاز به احداث شبکه
            electProject.UpdateNeedElectNetwork(request.NeedElectNetwork);

                // اگر نیازه به احداث شبکه باشد باید قبلش فایل کروکی احداث شبکه آپلود شود
                if (request.NeedElectNetwork)
                {
                    if (electNetworkFileExist is null)
                        throw new NotFoundException("فایل کروکی احداث شبکه را آپلود نمایید");
                }

                // آپدیت کردن خود پرونده به تایید شده توسط کارشناس
                electProject.UpdateProjectLevel(ProjectLevelEnum.ApproveStage);

                // آپدیت کردن پروسس به استیت تایید شده توسط کارشناس در مرحله کارشناسی
                projectProcess.UpdateDoneExpertStage(DateTime.Now.Date, Helper.MiladiToShamsi(DateTime.Now.Date), request.Des);


            


                var comments = await commentEngFormRepository.GetCommentEngForm(electProject.Id,engineer.Id);
                if (comments is null) throw new NotFoundException("فرم شماره 3 خالی می باشد");

                var checkList = await checkListFormRepository.GetCheckLIstEngForm(electProject.Id, engineer.Id);
                if (checkList is null) throw new NotFoundException("چک لیست خالی می باشد");

                

            var workPermitNum = await engineerRepository.GetLastWorkPermitNum(engineer.Id);

                // ایجاد فرم شماره 3 پی دی اف
                await reportService.GetApprovedCommentForm(signEngineerPathS3, comments,electProject,engineer);

                // ایجاد پی دی اف چک لیست
                await reportService.GetApprovedCheckListForm(signEngineerPathS3, checkList, electProject, engineer,
                    workPermitNum, projectProcess.SolarDateDeliverEngineer);

  
            // چک کردن اینکه فایل فرم شماره 3 وجود دارد اگر ندارد ایجادش میکند در دیتابیس
            var crookyFileExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.Crooky);
                if (crookyFileExist is null) throw new NotFoundException("فایل کروکی وجود ندارد");
                
                
                if (approvedFilesExist is not null)
                {
                    approvedFilesExist.UpdateProjectFile("approved-comment-form.pdf", electProject);
                }
                else
                {
                    var electProjectFile = new ElectProjectFile(
                        "approved-comment-form.pdf" + "-" + electProject.Id,
                        "approved-comment-form",
                        FileTypeEnum.ApprovedComment,
                        electProject.Id.ToString(),
                        "approved-comment-form.pdf",
                        currentUser.UserId,
                        currentUser.UserId,
                        electProject);
                    electProjectFileRepository.Add(electProjectFile);
                }

                // چک کردن اینکه فایل چک لیست وجود دارد اگر ندارد ایجادش میکند در دیتابیس
                if (approvedCheckListFilesExist is not null)
                {
                    approvedCheckListFilesExist.UpdateProjectFile("approved-check-list-form.pdf", electProject);
                }
                else
                {
                    var electProjectFile = new ElectProjectFile(
                        "approved-check-list-form.pdf" + "-" + electProject.Id,
                        "approved-check-list-form",
                        FileTypeEnum.ApprovedCheckList,
                        electProject.Id.ToString(),
                        "approved-check-list-form.pdf",
                        currentUser.UserId,
                        currentUser.UserId,
                        electProject);
                    electProjectFileRepository.Add(electProjectFile);
                }




            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                // اگر این پرونده زیرمجموعه پرونده بزرگ است و همه برادرها به مرحله تایید-نظام رسیده اند
                // پرونده والد را هم به مرحله تایید-نظام ببر
                await RollupParentApproveStageAsync(electProject, cancellationToken);

                return projectProcess.Id.ToString();

        }

        // مرحله تایید مجری ارت
        if (projectProcess.ProjectLevelEnum == ProjectLevelEnum.ErtStage)
        {
         

            var workPermitNum = await engineerRepository.GetLastWorkPermitNum(engineer.Id);

            // دریافت مبلغ تاییدیه ارت در دوره
            var ertApprovedFee = quarterTariff.ErtApprovedFee;
            if (ertApprovedFee > 0)
            {

                // دریافت فاکتور مربوط  به تایید ارت

                var invoiceErtApproved = await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
                InvoicePayTypeEnum.ErtApprovedStage);
                if (invoiceErtApproved is null)
                {
                
                        // ایجاد فاکتور برای تایید ارت
                        var invoice = new Invoice(client, electProject, ertApprovedFee, InvoiceStatusEnum.Pending,
                            InvoicePayTypeEnum.ErtApprovedStage);
                        invoiceRepository.Add(invoice);

                        // ایجاد تراکنش برداشت مرحله ایجاد پرونده برای بازرسی
                        // اگر پرونده بزرگ باشد مقدا مبلغ تراکنش صفر میشود چون در تراکنش فرزندانش کسر میشود
                        var transaction = new Transaction(ertApprovedFee, client, client.Id.ToString(),
                            GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                            Helper.MiladiToShamsi(DateTime.Now.Date), electProject.FileNumber.ToString(),
                            $"برداشت تاییدیه ارت:{electProject.FileNumber}", electProject.Id.ToString());
                        invoice.Done(transaction);

                        await invoiceRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                        return "ok";

                }
                else
                {
                    var getProjectBalance = await electProjectRepository.GetProjectBalance(electProject.Id);
                    if (getProjectBalance < 0)
                    {
                        throw new NotFoundException("بالانس پرونده منفی است و امکان تایید ارت وجود ندارد");
                    }
                }
            }


            // دریافت فاکتور مربوط به مرحله مجری ارت پرونده
            //var invoiceErtProject = await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
            //    InvoicePayTypeEnum.ErtStage);
            //if (invoiceErtProject is null) throw new NotFoundException("فاکتور مرحله مجری ارت ایجاد نشده است");
            //if (invoiceErtProject.Transaction is null)
            //    throw new NotFoundException("برای فاکتور مرحله مجری ارت پرونده تراکنشی وجود ندارد");

            var ertForm = await electProjectErtFormRepository.GetEpeFormByElectProjectId(electProject.Id);

             if (ertForm.Approved) throw new NotFoundException("قبلا تایید شده است");
            if (string.IsNullOrEmpty(ertForm.ConstructionDate) || string.IsNullOrEmpty(ertForm.MeasurementDate) 
                || string.IsNullOrEmpty(ertForm.ElectrodeAddress) || string.IsNullOrEmpty(ertForm.MeasurementMethod)
                || ertForm.ElectrodeExecuteTypeEnum == ElectrodeExecuteTypeEnum.None
                || ertForm.ElectrodeTypeEnum == ElectrodeTypeEnum.None
                || ertForm.ElectrodeUsageTypeEnum == ElectrodeUsageTypeEnum.None
                || ertForm.ElectrodeMaterialTypeEnum == ElectrodeMaterialTypeEnum.None)
                throw new NotFoundException("ابتدا فرم مربوط به شناسنامه ارت را تکمیل کنید");

            if (ertDocumentExist is null )
            {
                throw new NotFoundException("مستندا مجری ارت باید آپلود شود");
            }

            // دریافت کروکی الکترودها
            var crookyOfElectrodeFileExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.CrookyOfElectrode);
            if (crookyOfElectrodeFileExist is null)
                throw new NotFoundException("فایل کروکی چیدمان الکترودها را به صورت عکس بارگزاری کنید ");
            var crookyOfElectrodePath = $"Upload\\ElectProjects\\{electProject.Id}\\{crookyOfElectrodeFileExist.FileName}";
            var crookyOfElectrodePathS3 = await s3Service.GetFile(crookyOfElectrodePath.Replace(@"\", "/"));
            if (crookyOfElectrodePathS3 is null) throw new NotFoundException("خطای دریافت فایل کروکی چیدمان الکترودها");


            // ایجاد پی دی اف شناسنامه ارت
            await reportService.GetApprovedErtForm(signEngineerPathS3, ertForm, engineer, projectProcess.SolarDateDeliverEngineer, crookyOfElectrodePathS3,workPermitNum);

            
            // چک کردن اینکه فایل فرم ارت وجود دارد اگر ندارد ایجادش میکند در دیتابیس
            if (approvedErtFileExist is not null)
            {
                approvedErtFileExist.UpdateProjectFile("approved-ert-form.pdf", electProject);
            }
            else
            {
                var electProjectFile = new ElectProjectFile(
                    "approved-ert-form.pdf" + "-" + electProject.Id,
                    "approved-ert-form",
                    FileTypeEnum.ApprovedErtExecutor,
                    electProject.Id.ToString(),
                    "approved-ert-form.pdf",
                    currentUser.UserId,
                    currentUser.UserId,
                    electProject);
                electProjectFileRepository.Add(electProjectFile);
            }


            if(electProject.IsBuildingInspection){
				electProject.UpdateProjectLevel(ProjectLevelEnum.ApproveErtStage);
			}
            else{
				electProject.UpdateProjectLevel(ProjectLevelEnum.ApproveStage);
			}

			projectProcess.UpdateDoneErtStage(DateTime.Now.Date, Helper.MiladiToShamsi(DateTime.Now.Date), request.Des);
            // اساین کردن فاکتور به پروسس
            //invoiceErtProject.UpdateElectProjectProcess(projectProcess);

            // در اینجا اگر مجری ارت تایید بزند اما کارشناسی آن کنسل خورده باشد باید پرونده را به مرحله تخصیص به کارشناس ببرد
            var findExpertProcess =
                allProjectProcess.Count(w => w.ProjectLevelEnum == ProjectLevelEnum.ExpertStage);
            var findAllExpertProcessCanceled =
                allProjectProcess.Count(w => w.ProjectLevelEnum == ProjectLevelEnum.ExpertStage && w.InspectionStatusEnum is InspectionStatusEnum.Canceled);
            if(findExpertProcess>0 && findExpertProcess == findAllExpertProcessCanceled) electProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);


            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            // اگر این پرونده زیرمجموعه پرونده بزرگ است و همه برادرها به مرحله تایید-نظام رسیده اند
            // پرونده والد را هم به مرحله تایید-نظام ببر
            await RollupParentApproveStageAsync(electProject, cancellationToken);

            return projectProcess.Id.ToString();

        }

        // تایید کردن تست و تحویل
        if (projectProcess.ProjectLevelEnum == ProjectLevelEnum.TestDeliveryStage)
        {


            // چک کردن اینکه فایل تست و تحویل بارگذاری شده است
            if (testAndDeliveryFileExist is null) throw new NotFoundException("فایل تست و تحویل بارگذاری نشده است");

            // آپدیت کردن پرونده به مرحله تست و تحویل تایید شده
            electProject.UpdateProjectLevel(ProjectLevelEnum.ApproveTestDeliveryStage);
            projectProcess.UpdateDoneTestAndDeliveryStage(DateTime.Now.Date, Helper.MiladiToShamsi(DateTime.Now.Date), request.Des);


            // تایید کردن نهایی پرونده توسط کارشناس
            electProject.SubmitTestAndDelivery();
            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return projectProcess.Id.ToString();

        }



        return "ok";



    }

    // برای پرونده های زیرمجموعه پرونده بزرگ: اگر همه فرزندها به مرحله تایید-نظام رسیده باشند
    // پرونده والد را هم به همان مرحله ببر و ذخیره کن. فرزندهای حذف شده نادیده گرفته می شوند.
    private async Task RollupParentApproveStageAsync(ElectProject electProject, CancellationToken cancellationToken)
    {
        if (electProject.ParentProject is null) return;

        var parent = await electProjectRepository.GetElectProjectById(electProject.ParentProject.Id);
        if (parent?.ChildProjects is null) return;

        var activeChildren = parent.ChildProjects.Where(c => !c.IsDelete).ToList();
        if (activeChildren.Count == 0) return;

        if (activeChildren.All(c => c.ProjectLevelEnum == ProjectLevelEnum.ApproveStage))
        {
            parent.UpdateProjectLevel(ProjectLevelEnum.ApproveStage);
            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}