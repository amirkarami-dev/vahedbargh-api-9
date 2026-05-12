using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.ProjectProcess
{
    public class ProjectProcessCommand:IRequest<int>
    {
        public IEnumerable<Guid> IdElectProjects { get; set; } = new List<Guid>();
        public Guid IdEngineer { get; set; }
        public ProjectProcessTypeEnum ProjectProcessTypeEnum { get; set; }
        public long ErtAmount { get; set; }
    }
}
