using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using MediatR;

namespace Coreapi.Application.Features.Supports.Queries.GetListSupportTicket;

public class GetListSupportTicketQueryHandler(
    IMapper mapper,
    IClientRepository clientRepository,
    ICurrentUser currentUser,
    ISupportMessageRepository supportMessageRepository,
    ISupportRepository supportRepository)
    : IRequestHandler<GetListSupportTicketQuery, IEnumerable<ListSupportTicketDto>>
{
    public async Task<IEnumerable<ListSupportTicketDto>> Handle(GetListSupportTicketQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetWithSetting(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        var support = await supportRepository.GetById(Guid.Parse(request.SupportId));
        if (support is null) throw new NotFoundException("این تیکت وجود ندارد");

        var tickets = await supportMessageRepository.getBySupportId(request.SupportId);
        if (tickets is null) throw new NotFoundException("لیست تیکت ها وجود ندارد");

        var isAdmin = currentUser.Role.Contains("Administrator") || currentUser.Role.Contains("Employee") || currentUser.Role.Contains("Accountant");

        // Check if current user is the receiver
        var isReceiver = support.ToUserId == currentUser.UserId;

        if (isAdmin)
        {
            support.UpdateRead(true);
            foreach (var supportMessage in tickets)
            {
                supportMessage.UpdateReceive(true);
            }
        }

        // Mark messages as read by receiver if current user is the receiver
        if (isReceiver)
        {
            await supportMessageRepository.MarkMessagesAsReadByReceiver(request.SupportId, currentUser.UserId);
        }

        await supportMessageRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<IEnumerable<ListSupportTicketDto>>(tickets);
    }
}