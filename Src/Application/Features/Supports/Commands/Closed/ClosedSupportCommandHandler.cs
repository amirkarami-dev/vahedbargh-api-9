using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using MediatR;

namespace Coreapi.Application.Features.Supports.Commands.Closed;

public class ClosedSupportCommandHandler:IRequestHandler<ClosedSupportCommand,string>
{
    private readonly IClientRepository clientRepository;
    private readonly ICurrentUser currentUser;
    private readonly ISupportRepository supportRepository;

    public ClosedSupportCommandHandler(IClientRepository clientRepository, ICurrentUser currentUser, ISupportRepository supportRepository)
    {
        this.clientRepository = clientRepository;
        this.currentUser = currentUser;
        this.supportRepository = supportRepository;
    }
    public async Task<string> Handle(ClosedSupportCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        var support = await supportRepository.GetById(Guid.Parse(request.SupportId));
        if (support is null) throw new NotFoundException("درخواست یافت نشد");

        support.ClosedSupport(!support.Closed);

        await supportRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return support.Id.ToString();

    }
}