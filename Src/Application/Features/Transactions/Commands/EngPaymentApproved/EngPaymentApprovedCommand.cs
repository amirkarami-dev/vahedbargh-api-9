using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.EngPaymentApproved;

public class EngPaymentApprovedCommand:IRequest<string>
{
    public string EngPaymentTaskId { get; set; }
    public string SolarApproved { get; set; }
    
}