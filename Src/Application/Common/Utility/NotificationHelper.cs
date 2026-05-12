using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Common.Utility
{
    public static class NotificationHelper
    {
        public static string GenerateMessage(NotificationTypeEnum type, params object[] args) =>
            string.Format(GetMessageTemplate(type), args);

        private static string GetMessageTemplate(NotificationTypeEnum type) => type switch
        {
            NotificationTypeEnum.StartWork => "{0} {1} Start Working",
            NotificationTypeEnum.FinishWork => "{0} {1} Finish Working",
            NotificationTypeEnum.LeaveRequest => "{0} {1} Requested Leave",
            NotificationTypeEnum.TimesheetUpdateRequest => "{0} {1} Requested Timesheet Update",
            NotificationTypeEnum.BreakUpdateRequest => "{0} {1} Requested Break Update",
            NotificationTypeEnum.OutArea => "{0} {1} Trigger an Event Out of Area",
            _ => string.Empty,
        };

        public static string GenerateTitle(NotificationTypeEnum type) => type switch
        {
            NotificationTypeEnum.StartWork => "Work Started",
            NotificationTypeEnum.FinishWork => "Work Finished",
            NotificationTypeEnum.LeaveRequest => "Leave Requested",
            NotificationTypeEnum.TimesheetUpdateRequest => "Timesheet Update Requested",
            NotificationTypeEnum.BreakUpdateRequest => "Break Update Requested",
            NotificationTypeEnum.OutArea => "Out Area Event",
            _ => string.Empty,
        };
    }
}
