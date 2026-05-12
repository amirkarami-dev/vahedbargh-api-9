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
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using MediatR;

namespace Coreapi.Application.Features.QuarterTariffs.Queries.GetListQuarterTariff
{
    public class GetListQuarterTariffQueryHandler:IRequestHandler<GetListQuarterTariffQuery,IEnumerable<ListQuarterTariffDto>>
    {
        private readonly IQuarterTariffRepository quarterTariffRepository;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;
        private readonly IMapper mapper;


        public GetListQuarterTariffQueryHandler(IQuarterTariffRepository quarterTariffRepository, ICurrentUser currentUser, IClientRepository clientRepository, IMapper mapper)
        {
            this.quarterTariffRepository = quarterTariffRepository;
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ListQuarterTariffDto>> Handle(GetListQuarterTariffQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var quarterTariffs = await quarterTariffRepository.GetAll();

            return mapper.Map<IEnumerable<ListQuarterTariffDto>>(quarterTariffs.OrderByDescending(o=>o.Period));
        }
    }
}
