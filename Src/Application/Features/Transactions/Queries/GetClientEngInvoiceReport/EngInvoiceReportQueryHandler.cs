using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Features.Transactions.Queries.GetEngClientInvoices;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetClientEngInvoiceReport
{
    public class EngInvoiceReportQueryHandler:IRequestHandler<EngInvoiceReportQuery,IEnumerable<EngInvoiceReportDto>>
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;
        private readonly IEngineerRepository engineerRepository;

        public EngInvoiceReportQueryHandler(ITransactionRepository transactionRepository, IMapper mapper, ICurrentUser currentUser, IClientRepository clientRepository, IEngineerRepository engineerRepository)
        {
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
            this.engineerRepository = engineerRepository;
        }
        public async Task<IEnumerable<EngInvoiceReportDto>> Handle(EngInvoiceReportQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);


            var transactions = await transactionRepository.GetClientEngInvoices(client.Id, request.StartDate,request.EndDate);
            return mapper.Map<IEnumerable<EngInvoiceReportDto>>(transactions);
        }
    }
}
