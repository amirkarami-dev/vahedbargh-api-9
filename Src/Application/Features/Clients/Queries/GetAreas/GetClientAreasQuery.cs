using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Queries.GetAreas;

public class GetClientAreasQuery: IRequest<IEnumerable<ClientAreaDto>>
{
}
