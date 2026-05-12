using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Mappings;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetRemainingBalance;

public class RemainingBalanceDto : IMapFrom<Client>
{
    public decimal Balance { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Client, RemainingBalanceDto>();
    }
}
