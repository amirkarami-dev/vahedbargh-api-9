using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetAreas;

public class ClientAreaDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Radius { get; set; }
    public SegmentTypeEnum Type { get; set; }

    public IEnumerable<ClientAreaPointDto> Points { get; set; }
    public IEnumerable<ClientAreaUserDto> Users { get; set; }
}

public class ClientAreaPointDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set;}
}

public class ClientAreaUserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }
}
