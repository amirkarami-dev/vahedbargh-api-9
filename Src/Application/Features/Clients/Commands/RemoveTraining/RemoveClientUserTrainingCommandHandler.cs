using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Commands.RemoveTraining
{
    public class RemoveClientUserTrainingCommandHandler : IRequestHandler<RemoveClientUserTrainingCommand>
    {
        private readonly IClientRepository repository;

        public RemoveClientUserTrainingCommandHandler(IClientRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(RemoveClientUserTrainingCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithTrainings(request.ClientId);
            if (client is null)
                throw new NotFoundException(nameof(Client), request.ClientId);

            var training = client.UserTrainings.FirstOrDefault(c => c.Id == request.TrainingId);
            if (training is null)
                throw new NotFoundException(nameof(ClientUserTraining), request.TrainingId);

            client.RemoveTraining(training);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
