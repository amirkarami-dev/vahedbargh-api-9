using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;

namespace Coreapi.Application.Features.QuarterTariffs.Queries.GetListQuarterTariff
{
    public class ListQuarterTariffDto:IMapFrom<QuarterTariff>
    {
        public Guid Id { get; init; }
        public QuarterTypeEnum QuarterTypeEnum { get; set; }
        public string QuarterTypeName { get; set; }
        public int Year { get; set; }
        public AllotmentRoundTypeEnum AllotmentRoundTypeEnum { get; set; }
        public string AllotmentRoundTypeName { get; set; }
        public long Fee { get; set; }
        public bool IsQuota { get; set; } = false;
        public DateTime JulianAllotmentDate { get; set; }
        public string SolarAllotmentDate { get; set; }
        public int OldId { get; set; }
        public int Period { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<QuarterTariff, ListQuarterTariffDto>()
                .ForMember(destination => destination.QuarterTypeName,
                    opt => opt.MapFrom(source => source.QuarterTypeEnum.GetDisplayName()))
                .ForMember(destination => destination.AllotmentRoundTypeName,
                    opt => opt.MapFrom(source => source.AllotmentRoundTypeEnum.GetDisplayName()))
                ;
        }

    }
}
