using System;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Domain.AggregatesModel.ErtTariffAgg;

public class ErtTariff
{
    private ErtTariff(){}

    public ErtTariff(Guid id, Client client, string userId, ErtSystemTypeEnum ertSystemTypeEnum, long tariff, float factor, DateTime julianDateExecute, string solarDateExecute)
    {
        Id = Guid.NewGuid();
        Client = client;
        UserId = userId;
        ErtSystemTypeEnum = ertSystemTypeEnum;
        Tariff = tariff;
        Factor = factor;
        JulianDateExecute = julianDateExecute;
        SolarDateExecute = solarDateExecute;
    }


    public Guid Id { get; set; }
    public Client Client { get; set; }
    public string UserId { get; set; }
    public ErtSystemTypeEnum ErtSystemTypeEnum { get; set; }
    public long Tariff { get; set; }
    public float Factor { get; set; }
    public DateTime JulianDateExecute { get; private set; }
    public string SolarDateExecute { get; set; }

}