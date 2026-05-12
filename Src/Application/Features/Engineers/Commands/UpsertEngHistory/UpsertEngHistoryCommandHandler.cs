using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Engineers.Commands.UpsertEngHistory;

public class UpsertEngHistoryCommandHandler(
    IClientRepository clientRepository,
    ICurrentUser currentUser,
    IEngineerRepository engineerRepository,
    IEngineerHistoryRepository historyRepository)
    : IRequestHandler<UpsertEngHistoryCommand, int>
{
    public async Task<int> Handle(UpsertEngHistoryCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        if (request.Id != null)
        {
            var engHistory = await historyRepository.GetById(Guid.Parse(request.Id));
            if (engHistory is null)
            {
                throw new NotFoundException("این تاریخچه صلاحیت وجود ندارد");
            }

            engHistory.Update(request.EngineerGradeTypeEnum,request.WorkPermitNum,Helper.ShamsiToMiladi(request.SolarIssueDate),request.SolarIssueDate,Helper.ShamsiToMiladi(request.SolarValidityDate),request.SolarValidityDate,request.WorkPermission,
                request.WorkPermitTypeEnum);
        }
        else
        {
            var engineer = await engineerRepository.GetById(Guid.Parse(request.EngId));
            if (engineer is null) throw new NotFoundException("کارشناس وجود ندارد");
            var engHistory = new EngineerHistory(engineer, request.EngineerGradeTypeEnum, request.WorkPermitNum,
                Helper.ShamsiToMiladi(request.SolarIssueDate), request.SolarIssueDate,
                Helper.ShamsiToMiladi(request.SolarValidityDate), request.SolarValidityDate,
                request.WorkPermission,request.WorkPermitTypeEnum);

            historyRepository.Add(engHistory);
        }

        await historyRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return 0;
    }
}