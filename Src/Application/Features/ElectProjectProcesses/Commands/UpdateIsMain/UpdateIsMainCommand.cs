using MediatR;
using System;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.UpdateIsMain
{
	public class UpdateIsMainCommand : IRequest<Guid>
	{
		public Guid Id { get; set; }
		public bool IsMain { get; set; }
	}
}
