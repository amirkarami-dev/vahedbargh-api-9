using MediatR;
using System.Collections.Generic;

namespace Coreapi.Application.Features.Transactions.Queries.GetEngPaymentTasks;

public class GetEngPaymentTasksQuery: IRequest<IEnumerable<EngPaymentTaskDto>>
{
    
}