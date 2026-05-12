using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.AddFile;

public class AddElectProjectFileCommandHandler(
    IElectProjectRepository electProjectRepository,
    IElectProjectFileRepository electProjectFileRepository,
    ICurrentUser currentUser,
    IFileService fileService,
    IUserManager userManager,
    IS3Service s3Service,
    IElectProjectProcessRepository electProjectProcessRepository)
    : IRequestHandler<AddElectProjectFileCommand, List<Guid>>
{
    private readonly IFileService fileService = fileService;
    private readonly IUserManager userManager = userManager;

    public async Task<List<Guid>> Handle(AddElectProjectFileCommand request, CancellationToken cancellationToken)

    {
        var electIds = new List<Guid>();
        if (request.File is null) throw new NotAllowedException("فایل آپلود نشده است");
        var listGuid = JsonSerializer.Deserialize<List<Guid>>(request.ElectProjectId[0]);
        if (listGuid is null) throw new NotAllowedException("پرونده ای وجود ندارد");
        var fileName = request.FileName.Split('.')[0] + Helper.MiladiToShamsiFileName(DateTime.Now) + "." + request.FileName.Split('.')[1];
        var fileType = request.FileName.Split('.')[1].ToUpper();

        if (!currentUser.Role.Contains("Administrator") && !currentUser.Role.Contains("Section") && !currentUser.Role.Contains("ElectAdmin"))
        {
            if (request.FileTypeEnum is FileTypeEnum.ElectPlan or FileTypeEnum.ErtMap or FileTypeEnum.RelatedPermit)
                throw new NotFoundException("دسترسی جهت آپلود فایل ندارید");
        }


        //var getFiles = await electProjectFileRepository.GetByFileType(FileTypeEnum.None);
        //foreach (var file in getFiles)
        //{

        //    await s3Service.DeleteDuplicateFiles("Upload/ElectProjects" + "/" + file.FolderName, file.FileName);
        //}

        foreach (var guid in listGuid)
        {



            var electProject = await electProjectRepository.GetElectProjectById(guid);
            if (electProject == null) throw new NotFoundException(nameof(electProject), guid);

            // چک کردن فایل قابل آپلود برای کارشناس و مجری
            if (currentUser.Role.Contains("Engineer"))
            {
                var projectProcess = await electProjectProcessRepository.GetElectProjectProcessByEpId(guid);
                var findProcess = projectProcess.FirstOrDefault(w => w.Engineer.UserId == currentUser.UserId);

                if (findProcess is null) throw new NotFoundException("فقط کارشناس مربوطه میتواند فایل آپلود کند");


                if (findProcess.ProjectLevelEnum is ProjectLevelEnum.ErtStage)
                {
                    // اگر مجری ارت بخواهد برای پرونده اش فایل آپلود کند
                    // چک میشود که فقط بتواند چیدمان الکترود و مستندات مجری ارت آپلود کند
                    if ( request.FileTypeEnum != FileTypeEnum.CrookyOfElectrode 
                        && request.FileTypeEnum!= FileTypeEnum.ErtDocument)
                    {
                        throw new NotFoundException("فقط به کروکی،چیدمان الکترود و مستندات مجری ارت دسترسی دارید ");

                    }
                }

                if (findProcess.ProjectLevelEnum is ProjectLevelEnum.ExpertStage )
                {
                    // اگر بازرسی بخواد برای پرونده اش فایل آپلود کند
                    if (request.FileTypeEnum != FileTypeEnum.ElectNetwork 
                        && request.FileTypeEnum != FileTypeEnum.Crooky
                        && request.FileTypeEnum != FileTypeEnum.SupervisorApproveForm 
                        && request.FileTypeEnum != FileTypeEnum.ExpertDocument
                        && request.FileTypeEnum != FileTypeEnum.CheckListBoard
                        && request.FileTypeEnum != FileTypeEnum.SupervisionDocument
                        && request.FileTypeEnum != FileTypeEnum.AzbuiltMap
                       )
                    {
                        throw new NotFoundException("برای آپلود این نوع فایل دسترسی ندارید");

                    }
                }

                if (findProcess.ProjectLevelEnum is ProjectLevelEnum.TestDeliveryStage)
                {
                    // اگر بازرسی بخواد برای پرونده اش فایل آپلود کند
                    if (request.FileTypeEnum != FileTypeEnum.TestAndDeliveryDocument &&
                        request.FileTypeEnum != FileTypeEnum.TestAndDelivery)
                    {
                        throw new NotFoundException("فقط میتوانید مستندات تست و تحویل آپلود کنید");

                    }
                }
            }


            if (request.FileTypeEnum == FileTypeEnum.CrookyOfElectrode && fileType != "PNG" && fileType != "JPG" &&
                fileType != "JPEG")
                throw new NotFoundException("کروکی چیدمان الکترودها باید فرمت عکس باشد ");

            await s3Service.DeleteFile("Upload/" + request.FolderName + "/" + guid, request.FileName.Split('.')[0]);

            await s3Service.UploadFileAttach(request.File, fileName, request.FolderName, guid.ToString());

          

            var electProjectFiles = await electProjectFileRepository.GetByIdElectProject(guid);
            var electProjectFilesExist = electProjectFiles.FirstOrDefault(c => c.FileTypeEnum == request.FileTypeEnum);
            if (electProjectFilesExist is not null)
            {
                electProjectFilesExist.UpdateProjectFile(fileName.Split('.')[0] + "." + fileName.Split('.')[1], electProject);
                electIds.Add(electProjectFilesExist.Id);
            }
            else
            {
                var electProjectFile = new ElectProjectFile(
                request.Name + "-" + guid.ToString(),
                    request.Des,
                    request.FileTypeEnum,
                    guid.ToString(),
                    fileName.Split('.')[0] + "." + fileName.Split('.')[1],
                    currentUser.UserId,
                    request.ToUserId,
                    electProject);
                electProjectFileRepository.Add(electProjectFile);
                electIds.Add(electProjectFile.Id);
            }

            // اضافه کردن به همه زیر پرونده ها
            foreach (var childProject in electProject.ChildProjects)
            {
          

                var electProjectFilesChild = await electProjectFileRepository.GetByIdElectProject(childProject.Id);
                var electProjectFilesExistChild = electProjectFilesChild.FirstOrDefault(c => c.FileTypeEnum == request.FileTypeEnum);
                if (electProjectFilesExistChild is not null)
                {
                    electProjectFilesExistChild.UpdateProjectFile(fileName.Split('.')[0] + "." + fileName.Split('.')[1], childProject);
                    electIds.Add(electProjectFilesExistChild.Id);
                }
                else
                {
                    var electProjectChildFile = new ElectProjectFile(
                        request.Name + "-" + childProject.Id,
                        request.Des,
                        request.FileTypeEnum,
                        childProject.Id.ToString(),
                        fileName.Split('.')[0] + "." + fileName.Split('.')[1],
                        currentUser.UserId,
                        request.ToUserId,
                        childProject);
                    electProjectFileRepository.Add(electProjectChildFile);
                    electIds.Add(electProjectChildFile.Id);
                }
                await s3Service.DeleteFile("Upload/electProjects/" + childProject.Id, request.FileName.Split('.')[0]);

                await s3Service.UploadFileAttach(request.File, fileName, request.FolderName, childProject.Id.ToString());

            }

            await electProjectFileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        return electIds;

    }
}