using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.UpdateEngPayment;

public class UpdateEngPaymentCommandHandler:IRequestHandler<UpdateEngPaymentCommand,EngPaymentList>
{
    private readonly IClientRepository clientRepository;
    private readonly ICurrentUser currentUser;
    private readonly IEngPaymentListRepository engPaymentListRepository;

    public UpdateEngPaymentCommandHandler(IClientRepository clientRepository, ICurrentUser currentUser, IEngPaymentListRepository engPaymentListRepository)
    {
        this.clientRepository = clientRepository;
        this.currentUser = currentUser;
        this.engPaymentListRepository = engPaymentListRepository;
    }
    public async Task<EngPaymentList> Handle(UpdateEngPaymentCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        var engPaymentSingle = await engPaymentListRepository.GetEngPaymentListById(Guid.Parse(request.Id));
        if (engPaymentSingle is null) throw new NotFoundException("این سطر یافت نیشد");
        if (engPaymentSingle.EngPaymentTask.Approved) throw new NotFoundException("پرداختی انجام شده و قابل ویرایش نیست");
        engPaymentSingle.UpdateAmount(
            request.AmountSystem,
            request.Deduction2
            );

        await engPaymentListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return engPaymentSingle;
    }
}