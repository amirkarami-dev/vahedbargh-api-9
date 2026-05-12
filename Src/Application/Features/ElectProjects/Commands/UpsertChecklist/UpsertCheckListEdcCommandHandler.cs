using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpsertChecklist;

public class UpsertCheckListEdcCommandHandler:IRequestHandler<UpsertCheckListEdcCommand,string>
{

    private readonly IClientRepository clientRepository;
    private readonly IElectProjectRepository electProjectRepository;
    private readonly ICheckListEdcRepository checkListEdcRepository;
    private readonly ICurrentUser currentUser;
    private readonly IElectProjectProcessRepository processRepository;


    public UpsertCheckListEdcCommandHandler(IClientRepository clientRepository, IElectProjectRepository electProjectRepository, ICheckListEdcRepository checkListEdcRepository, ICurrentUser currentUser, IElectProjectProcessRepository processRepository)
    {
        this.clientRepository = clientRepository;
        this.electProjectRepository = electProjectRepository;
        this.checkListEdcRepository = checkListEdcRepository;
        this.currentUser = currentUser;
        this.processRepository = processRepository;
    }
    public async Task<string> Handle(UpsertCheckListEdcCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }


        if (string.IsNullOrEmpty(request.ElectProjectId)) throw new NotFoundException("این پرونده وجود ندارد");
        var electProject = await electProjectRepository.GetById(Guid.Parse(request.ElectProjectId));
        if (electProject is null) throw new NotFoundException("این پرونده وجود ندارد");


        // فعلا برداشتم تا تعیین تکلیف
        //if (electProject.ProjectLevelEnum == ProjectLevelEnum.ApproveStage)
        //    throw new NotFoundException("این پرونده قبلا تایید شده است");



        var checkListId = request.Id;

        if (!string.IsNullOrEmpty(request.DeleteId) && string.IsNullOrEmpty(checkListId))
        {
            var resultDelete = await checkListEdcRepository.DeleteById(Guid.Parse(request.DeleteId));
            if (resultDelete is not null)
                checkListId = resultDelete.Id.ToString();
            else
                throw new NotFoundException("قابل حذف نیست");
        }
        else
        {
            if (string.IsNullOrEmpty(request.SolarChecked)) throw new NotFoundException("تاریخ وارد نشده است");

            if (string.IsNullOrEmpty(checkListId))
            {
                var checkList = new CheckListEdc(request.SolarChecked, Helper.ShamsiToMiladi(request.SolarChecked), request.CheckListEdcEnum, request.IsComplete, request.ResultDes, electProject);
                checkListEdcRepository.Add(checkList);
                checkListId = checkList.Id.ToString();
            }
            else
            {
                var checkList = await checkListEdcRepository.GetById(Guid.Parse(checkListId));
                if (checkList is null) throw new NotFoundException("چک یست وجود ندارد");
                checkList.UpdateComment(request.SolarChecked, Helper.ShamsiToMiladi(request.SolarChecked),
                    request.CheckListEdcEnum, request.IsComplete, request.ResultDes);
            }

        }


        await checkListEdcRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return checkListId;


    }
}