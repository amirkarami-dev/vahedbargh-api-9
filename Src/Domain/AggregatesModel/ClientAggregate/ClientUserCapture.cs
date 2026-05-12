using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate;

public class ClientUserCapture
{
    public ClientUserCapture(Client client,string userId, double latitude, double longitude, Guid refId
        , CaptureRefrenceEnum refType, CaptureEventEnum eventType, CaptureStatusEnum status)
    {
        Id = Guid.NewGuid();
        Client = client;
        Created = DateTime.Now;
        UserId = userId;
        Latitude = latitude;
        Longitude = longitude;
        RefId = refId;
        RefType = refType;
        EventType = eventType;
        Status = status;
    }

    private ClientUserCapture()
    {
    }

    public Guid Id { get; init; }
    public Client Client { get; init; }
    public DateTime Created { get; set; }
    public string UserId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public Guid RefId { get; init; }
    public CaptureRefrenceEnum RefType { get; init; }
    public CaptureEventEnum EventType { get; init; }
    public CaptureStatusEnum Status { get; init; }
}
