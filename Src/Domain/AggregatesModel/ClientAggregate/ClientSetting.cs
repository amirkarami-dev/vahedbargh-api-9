using System;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate
{
    public class ClientSetting 
    {
        public ClientSetting(string inMessage, string outMessage, string breakInMessage, string breakOutMessage
            , int allowedBreak, int allowedWorkHour, int allowedOvertime, int autoOutCycle, DayOfWeek firstOffDay, DayOfWeek? secondOffDay)
        {
            Id = Guid.NewGuid();
            InMessage = inMessage ?? throw new ArgumentNullException(nameof(inMessage));
            OutMessage = outMessage ?? throw new ArgumentNullException(nameof(outMessage));
            BreakInMessage = breakInMessage ?? throw new ArgumentNullException(nameof(breakInMessage));
            BreakOutMessage = breakOutMessage ?? throw new ArgumentNullException(nameof(breakOutMessage));
            AllowedBreak = allowedBreak;
            AllowedWorkHour = allowedWorkHour;
            AllowedOvertime = allowedOvertime;
            DefaultFirstOffDay = firstOffDay;
            DefaultSecondOffDay = secondOffDay;
            AutoOutCycle = autoOutCycle;
        }

        private ClientSetting() { }

        public void Update(string inMessage, string outMessage, string breakInMessage, string breakOutMessage
            , int allowedBreak, int allowedWorkHour, int allowedOvertime, int autoOutCycle, DayOfWeek firstOffDay, DayOfWeek? secondOffDay)
        {
            InMessage = inMessage ?? throw new ArgumentNullException(nameof(inMessage));
            OutMessage = outMessage ?? throw new ArgumentNullException(nameof(outMessage));
            BreakInMessage = breakInMessage ?? throw new ArgumentNullException(nameof(breakInMessage));
            BreakOutMessage = breakOutMessage ?? throw new ArgumentNullException(nameof(breakOutMessage));
            AllowedBreak = allowedBreak;
            AllowedWorkHour = allowedWorkHour;
            AllowedOvertime = allowedOvertime;
            DefaultFirstOffDay = firstOffDay;
            DefaultSecondOffDay = secondOffDay;
            AutoOutCycle = autoOutCycle;
        }

        public Guid Id { get; init; }
        public string InMessage { get; private set; }
        public string OutMessage { get; private set; }
        public string BreakInMessage { get; private set; }
        public string BreakOutMessage { get; private set; }
        public int AllowedBreak { get; private set; }
        public int AllowedWorkHour { get; private set; }
        public int AllowedOvertime { get; private set; }
        public int AutoOutCycle { get; private set; }
        public DayOfWeek DefaultFirstOffDay { get; private set; }
        public DayOfWeek? DefaultSecondOffDay { get; private set; }

    }
}
