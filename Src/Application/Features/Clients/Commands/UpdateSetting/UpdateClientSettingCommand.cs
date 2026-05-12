using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpdateSetting
{
    public class UpdateClientSettingCommand : IRequest
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
    }
}
