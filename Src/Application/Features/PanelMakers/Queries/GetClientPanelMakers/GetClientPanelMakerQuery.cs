using System.Collections.Generic;
using MediatR;

namespace Coreapi.Application.Features.PanelMakers.Queries.GetClientPanelMakers
{
    public class GetClientPanelMakerQuery : IRequest<IEnumerable<PanelMakerDto>>
    {

    }
}

