using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate
{
    public class ClientUserSetting
    {
        private ClientUserSetting()
        {
        }

        public ClientUserSetting(string userId, DayOfWeek firstOffDay, DayOfWeek? secondOffDay)
        {
            UserId = userId;
            FirstOffDay = firstOffDay;
            SecondOffDay = secondOffDay;
            Type = UserSettingTypeEnum.Default;
        }

        public void Update(DayOfWeek firstOffDay, DayOfWeek? secondOffDay)
        {
            FirstOffDay = firstOffDay;
            SecondOffDay = secondOffDay;
        }

        public void UpdateType(UserSettingTypeEnum type)
        {
            Type = type;
        }

        public string UserId { get; init; }
        public DayOfWeek FirstOffDay { get; private set; }
        public DayOfWeek? SecondOffDay { get; private set; }
        public UserSettingTypeEnum Type { get; private set; }
    }
}
