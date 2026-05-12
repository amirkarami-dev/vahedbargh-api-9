using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.ChangeProjectProcess
{
	public class ChangeProjectProcessCommand:IRequest<Guid>
	{
		public Guid Id { get; set; }
		public Guid IdEngineer { get; set; }
	}
}
