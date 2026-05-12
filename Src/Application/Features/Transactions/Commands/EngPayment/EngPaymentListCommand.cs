using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.EngPayment;

public class EngPaymentListCommand:IRequest<string>
{
    public string FromSolar { get; set; }
    public string ToSolar { get; set; }
    public string Description { get; set; }

}