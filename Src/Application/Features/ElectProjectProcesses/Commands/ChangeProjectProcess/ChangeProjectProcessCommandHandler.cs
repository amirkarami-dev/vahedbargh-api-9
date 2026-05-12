using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.ChangeProjectProcess
{
	public class ChangeProjectProcessCommandHandler(
	IElectProjectProcessRepository electProjectProcessRepository,
	IClientRepository clientRepository,
	IEngineerRepository engineerRepository,
	ICurrentUser currentUser,
	IEngPaymentTaskRepository engPaymentTaskRepository
	) : IRequestHandler<ChangeProjectProcessCommand, Guid>
	{
		public async Task<Guid> Handle(ChangeProjectProcessCommand request, CancellationToken cancellationToken)
		{
			var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
			if (client is null)
			{
				throw new NotFoundException(nameof(Client), currentUser.ClientId);
			}

			var Epp = await electProjectProcessRepository.GetElectProjectProcessById(request.Id) ?? 
													throw new NotFoundException("تخصیص وجود ندارد");

			if(Epp.InspectionStatusEnum is not InspectionStatusEnum.Undefined) throw new NotFoundException("وضعیت باید نامشخص باشد");

			var paymentTask = await engPaymentTaskRepository.GetLastByOrderPeriod();

			// برای ارت مهم نیست هر موقع بخواهد میتواند تغییرش بده
			if (Epp.JulianDateAccepted is not null && paymentTask.ToJulian.Date >= Epp.JulianDateAccepted?.Date && Epp.ProjectLevelEnum is not ProjectLevelEnum.ErtStage)
				throw new NotFoundException("لیست پرداخت بعد از قبول پرونده وجود دارد");


			var engineer = await engineerRepository.GetById(request.IdEngineer) ?? 
													throw new NotFoundException("مهندس وجود ندارد");
			Epp.ChangeEngineer(engineer);
			
			await electProjectProcessRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return request.Id;

		}
	}
}
