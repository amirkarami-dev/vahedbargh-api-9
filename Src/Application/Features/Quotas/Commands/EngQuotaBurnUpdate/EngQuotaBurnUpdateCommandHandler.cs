using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using MediatR;

namespace Coreapi.Application.Features.Quotas.Commands.EngQuotaBurnUpdate;

public class EngQuotaBurnUpdateCommandHandler(
    IEngQuotaBurnRepository engQuotaBurnRepository,
    ICurrentUser currentUser,
    IClientRepository clientRepository,
    IEngineerRepository engineerRepository,
    IQuarterTariffRepository quarterTariffRepository)
    : IRequestHandler<EngQuotaBurnUpdateCommand, string>
{
    public async Task<string> Handle(EngQuotaBurnUpdateCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        if (Guid.TryParse(request.Id,out var engQuotaBurnId))
        {
            var engQuotaBurn = await engQuotaBurnRepository.GetById(engQuotaBurnId);
            engQuotaBurn.Update(
                engQuotaBurn.AmountRemaining, 
                request.AmountBurning,
                engQuotaBurn.ErtCountRemaining,
                request.ErtCountBurning,
                request.InspectionDelayFactor,
                request.ErtDelayFactor,
                engQuotaBurn.Approved, 
                request.Des
                );
            await engQuotaBurnRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return engQuotaBurn.Id.ToString();
        }

        var quarter = await quarterTariffRepository.GetById(Guid.Parse(request.QtId));
        if (quarter is null) throw new NotFoundException("دوره تخصیص یافت نشد");
        if (request.EngId is null) throw new NotFoundException("کارشناس را انتخاب کنید");
        var eng = await engineerRepository.GetById(Guid.Parse(request.EngId));
        if (eng == null) throw new NotFoundException("کارشناس یافت نشد");

        var newEngQuotaBurn = new EngQuotaBurn(
            client, 
            quarter, 
            eng, 
            0,
            request.AmountBurning,
            0,
            request.ErtCountBurning,
            request.InspectionDelayFactor,
            request.ErtDelayFactor,
            true, 
            request.Des
            );
        engQuotaBurnRepository.Add(newEngQuotaBurn);
        await engQuotaBurnRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        engQuotaBurnId = newEngQuotaBurn.Id;

        return engQuotaBurnId.ToString();

    }
}