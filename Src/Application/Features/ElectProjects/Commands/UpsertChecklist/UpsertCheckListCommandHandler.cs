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

public class UpsertCheckListCommandHandler:IRequestHandler<UpsertCheckListCommand,string>
{
    private readonly IClientRepository clientRepository;
    private readonly IElectProjectRepository electProjectRepository;
    private readonly ICheckListFormRepository checkListFormRepository;
    private readonly IEngineerRepository engineerRepository;
    private readonly ICurrentUser currentUser;
    private readonly IElectProjectProcessRepository processRepository;


    public UpsertCheckListCommandHandler(IClientRepository clientRepository, IElectProjectRepository electProjectRepository, ICheckListFormRepository checkListFormRepository, IEngineerRepository engineerRepository, ICurrentUser currentUser, IElectProjectProcessRepository processRepository)
    {
        this.clientRepository = clientRepository;
        this.electProjectRepository = electProjectRepository;
        this.checkListFormRepository = checkListFormRepository;
        this.engineerRepository = engineerRepository;
        this.currentUser = currentUser;
        this.processRepository = processRepository;
    }
    public async Task<string> Handle(UpsertCheckListCommand request, CancellationToken cancellationToken)
    {


        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        var eng = await engineerRepository.getByUserId(currentUser.UserId);
        if (eng is null) throw new NotFoundException("این کاربری کارشناس نیست");

        var epp = await processRepository.GetElectProjectProcessById(Guid.Parse(request.EppId));
        if (epp is null) throw new NotFoundException("این تخصیص وجود ندارد");

        if (!epp.Accepted) throw new NotFoundException("ابتدا باید پرونده را قبول کنید");


        if (string.IsNullOrEmpty(request.ElectProjectId)) throw new NotFoundException("این پرونده وجود ندارد");
        var electProject = await electProjectRepository.GetById(Guid.Parse(request.ElectProjectId));
        if (electProject is null) throw new NotFoundException("این پرونده وجود ندارد");

        //if (electProject.ProjectLevelEnum == ProjectLevelEnum.ApproveStage)
        //    throw new NotFoundException("این پرونده قبلا تایید شده است");


        var checkListId = request.Id;

        if (!string.IsNullOrEmpty(request.DeleteId) && string.IsNullOrEmpty(checkListId))
        {
           var resultDelete = await checkListFormRepository.DeleteById(Guid.Parse(request.DeleteId));
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
                var checkList = new CheckListForm(request.SolarChecked, Helper.ShamsiToMiladi(request.SolarChecked), request.InspectionDesEnum, request.IsComplete, request.ResultDes, electProject, eng);
                checkListFormRepository.Add(checkList);
                checkListId = checkList.Id.ToString();
            }
            else
            {
                var checkList = await checkListFormRepository.GetById(Guid.Parse(checkListId));
                if (checkList is null) throw new NotFoundException("چک یست وجود ندارد");
                checkList.UpdateComment(request.SolarChecked, Helper.ShamsiToMiladi(request.SolarChecked),
                    request.InspectionDesEnum, request.IsComplete, request.ResultDes);
            }

        }


        await checkListFormRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return checkListId;

    }
}