using System.Collections.Generic;
using Coreapi.Common.Enums;
using MediatR;

namespace Coreapi.Application.Features.Supports.Queries.GetListSupport;

public class GetListSupportQuery:IRequest<IEnumerable<ListSupportDto>>
{
    public bool Closed { get; set; }
    public int TicketNumber { get; set; } = 0;
    public string Sender { get; set; }
    public string Role { get; set; }
    public UserType UserType { get; set; } = UserType.None;
    public SupportListTypeEnum SupportListTypeEnum { get; set; }
    public string SearchUserId { get; set; }

}