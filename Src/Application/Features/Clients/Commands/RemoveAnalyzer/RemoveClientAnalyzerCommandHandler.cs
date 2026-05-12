using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Commands.RemoveAnalyzer
{
    public class RemoveClientAnalyzerCommandHandler : IRequestHandler<RemoveClientAnalyzerCommand>
    {
        private readonly IClientRepository repository;

        public RemoveClientAnalyzerCommandHandler(IClientRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(RemoveClientAnalyzerCommand request, CancellationToken cancellationToken)
        {
            var clients = await repository.GetAll(new List<Guid> { request.ClientId });

            if(clients.Any())
            {
                clients.First().RemoveAnalyzer(request.UserId);

                await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
