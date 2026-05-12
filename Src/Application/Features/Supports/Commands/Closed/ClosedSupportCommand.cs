using MediatR;

namespace Coreapi.Application.Features.Supports.Commands.Closed;

public class ClosedSupportCommand:IRequest<string>
{
    public string SupportId { get; set; }
    
}