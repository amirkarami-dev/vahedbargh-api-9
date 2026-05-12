using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Features.Clients.Commands.UpsertArea;

public class UpsertClientAreaCommand : IRequest
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Radius { get; set; } = 0;
    public SegmentTypeEnum Type { get; set; } = SegmentTypeEnum.Polygon;

    public IEnumerable<InputPoint> Area { get; set; }
}

public class InputPoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
