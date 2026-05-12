using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetUserAreas;

public class UserAreaDto : IMapFrom<ClientArea>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Radius { get; set; }
    public SegmentTypeEnum Type { get; set; }

    public IEnumerable<ClientAreaPointDto> Points { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ClientArea, UserAreaDto>();
    }
}

public class ClientAreaPointDto : IMapFrom<ClientAreaPoint>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ClientAreaPoint, ClientAreaPointDto>()
            .ForMember(c => c.Latitude, opt => opt.MapFrom(c => c.Location.Latitude))
            .ForMember(c => c.Longitude, opt => opt.MapFrom(c => c.Location.Longitude));
    }
}

