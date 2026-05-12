using System;
using System.Collections.Generic;

namespace Coreapi.Domain.AggregatesModel.RoutAgg;

public class Route
{
    private Route(){}

    public Route(string routName, string routAddress, string routAddressEn, Route parent, Guid? parentId)
    {
        Id = Guid.NewGuid();
        RoutName = routName;
        RoutAddress = routAddress;
        RoutAddressEn = routAddressEn;
        Parent = parent;
        ParentId = parentId;
    }
    public Guid Id { get; init; }
    public string RoutName { get; private set; }
    public string RoutAddress { get; private set; }
    public string RoutAddressEn { get; private set; }
    public Route Parent { get; private set; }
    public Guid? ParentId { get; private set; }
    public ICollection<Route> SubRoutes { get; } = new List<Route>();
    
    
}