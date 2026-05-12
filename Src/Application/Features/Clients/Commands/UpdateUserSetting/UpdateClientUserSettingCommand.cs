using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpdateUserSetting
{
    public class UpdateClientUserSettingCommand : IRequest
    {
        public string UserId { get; set; }

        public bool IsCustom { get; set; }

        public DayOfWeek FirstOffDay { get; set; }
        public DayOfWeek? SecondOffDay { get; set; }
    }
}
