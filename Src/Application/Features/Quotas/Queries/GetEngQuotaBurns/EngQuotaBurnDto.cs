using System;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;

namespace Coreapi.Application.Features.Quotas.Queries.GetEngQuotaBurns;

public class EngQuotaBurnDto :IMapFrom<EngQuotaBurn>
{
    public Guid Id { get; init; }
    public QuarterTariff QuarterTariff { get; set; }
    public string QuarterTypeName { get; set; }

    public Engineer Engineer { get; set; }
    public string AllotmentRoundTypeName { get; set; }

    public long AmountRemaining { get; set; }
    public long AmountBurning { get; set; }
    public int ErtCountBurning { get; set; }

    public double InspectionDelayFactor { get; set; }
    public double ErtDelayFactor { get; set; }

    public bool Approved { get; set; }
    public string Des { get; set; }
    public string QuotaDes { get; set; }
    

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EngQuotaBurn, EngQuotaBurnDto>()
            .ForMember(destination => destination.Engineer,
                otp => otp.MapFrom(source => source.Engineer))
            .ForMember(destination => destination.QuarterTypeName,
                opt => opt.MapFrom(source => source.QuarterTariff.QuarterTypeEnum.GetDisplayName()))
            .ForMember(destination => destination.AllotmentRoundTypeName,
                opt => opt.MapFrom(source => source.QuarterTariff.AllotmentRoundTypeEnum.GetDisplayName()))
            ;
    }
}