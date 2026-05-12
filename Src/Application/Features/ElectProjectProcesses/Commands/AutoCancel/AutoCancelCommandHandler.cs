using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.AutoCancel;

public class AutoCancelCommandHandler:IRequestHandler<AutoCancelCommand,int>
{
    private readonly IElectProjectProcessRepository electProjectProcessRepository;

    public AutoCancelCommandHandler(IElectProjectProcessRepository electProjectProcessRepository)
    {
        this.electProjectProcessRepository = electProjectProcessRepository;
    }
    public async Task<int> Handle(AutoCancelCommand request, CancellationToken cancellationToken)
    {
        var processes = await electProjectProcessRepository.GetEppNotAccepted(14);
        if (processes == null || !processes.Any()) return 0;
        foreach (var epp in processes)
        {
            switch (epp.ProjectLevelEnum)
            {
                case ProjectLevelEnum.ExpertStage:
                {
                    epp.ElectProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);
                    break;
                }
                // اگر این پرونده در مرحله ارت تایید شده باشد یعنی بازرسی آن 7 روز گذشته
                case ProjectLevelEnum.ApproveErtStage:
                {
                    if (epp.ElectProject.IsBuildingInspection)
                    {
                        epp.ElectProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);
                    } 
                    break;
                }
                case ProjectLevelEnum.ErtStage:
                {
                    // اگر پرونده بازرسی داشته باشد و قبلا در مرحله دو خط بالاتر نشده باشه به نال استیج بیا و ببرش به مرحله بازرسی برای تخصیص ارت
                    if (epp.ElectProject.IsBuildingInspection &&
                        epp.ElectProject.ProjectLevelEnum is not ProjectLevelEnum.NullStage)
                    {
                        epp.ElectProject.UpdateProjectLevel(ProjectLevelEnum.ExpertStage);
                    }
                    if (epp.ElectProject.IsEarthSystem &&
                        !epp.ElectProject.IsBuildingInspection) 
                        epp.ElectProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);
                    break;
                }
                case ProjectLevelEnum.TestDeliveryStage:
                {
                    epp.ElectProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);
                    break;
                }
            }
            epp.UpdateCancelExpertStage(DateTime.Now, Helper.MiladiToShamsi(DateTime.Now), "کنسل سیستمی", epp.ProjectLevelEnum);
        }
        await electProjectProcessRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return 0;
    }
}