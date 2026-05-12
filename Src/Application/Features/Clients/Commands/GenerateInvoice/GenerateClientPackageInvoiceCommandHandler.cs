using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;


namespace Coreapi.Application.Features.Clients.Commands.GenerateInvoice
{
    public class GenerateClientPackageInvoiceCommandHandler : IRequestHandler<GenerateClientPackageInvoiceCommand, InvoiceDto>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;
       

        public GenerateClientPackageInvoiceCommandHandler(IClientRepository repository, ICurrentUser currentUser
           )
        {
            this.repository = repository;
            this.currentUser = currentUser;
       
        }

        public async Task<InvoiceDto> Handle(GenerateClientPackageInvoiceCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithPackages(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var card = (await repository.GetWithCards(Guid.Parse(currentUser.ClientId)))?.Cards.FirstOrDefault(c=>c.Id == request.CardId);
            if (card is null)
                throw new NotFoundException(nameof(ClientCard), request.CardId);



            var output = new InvoiceDto()
            {
                Amount = 0,
                InvoiceId = Guid.Empty
            };

            return output;
        }
    }
}
