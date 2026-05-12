using System.Collections.Generic;
using MediatR;

namespace Coreapi.Application.Features.Quotas.Queries.GetEngQuotaBurns;

public class GetEngQuotaBurnQuery:IRequest<IEnumerable<EngQuotaBurnDto>>
{
    public string EngId { get; set; }
    public string QtId { get; set; }
}