using System;

namespace Coreapi.Domain.AggregatesModel.SupportAgg;

public class SupportMessage
{
    private SupportMessage(){}
    public SupportMessage(string message, string solarCreated, DateTime julianCreated, string userId, string fileName, string toUserId, string name, string toName, Support support)
    {
        Id = Guid.NewGuid();
        Message = message;
        SolarCreated = solarCreated;
        JulianCreated = julianCreated;
        UserId = userId;
        FileName = fileName;
        ToUserId = toUserId;
        Name = name;
        ToName = toName;
        IsSend = true;
        IsReceive = false;
        IsReadByReceiver = false;
        Support = support;
    }
    public Guid Id { get; init; }
    public string Message { get; set; }
    public string SolarCreated { get; set; }
    public DateTime JulianCreated { get; set; }
    public string UserId { get; set; }
    public string FileName { get; set; }
    public string ToUserId { get; set; }
    public string Name { get; set; }
    public string ToName { get; set; }
    public bool IsSend { get; set; }
    public bool IsReceive { get; set; }
    public bool IsReadByReceiver { get; set; }
    public Support Support { get; set; }

    public void UpdateReceive(bool isReceive)
    {
        IsReceive = isReceive;
    }

    public void MarkAsReadByReceiver()
    {
        IsReadByReceiver = true;
    }
}