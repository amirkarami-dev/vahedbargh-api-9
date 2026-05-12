using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpsertErtForm;

public class UpsertErtFormCommandHandler(
    ICurrentUser currentUser,
    IClientRepository clientRepository,
    IElectProjectErtFormRepository electProjectErtFormRepository,
    IElectProjectProcessRepository processRepository,
    IElectProjectRepository projectRepository,
    IReportService reportService,
    IS3Service s3Service,
    IElectProjectFileRepository electProjectFileRepository)
    : IRequestHandler<UpsertErtFormCommand, string>
{
    public async Task<string> Handle(UpsertErtFormCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        var ertForm = await electProjectErtFormRepository.GetEpeFormById(Guid.Parse(request.Id));
        if (ertForm is null) throw new NotFoundException("فرم ارت ثبت نشده است");

        var electProject = await projectRepository.GetById(ertForm.ElectProject.Id);
        if (electProject is null) throw new NotFoundException("این پرونده وجود ندارد");

        var epp = await processRepository.GetElectProjectProcessById(Guid.Parse(request.EppId));
        if (epp is null) throw new NotFoundException("این تخصیص وجود ندارد");

        if (!epp.Accepted) throw new NotFoundException("ابتدا باید پرونده را قبول کنید");

        if (request.BuildingTypeEnum is BuildingTypeEnum.None)
            throw new NotFoundException("کاربری ساختمان را مشخص کنید");
        electProject.UpdateBuildingTypeEnum(request.BuildingTypeEnum);



        ertForm.Update(
            request.ElectrodeAddress,request.UtmX,request.UtmY,request.ConstructionDate,
            request.ElectrodeUsageTypeEnum,request.ElectrodeUsageTypeOther,
            request.ElectrodeExecuteTypeEnum,request.ElectrodeExecuteTypeOther,
            request.ElectrodeTypeEnum,request.ElectrodeTypeOther,
            request.ElectrodeMaterialTypeEnum,request.ElectrodeMaterialTypeOther,
            request.ElectrodeLength, request.ElectrodeDiameter,request.OtherElectrodeMeasure,
            request.Des,request.ErtBrand,request.ErtTesterBrand,request.MeasurementDate,
            request.MeasurementHour, request.Temperature,request.RainfallAmount,
            request.ElectrodeResistanceValue,request.MeasurementMethod,request.Amount
            );

        await electProjectErtFormRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        if (ertForm.Approved) throw new NotFoundException("قبلا تایید شده است");


        // دریافت فایل های مربوط به این پرونده
        var electProjectFiles = await electProjectFileRepository.GetByIdElectProject(electProject.Id);
        if (electProjectFiles is null) throw new NotFoundException("فایل های این پرونده وجود ندارد");
        var approvedErtFileExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.ApprovedErtExecutor);

        // دریافت کروکی الکترودها
        var crookyOfElectrodeFileExist = electProjectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.CrookyOfElectrode);
        if (crookyOfElectrodeFileExist is null)
            throw new NotFoundException("فایل کروکی چیدمان الکترودها را به صورت عکس بارگزاری کنید ");
        var crookyOfElectrodePath = $"Upload\\ElectProjects\\{electProject.Id}\\{crookyOfElectrodeFileExist.FileName}";
        var crookyOfElectrodePathS3 = await s3Service.GetFile(crookyOfElectrodePath.Replace(@"\", "/"));
        if (crookyOfElectrodePathS3 is null) throw new NotFoundException("خطای دریافت فایل کروکی چیدمان الکترودها");

        // دریافت امضای کارشناس
        const string signEngineerPath = $"Upload\\Nezam\\Draft.png";
        var signEngineerPathS3 = await s3Service.GetFile(signEngineerPath.Replace(@"\", "/"));
        if (signEngineerPathS3 is null) throw new NotFoundException("خطای دریافت مهر پیشنویس");



        // ایجاد پی دی اف شناسنامه ارت
        await reportService.GetApprovedErtForm(signEngineerPathS3, ertForm, epp.Engineer, epp.SolarDateDeliverEngineer, crookyOfElectrodePathS3,0);


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


        await  electProjectFileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return ertForm.Id.ToString();

    }
}