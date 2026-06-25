using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CheckListForm = Coreapi.Common.ViewModels.CheckListForm;
using CommentEngForm = Coreapi.Common.ViewModels.CommentEngForm;

namespace Coreapi.Application.Features.ElectProjects.Commands.SubmitByAdmin
{
    public class SubmitByAdminCommandHandler(
        ICurrentUser currentUser,
        IClientRepository clientRepository,
        IElectProjectRepository electProjectRepository,
        IElectProjectFileRepository electProjectFileRepository,
        ISmsService smsService,
        IReportService reportService,
        IS3Service s3Service,
        IElectProjectProcessRepository electProjectProcessRepository,
        ICommentEngFormRepository commentEngFormRepository,
        ICheckListFormRepository checkListFormRepository,
        IEngineerRepository engineerRepository
        )
        : IRequestHandler<SubmitByAdminCommand, string>
    {
        public async Task<string> Handle(SubmitByAdminCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var electProject = await electProjectRepository.GetElectProjectById(request.Id);
            if (electProject is null) throw new NotAllowedException(" پرونده ای  با این شماره وجود ندارد ");
            if (electProject.IsStop) throw new NotFoundException("این پرونده متوقف شده است و قابل تایید نیست");

            // دریافت امضای مدیر
            var signAdminPath = $"Upload\\UserFiles\\{currentUser.UserId}\\F2.png";
            var signAdminPathS3 = await s3Service.GetFile(signAdminPath.Replace(@"\", "/"));
            if (signAdminPathS3 is null) throw new NotFoundException("لطفا مهر و امضای خود را در قسمت فایل های من بارگذاری کنید");

            // دریافت فایل های مربوط به این پرونده
            var electProjectFiles = await electProjectFileRepository.GetByIdElectProject(electProject.Id);
            if (electProjectFiles is null) throw new NotFoundException("فایل های این پرونده وجود ندارد");
            var approvedSentToElectFilesExist = electProjectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.ApprovedSentToElect);



		


			if (electProject.IsBigProject)
            {
                // برای پرونده های بزرگ
                var listEngName = "";
                var workPermitNum = "";
                var childProjects = electProject.ChildProjects;
                var engineersAndImages = new List<(Engineer, byte[])>();
                var comments = new List<CommentEngForm>();
                var checkList = new List<CheckListForm>();

                foreach (var childProject in childProjects)
                {
                    var childProcess = await electProjectProcessRepository.GetElectProjectProcessByEpId(childProject.Id);
                    var approvedChildProcess = childProcess.SingleOrDefault(c => c.InspectionStatusEnum == InspectionStatusEnum.Done && c.ProjectLevelEnum == ProjectLevelEnum.ExpertStage);
                    if (approvedChildProcess is not null)
                    {
                        listEngName += approvedChildProcess.Engineer.FullName + " و ";
                       var getWorkPermitNum = await engineerRepository.GetLastWorkPermitNum(approvedChildProcess.Engineer.Id);
                       workPermitNum += getWorkPermitNum + "-";


                        // دریافت امضای کارشناس
                        var signEngineerPathChild = $"Upload\\UserFiles\\{approvedChildProcess.Engineer.UserId}\\F2.png";
                        var signEngineerPathS3Child = await s3Service.GetFile(signEngineerPathChild.Replace(@"\", "/"));
                        if (signEngineerPathS3Child is null) throw new NotFoundException($"مهر و امضای کارشناس {approvedChildProcess.Engineer.FullName} بارگذاری نشده");
                        engineersAndImages.Add((approvedChildProcess.Engineer, signEngineerPathS3Child));

                        if (approvedChildProcess.IsMain)
                        {
                            var comments1 = await commentEngFormRepository.GetCommentEngForm(childProject.Id, approvedChildProcess.Engineer.Id);
                            comments = comments1.ToList();
                            if (comments is null) throw new NotFoundException("فرم شماره 3 خالی می باشد");

                            var checkList1 = await checkListFormRepository.GetCheckLIstEngForm(childProject.Id, approvedChildProcess.Engineer.Id);
                            checkList = checkList1.ToList();
                            if (checkList is null) throw new NotFoundException("چک لیست خالی می باشد");

                         

                        }

                    }
             
                }
                if (listEngName == "") throw new NotFoundException("تخصیص کارشناسی تایید شده وجود ندارد");

                // ایجاد فرم شماره 3 پی دی اف
                await reportService.GetApprovedCommentFormBigProject(engineersAndImages, comments, electProject);


                // ایجاد پی دی اف چک لیست
                await reportService.GetApprovedCheckListFormBigProject(engineersAndImages, checkList, electProject,
                    workPermitNum);

                await reportService.GetApprovedSentToElectForm(signAdminPathS3, electProject, listEngName, "شرکت توزیع برق");


                // چک کردن اینکه فایل فرم شماره 3 وجود دارد اگر ندارد ایجادش میکند در دیتابیس
                var approvedFilesExist = electProjectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.ApprovedComment);
                var approvedCheckListFilesExist = electProjectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.ApprovedCheckList);


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


            }
            else
            {
                var electProcess = await electProjectProcessRepository.GetElectProjectProcessByEpId(electProject.Id);
                if (electProcess is null) throw new NotFoundException("هیچ تخصیصی وجود ندارد");
                var approvedProcess = electProcess.SingleOrDefault(c => c.InspectionStatusEnum == InspectionStatusEnum.Done && c.ProjectLevelEnum == ProjectLevelEnum.ExpertStage);
                if (approvedProcess is null) throw new NotFoundException("تاییدیه کارشناس وجود ندارد");

				var comments = await commentEngFormRepository.GetCommentEngForm(electProject.Id, approvedProcess.Engineer.Id);
				if (comments is null) throw new NotFoundException("فرم شماره 3 خالی می باشد");

				var checkList = await checkListFormRepository.GetCheckLIstEngForm(electProject.Id, approvedProcess.Engineer.Id);
				if (checkList is null) throw new NotFoundException("چک لیست خالی می باشد");



				// دریافت امضای کارشناس
				var signEngineerPath = $"Upload\\UserFiles\\{approvedProcess.Engineer.UserId}\\F2.png";
				var signEngineerPathS3 = await s3Service.GetFile(signEngineerPath.Replace(@"\", "/"));
				if (signEngineerPathS3 is null) throw new NotFoundException("لطفا مهر و امضای خود را در قسمت فایل های من بارگذاری کنید");


				// ایجاد پی دی اف فرم نامه ارسال به شرکت توزیع

				await reportService.GetApprovedSentToElectForm(signAdminPathS3, electProject, approvedProcess.Engineer.FullName, "شرکت توزیع برق");


				var approvedFilesExist = electProjectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.ApprovedComment);

				var workPermitNum = await engineerRepository.GetLastWorkPermitNum(approvedProcess.Engineer.Id);
				var approvedCheckListFilesExist = electProjectFiles.FirstOrDefault(c => c.FileTypeEnum == FileTypeEnum.ApprovedCheckList);


				// ایجاد فرم شماره 3 approved-comment-form.pdf اف
				await reportService.GetApprovedCommentForm(signEngineerPathS3, comments, electProject, approvedProcess.Engineer);

				// ایجاد پی دی اف approved-check-list-form.pdf چک لیست
				await reportService.GetApprovedCheckListForm(signEngineerPathS3, checkList, electProject, approvedProcess.Engineer,
					workPermitNum, approvedProcess.SolarDateDeliverEngineer);


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
						approvedProcess.Engineer.UserId,
						approvedProcess.Engineer.UserId,
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
						approvedProcess.Engineer.UserId,
						approvedProcess.Engineer.UserId,
						electProject);
					electProjectFileRepository.Add(electProjectFile);
				}






			}


            // چک کردن اینکه فایل تاییدیه وجود دارد اگر ندارد ایجادش میکند در دیتابیس
            if (approvedSentToElectFilesExist is not null)
            {
                approvedSentToElectFilesExist.UpdateProjectFile("approved-send-to-elect.pdf", electProject);
            }
            else
            {
                var electProjectFile = new ElectProjectFile(
                    "approved-send-to-elect.pdf" + "-" + electProject.Id,
                    "approved-send-to-elect",
                    FileTypeEnum.ApprovedSentToElect,
                    electProject.Id.ToString(),
                    "approved-send-to-elect.pdf",
                    currentUser.UserId,
                    currentUser.UserId,
                    electProject);
                electProjectFileRepository.Add(electProjectFile);
            }

            // در هنگام تایید توسط نظام به مرحله در انتظار شرکت توزیع میرود
            electProject.SubmitByAdmin();
            electProject.UpdateCountSend(electProject.CountSendToElectCo+1);

            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await smsService.SendSms2Params(electProject.LandlordPhoneNumber, 9857,
                Helper.MiladiToShamsiForSms(DateTime.Now.Date), electProject.ElectRequestNumber == 0 ? electProject.FileNumber.ToString() : electProject.ElectRequestNumber.ToString());

            return electProject.Id.ToString();
        }
    }
}
