using System.Collections.Generic;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetAllProject;

public class GetAllProjectQuery:IRequest<IEnumerable<ElectProject>>
{
    
}