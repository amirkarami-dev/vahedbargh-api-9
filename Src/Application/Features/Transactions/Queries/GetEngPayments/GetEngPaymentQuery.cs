using System.Collections.Generic;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetEngPayments;

public class GetEngPaymentQuery:IRequest<IEnumerable<EngPaymentDto>>
{
    public string EngPaymentTaskId { get; set; }
    
}