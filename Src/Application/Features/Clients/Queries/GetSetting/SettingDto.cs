using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Mappings;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetSetting
{
    public class SettingDto : IMapFrom<ClientSetting>
    {
        public string InMessage { get; set; }
        public string OutMessage { get; set; }
        public string BreakInMessage { get; set; }
        public string BreakOutMessage { get; set; }
        public int AllowedBreak { get; set; }
        public int AllowedWorkHour { get; set; }
        public int AllowedOvertime { get; set; }
        public int AutoOutCycle { get; set; }
        public DayOfWeek FirstOffDay { get; set; }
        public DayOfWeek? SecondOffDay { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ClientSetting, SettingDto>()
                .ForMember(s => s.FirstOffDay, opt => opt.Ignore())
                .ForMember(s => s.SecondOffDay, opt => opt.Ignore());
        }
    }
}
