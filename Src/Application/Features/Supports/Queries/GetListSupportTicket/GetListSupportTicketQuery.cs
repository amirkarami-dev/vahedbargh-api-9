using System.Collections.Generic;
using MediatR;

namespace Coreapi.Application.Features.Supports.Queries.GetListSupportTicket;

public class GetListSupportTicketQuery:IRequest<IEnumerable<ListSupportTicketDto>>
{
    public string SupportId { get; set; }
}