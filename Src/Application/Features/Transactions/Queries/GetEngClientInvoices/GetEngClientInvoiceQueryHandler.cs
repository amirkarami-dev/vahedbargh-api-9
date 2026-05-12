using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Features.Transactions.Queries.GetClientInvoices;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetEngClientInvoices
{
    public class GetEngClientInvoiceQueryHandler:IRequestHandler<GetEngClientInvoiceQuery, IEnumerable<EngClientInvoiceDto>>
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;
        private readonly IEngineerRepository engineerRepository;

        public GetEngClientInvoiceQueryHandler(ITransactionRepository transactionRepository, IMapper mapper, ICurrentUser currentUser, IClientRepository clientRepository, IEngineerRepository engineerRepository)
        {
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
            this.engineerRepository = engineerRepository;
        }

        public async Task<IEnumerable<EngClientInvoiceDto>> Handle(GetEngClientInvoiceQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var engineer = await engineerRepository.getByUserId(currentUser.UserId);
            if(engineer is null) throw new NotFoundException("شما کارشناس نیستید");
            if(engineer.UserId != request.EngId.ToString()) throw new NotFoundException("کارشنسی با این آیدی یافت نشد");


            var transactions = await transactionRepository.GetClientInvoices(client.Id, engineer.Id, 0, 1000000);

            return mapper.Map<IEnumerable<EngClientInvoiceDto>>(transactions);
        }
    }
}
