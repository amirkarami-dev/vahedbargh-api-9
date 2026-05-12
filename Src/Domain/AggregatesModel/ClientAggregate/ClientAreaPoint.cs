using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.ValueObjects;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate;

public class ClientAreaPoint 
{
    private ClientAreaPoint()
    {
    }

    public ClientAreaPoint(Point location, int order)
    {
        Location = location;
        Order = order;
    }

    public long Id { get; }
    public int Order { get; init; }
    public Point Location { get; init; }
}
