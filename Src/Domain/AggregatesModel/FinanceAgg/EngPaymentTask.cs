using Coreapi.Domain.AggregatesModel.ClientAggregate;
using System;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg;

public class EngPaymentTask
{
    private EngPaymentTask(){}


    public EngPaymentTask(Client client, string solarCreated, DateTime julianCreated, int period, string fromSolar, DateTime fromJulian, string toSolar, DateTime toJulian, string des, string filePaymentName)
    {
        Id = Guid.NewGuid();
        Client = client;
        SolarCreated = solarCreated;
        JulianCreated = julianCreated;
        Period = period;
        FromSolar = fromSolar;
        FromJulian = fromJulian;
        ToSolar = toSolar;
        ToJulian = toJulian;
        Des = des;
        FilePaymentName = filePaymentName;
        Approved = false;
    }

    public Guid Id { get; init; }
    public Client Client { get; set; }

    public string SolarCreated { get; set; }
    public DateTime JulianCreated { get; set; }
    public int Period { get; set; }
    public string FromSolar { get; set; }
    public DateTime FromJulian { get; set; }
    public string ToSolar { get; set; }
    public DateTime ToJulian { get; set; }
    public string SolarPay { get; set; }
    public DateTime? JulianPay { get; set; }
    public string Des { get; set; }
    public string FilePaymentName { get; set; }
    public bool Approved { get; set; }


    public void PayTask(string solarPay, DateTime julianPay)
    {
        SolarPay = solarPay;
        JulianPay = julianPay;
        Approved = true;
    }
}