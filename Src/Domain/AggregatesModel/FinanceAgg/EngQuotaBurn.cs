using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using System;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg;

public class EngQuotaBurn
{
    private EngQuotaBurn()
    {
    }


    public EngQuotaBurn(
        Client client, 
        QuarterTariff quarterTariff, 
        Engineer engineer, 
        long amountRemaining,
        long amountBurning,
        int ertCountRemaining,
        int ertCountBurning,
        double inspectionDelayFactor,
        double ertDelayFactor,
        bool approved, 
        string des)
    {
        Id = Guid.NewGuid();
        Client = client;
        QuarterTariff = quarterTariff;
        Engineer = engineer;
        AmountRemaining = amountRemaining;
        AmountBurning = amountBurning;
        ErtCountRemaining = ertCountRemaining;
        ErtCountBurning = ertCountBurning;
        InspectionDelayFactor = inspectionDelayFactor;
        ErtDelayFactor = ertDelayFactor;
        Approved = approved;
        Des = des;
    }


    public Guid Id { get; init; }
    public Client Client { get; set; }
    public QuarterTariff QuarterTariff { get; set; }
    public Engineer Engineer { get; set; }

    public long AmountRemaining { get; set; }
    public long AmountBurning { get; set; }
    public int ErtCountRemaining { get; set; }
    public int ErtCountBurning { get; set; }
    public double InspectionDelayFactor { get; set; }

    public double ErtDelayFactor { get; set; }

    public bool Approved { get; set; }
    public string Des { get; set; }

    public void Update(
        long amountRemaining, 
        long amountBurning,
        int ertCountRemaining,
        int ertCountBurning,
        double inspectionDelayFactor,
        double ertDelayFactor,
        bool approved, 
        string des)
    {
        AmountRemaining = amountRemaining;
        AmountBurning = amountBurning;
        ErtCountRemaining = ertCountRemaining;
        ErtCountBurning = ertCountBurning;
        InspectionDelayFactor = inspectionDelayFactor;
        ErtDelayFactor = ertDelayFactor;
        Approved = approved;
        Des = des;

    }


    public void OnApproved(bool approve)
    {
        Approved = approve;
    }
}