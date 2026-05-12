using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpdateByEdc;

public class UpdateByEdcCommandHandler(
    IElectProjectRepository electProjectRepository,
    ICurrentUser currentUser,
    IElectProjectProcessRepository electProjectProcessRepository,
    ISmsService smsService
    )
    : IRequestHandler<UpdateByEdcCommand, int>
{
    // این هندلر برای تغییر وضعیت پرونده می باشد
    public async Task<int> Handle(UpdateByEdcCommand request, CancellationToken cancellationToken)
    {
        var electProject = await electProjectRepository.GetElectProjectById(Guid.Parse(request.ElectProjectId));
        if (electProject == null) throw new NotFoundException("این پرونده وجود ندارد");
        var eppApproved = await electProjectProcessRepository.GetEppByApproved(electProject.Id);


        if (currentUser.Role.Contains("Administrator") || currentUser.Role.Contains("Section"))
        {
            switch (request.ElectProjectStatusEnum)
            {
                // در فرانت دستی روی 13 ست شده
                case ElectProjectStatusEnum.DefectOnNezam:
                    electProject.UpdateProjectLevel(ProjectLevelEnum.Defect);
                    electProject.UpdateIsOk(false);
                    // فقط در تخصیص های تایید شده سرچ میکند

                    foreach (var  projectProcess  in eppApproved)
                    {
                        if ((projectProcess.ProjectLevelEnum == ProjectLevelEnum.ErtStage && request.ErtDefect) ||
                            (projectProcess.ProjectLevelEnum == ProjectLevelEnum.ExpertStage && request.InspectDefect))
                        {
                            projectProcess.InspectionStatusEnum = InspectionStatusEnum.Undefined;
                            await smsService.SendSms3Params(projectProcess.Engineer.CellPhone, 14016,
                                projectProcess.ElectProject.FileNumber.ToString()
                                , "نقص در پرونده", Helper.MiladiToShamsiForSms(DateTime.Now.Date));
                        }

                        // در اینجا اگر پرونده ارت و بازرسی داشه باشد و نقص ارت باشد بازرسی هم نقص میخورد برای
                        // تایید مجدد بازرسی
                        if (projectProcess.ElectProject.IsBuildingInspection &&
                            projectProcess.ElectProject.IsEarthSystem && 
                            projectProcess.ProjectLevelEnum == ProjectLevelEnum.ErtStage)
                        {
                            if (request.ErtDefect)
                            {
                                projectProcess.InspectionStatusEnum = InspectionStatusEnum.Undefined;
                                await smsService.SendSms3Params(projectProcess.Engineer.CellPhone, 14016,
                                    projectProcess.ElectProject.FileNumber.ToString()
                                    , "نقص در پرونده", Helper.MiladiToShamsiForSms(DateTime.Now.Date));
                            }
                        }

                    }


                    electProject.UpdateDefectByAdmin(request.Des,false);

                    break;
                case ElectProjectStatusEnum.SubmitOnNezam:
                    //electProject.UpdateProjectLevel(ProjectLevelEnum.ApproveStage);
                    //electProject.UpdateIsOk(true);
                    electProject.UpdateDefectByAdmin(request.Des,true);

                    break;

            }

        }
        else if (currentUser.Role.Contains("Engineer"))
        {
            electProject.UpdateSolvedDefectEng(request.Des);

            //electProject.UpdateProjectLevel(ProjectLevelEnum.ApproveStage);

        }
        else if (currentUser.Role.Contains("ElectAdmin"))
        {
            electProject.UpdateStatus(request.ElectProjectStatusEnum);
        

            electProject.UpdateDefectDes(request.Des);
        }
        else
        {
            throw new NotFoundException("خطای مجوز دسترسی");
        }



        await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return 0;
    }
}