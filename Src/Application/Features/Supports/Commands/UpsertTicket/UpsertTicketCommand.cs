using MediatR;
using System;

namespace Coreapi.Application.Features.Supports.Commands.UpsertTicket;

public class UpsertTicketCommand:IRequest<string>
{
    public string SupportId { get; init; }
    public string Message { get; set; }
    public string FileName { get; set; }

}