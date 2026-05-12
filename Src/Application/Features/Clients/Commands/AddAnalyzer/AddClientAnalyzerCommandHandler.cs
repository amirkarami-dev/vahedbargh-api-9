using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Commands.AddAnalyzer
{
    public class AddClientAnalyzerCommandHandler : IRequestHandler<AddClientAnalyzerCommand>
    {
        private readonly IClientRepository repository;

        public AddClientAnalyzerCommandHandler(IClientRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(AddClientAnalyzerCommand request, CancellationToken cancellationToken)
        {
            var analyzerClients = await repository.GetAnalyzerClients(request.UserId);

            analyzerClients.Where(c => !request.Clients.Any(id => c.Id == id)).ToList()
                .ForEach(c => c.RemoveAnalyzer(request.UserId));

            var addedClients = request.Clients.Where(id => !analyzerClients.Any(c => c.Id == id));

            var clients = await repository.GetAll(addedClients);

            clients.ToList().ForEach(c => c.AddAnalyzer(request.UserId));
            
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
