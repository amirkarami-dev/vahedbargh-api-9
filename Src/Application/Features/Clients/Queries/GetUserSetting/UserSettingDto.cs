using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetUserSetting
{
    public class UserSettingDto : IMapFrom<ClientUserSetting>
    {
        public string UserId { get; set; }
        public DayOfWeek FirstOffDay { get; set; }
        public DayOfWeek? SecondOffDay { get; set; }
        public UserSettingTypeEnum Type { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ClientUserSetting, UserSettingDto>();
        }
    }
}
