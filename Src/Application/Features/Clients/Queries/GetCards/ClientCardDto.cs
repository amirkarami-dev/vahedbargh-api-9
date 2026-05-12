using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetCards
{
    public class ClientCardDto : IMapFrom<ClientCard>
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public CardTypeEnum Type { get; set; }
        public string CCV { get; set; }
        public string ExpireDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ClientCard, ClientCardDto>();
        }
    }
}
