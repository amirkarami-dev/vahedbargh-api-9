using System;

namespace Coreapi.Common.Models;

public class ElecSmsLog
{
    private ElecSmsLog(){}

    public ElecSmsLog(string phoneNumber, string message, string response, string status, string solarDateSend, DateTime julianDateSend,string fullName, string description)
    {
        PhoneNumber = phoneNumber;
        Message = message;
        Response = response;
        Status = status;
        SolarDateSend = solarDateSend;
        JulianDateSend = julianDateSend;
        FullName = fullName;
        Description = description;
    }

    public long Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public string Response { get; set; }
    public string Status { get; set; }
    public string SolarDateSend { get; set; }
    public DateTime JulianDateSend { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }

}