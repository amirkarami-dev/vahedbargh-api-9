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
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.DeleteProjectProcess
{
    public class DeleteProjectProcessCommandHandler(
        IElectProjectProcessRepository electProjectProcessRepository,
        IElectProjectRepository electProjectRepository,
        IClientRepository clientRepository,
        ICurrentUser currentUser,
        IEngPaymentTaskRepository engPaymentTaskRepository)
        : IRequestHandler<DeleteProjectProcessCommand, int>
    {
        public async Task<int> Handle(DeleteProjectProcessCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var epp = await electProjectProcessRepository.GetElectProjectProcessById(request.EppId);
            if (epp is null) throw new NotFoundException("تخصیص وجود ندارد");


            //var engPaymentTasks = await engPaymentTaskRepository.GetByOrderPeriod();

            //var getLastPayment

            if (epp.Accepted)
                throw new NotFoundException($"پرونده توسط بازرس در تاریخ {epp.SolarDateAccepted} قبول شده و قابل حدف نیست");

            var electProject = await electProjectRepository.GetElectProjectById(epp.ElectProject.Id);
            if (electProject is null) throw new NotFoundException("پرونده وجود ندارد");

            if (epp.InspectionStatusEnum is not InspectionStatusEnum.Undefined)
                throw new NotFoundException("فقط تخصیصی که در انتظار تایید است قابل حذف می باشد");


           
            switch (epp.ProjectLevelEnum)
                {

                    case ProjectLevelEnum.ExpertStage:
                    {
                        if (electProject.ProjectLevelEnum is ProjectLevelEnum.ApproveStage) 
                            throw new NotFoundException("این تخصیص توسط مهندس تایید شده و قابل حذف نیست");
                        electProject.UpdateProjectLevel(ProjectLevelEnum.NullStage); 
                        
                        break;
                    }
                    case ProjectLevelEnum.ErtStage:
                    {
                        if (electProject.ProjectLevelEnum is ProjectLevelEnum.ApproveErtStage) 
                            throw new NotFoundException("این تخصیص توسط مجری ارت تایید شده و قابل حذف نیست");
                        if(electProject.IsBuildingInspection) electProject.UpdateProjectLevel(ProjectLevelEnum.ExpertStage);
                        if(electProject.IsEarthSystem && !electProject.IsBuildingInspection) electProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);
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
                            throw new NotFoundException("این تخصیص توسط مجری تست و تحویل تایید شده و قابل حذف نیست");
                        electProject.UpdateProjectLevel(ProjectLevelEnum.NullStage);
                        break;
                    }

                default:
                        throw new NotFoundException("مرحله پرونده مشکل دارد");
                }

            epp.UpdateCancelExpertStage(DateTime.Now, Helper.MiladiToShamsi(DateTime.Now), "حذف سیستمی", epp.ProjectLevelEnum);
                


            epp.SoftDelete();

            await electProjectProcessRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return 1;

        }
    }
}
