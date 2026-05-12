using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.DeleteProjectProcess
{
    public class DeleteProjectProcessCommand:IRequest<int>
    {
        public Guid EppId { get; set; }
    }
}
