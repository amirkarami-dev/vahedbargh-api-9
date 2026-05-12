using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.Handlers;
using Point = Coreapi.Domain.ValueObjects.Point;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate;

public class ClientArea
{
    private ClientArea()
    {
    }

    public ClientArea(string name, string description, IEnumerable<ValueObjects.Point> area)
    {
        var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

        var polygon = gf.CreatePolygon(area.Select(p => new NetTopologySuite.Geometries.Point(p.Longitude, p.Latitude)).Select(p => new Coordinate(p.X, p.Y)).ToArray());

        if (polygon != null && !polygon.Shell.IsCCW) polygon = (Polygon)polygon.Reverse();

        Id = Guid.NewGuid();
        Type = SegmentTypeEnum.Polygon;
        Name = name;
        Description = description;
        Area = polygon;
        Radius = 0;

        Points = area.Select((a, i) => new ClientAreaPoint(a, i)).ToList();
        Users = new Collection<ClientUserArea>();
    }

    public ClientArea(string name, string description, double radius, ValueObjects.Point point)
    {
        var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

        var circle = gf.CreatePoint(new NetTopologySuite.Geometries.Point(point.Longitude, point.Latitude).Coordinate).Buffer(radius);

        Id = Guid.NewGuid();
        Type = SegmentTypeEnum.Circle;
        Name = name;
        Description= description;
        Area = circle;
        Radius = radius;

        Points = new Collection<ClientAreaPoint> { new ClientAreaPoint(point, 0) };
        Users = new Collection<ClientUserArea>();
    }

    public Guid Id { get; init; }
    public SegmentTypeEnum Type { get; init; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public double Radius { get; private set; }
    public Geometry Area { get; private set; }
    public ICollection<ClientAreaPoint> Points { get; private set; }

    public void Update(string name, string description, IEnumerable<ValueObjects.Point> area)
    {
        var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

        var polygon = gf.CreatePolygon(area.Select(p => new NetTopologySuite.Geometries.Point(p.Longitude, p.Latitude)).Select(p => new Coordinate(p.X, p.Y)).ToArray());

        if (polygon != null && !polygon.Shell.IsCCW) polygon = (Polygon)polygon.Reverse();

        Name = name;
        Description = description;
        Area = polygon;
        Radius = 0;

        Points = area.Select((a, i) => new ClientAreaPoint(a, i)).ToList();
    }

    public void Update(string name, string description, double radius, ValueObjects.Point point)
    {
        var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

        var circle = gf.CreatePoint(new NetTopologySuite.Geometries.Point(point.Longitude, point.Latitude).Coordinate).Buffer(radius);

        Name = name;
        Description = description;
        Radius = radius;
        Area = circle;

        Points = new Collection<ClientAreaPoint> { new ClientAreaPoint(point, 0) };
    }

    public ICollection<ClientUserArea> Users { get; init; }
    public IStatusHandler AddUser(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));

        var status = new StatusHandler();
        if (Users is null)
        {
            status.AddError(
                "Must first retrive the Users list",
                nameof(ClientUserArea));
            return status;
        }

        if (!Users.Any(a => a.UserId.Equals(userId)))
            Users.Add(new ClientUserArea(userId));

        return status;
    }
    public IStatusHandler RemoveUser(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));

        var status = new StatusHandler();
        if (Users is null)
        {
            status.AddError(
                "Must first retrive the Users list",
                nameof(ClientUserArea));
            return status;
        }

        var user = Users.FirstOrDefault(a => a.UserId.Equals(userId));

        if (user is not null)
            Users.Remove(user);

        return status;
    }

}
