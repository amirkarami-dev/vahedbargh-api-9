using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Commands.UpsertTraining
{
    public class UpsertClientUserTrainingCommandHandler : IRequestHandler<UpsertClientUserTrainingCommand>
    {
        private readonly IClientRepository repository;

        public UpsertClientUserTrainingCommandHandler(IClientRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(UpsertClientUserTrainingCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithTrainings(request.ClientId);
            if (client is null)
                throw new NotFoundException(nameof(Client), request.ClientId);

            ClientUserTraining training = null;
            if (request.TrainingId.HasValue)
                training = client.UserTrainings.FirstOrDefault(c => c.Id == request.TrainingId.Value);

            if (training is null)
                client.AddTraining(request.UserId, request.Title, request.Link);
            else
                training.Update(request.Title, request.Link);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
