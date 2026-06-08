using Coreapi.Application.Common.Exceptions;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.UpdateIsMain
{
	public class UpdateIsMainCommandHandler(
		IElectProjectProcessRepository electProjectProcessRepository,
		IElectProjectRepository electProjectRepository
	) : IRequestHandler<UpdateIsMainCommand, Guid>
	{
		public async Task<Guid> Handle(UpdateIsMainCommand request, CancellationToken cancellationToken)
		{
			var epp = await electProjectProcessRepository.GetElectProjectProcessById(request.Id)
				?? throw new NotFoundException("تخصیص وجود ندارد");

			// IsMain may only be toggled when the related project is a child
			// (i.e. has a parent — belongs to a big project).
			var electProject = await electProjectRepository.GetElectProjectById(epp.ElectProject.Id)
				?? throw new NotFoundException("این پرونده وجود ندارد");

			if (electProject.ParentProject is null)
				throw new NotFoundException("این پرونده زیرمجموعه پرونده اصلی نیست");

			epp.UpdateIsMain(request.IsMain);

			await electProjectProcessRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return epp.Id;
		}
	}
}
