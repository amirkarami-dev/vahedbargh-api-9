using System;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;

namespace Coreapi.Domain.AggregatesModel.QuarterIncrease;

public class QuarterIncrease
{
    private QuarterIncrease(){}

    public QuarterIncrease( Engineer engineer, QuarterTariff quarterTariff)
    {
        Id = Guid.NewGuid();
        Engineer = engineer;
        QuarterTariff = quarterTariff;
    }

    public Guid Id { get; set; }
    public Engineer Engineer { get; set; }
    public QuarterTariff QuarterTariff { get; set; }
}