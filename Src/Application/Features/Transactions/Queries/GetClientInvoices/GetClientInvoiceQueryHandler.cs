using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetClientInvoices
{
    public class GetClientInvoiceQueryHandler:IRequestHandler<GetClientInvoiceQuery, IEnumerable<ClientInvoiceDto>>
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;

        public GetClientInvoiceQueryHandler(ITransactionRepository transactionRepository, IMapper mapper, ICurrentUser currentUser, IClientRepository clientRepository)
        {
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
        }
        public async Task<IEnumerable<ClientInvoiceDto>> Handle(GetClientInvoiceQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            var transactions = await transactionRepository.GetClientInvoices(client.Id, 0, 1000000);

            return mapper.Map<IEnumerable<ClientInvoiceDto>>(transactions);
        }
    }
}
