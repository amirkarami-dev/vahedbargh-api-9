using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetProjectFiles
{
    public class GetElectProjectFileQuery : IRequest<IEnumerable<ElectProjectFileDto>>
    {
        public string ElectProjectId { get; set; }
    }
}
