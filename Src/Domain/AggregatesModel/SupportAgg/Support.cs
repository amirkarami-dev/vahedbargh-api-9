using System;
using System.Threading;
using System.Xml.Linq;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Domain.AggregatesModel.SupportAgg;

public class Support
{
    private Support(){}


    public Support(int ticketNumber, Client client, string userId, string toUserId, string username, string name, string toName, UserType userType, string solarCreate, DateTime julianCreate, string solarEndSupport, DateTime? julianEndSupport, string title, long fileNumber, int rate, bool isRead, bool closed, string field1, string field2)
    {
        Id = Guid.NewGuid();
        TicketNumber = ticketNumber;
        Client = client;
        UserId = userId;
        ToUserId = toUserId;
        Username = username;
        Name = name;
        ToName = toName;
        UserType = userType;
        SolarCreate = solarCreate;
        JulianCreate = julianCreate;
        SolarEndSupport = solarEndSupport;
        JulianEndSupport = julianEndSupport;
        Title = title;
        FileNumber = fileNumber;
        Rate = rate;
        IsRead = isRead;
        Closed = closed;
        Field1 = field1;
        Field2 = field2;
    }

    public void ClosedSupport(bool closed)
    {
        JulianEndSupport = DateTime.Now;
        SolarEndSupport = Helper.MiladiToShamsi(DateTime.Now);
        Closed = closed;
    }

    public void SwitchToRole(string toUserId, string toName)
    {
        ToUserId = toUserId;
        ToName = ToName;
    }

    public void UpdateRead(bool isRead)
    {
        IsRead = isRead;
    }
    public Guid Id { get; init; }
    public int  TicketNumber { get; set; }
    public Client Client { get; set; }
    public string UserId { get; set; }
    public string ToUserId { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string ToName { get; set; }
    public UserType UserType { get; set; }
    public string SolarCreate { get; set; }
    public DateTime JulianCreate { get; set; }
    public string SolarEndSupport { get; set; }
    public DateTime? JulianEndSupport { get;set; }
    public string Title { get; set; }
    public long FileNumber { get; set; }
    public int Rate { get; set; }
    public bool IsRead { get; set; }
    public bool Closed { get; set; }
    public string Field1 { get; set; }
    public string Field2 { get; set; }


}