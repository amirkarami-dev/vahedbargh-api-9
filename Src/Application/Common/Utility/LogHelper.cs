using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Common.Utility
{
    public static class LogHelper
    {
        public static string GetMessage(ActivityLogTypeEnum type, ActivityLogClassificationEnum classification) 
            => (type, classification) switch
            {
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.AddAttachment) => "User {0} added attachment to Task {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.AddComment) => "User {0} left a comment on Task {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.AssignLabel) => "Label {0} Assigned to Task {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.RemoveLabel) => "Label {0} Removed from Task {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.AssignUser) => "User {0} Assigned to Task {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.RemoveUser) => "User {0} Removed from Task {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.DeleteComment) => "User {0} deleted comment on Task {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.Insert) => "Task {0} Added to Section {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.Update) => "Task {0} Has Updated",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.UpdatePriority) => "Task {0} reordered to {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.UpdateSection) => "Task {0} has moved to Section {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.AssignProject) => "Task {0} Assigned to Project {1}",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.UnassignProject) => "Task {0} Assigned to No Project",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.Archive) => "Task {0} Archived",
                (ActivityLogTypeEnum.Task, ActivityLogClassificationEnum.Unarchive) => "Task {0} Unarchived",
                (ActivityLogTypeEnum.Section, ActivityLogClassificationEnum.AssignLabel) => "Label {0} Assigned to Section {1}",
                (ActivityLogTypeEnum.Section, ActivityLogClassificationEnum.RemoveLabel) => "Label {0} Removed from Section {1}",
                (ActivityLogTypeEnum.Section, ActivityLogClassificationEnum.Insert) => "Section {0} Added to Team",
                (ActivityLogTypeEnum.Section, ActivityLogClassificationEnum.Update) => "Section {0} Has Updated",
                (ActivityLogTypeEnum.Section, ActivityLogClassificationEnum.UpdatePriority) => "Section {0} reordered to {1}",
                (ActivityLogTypeEnum.Section, ActivityLogClassificationEnum.Archive) => "Section {0} Archived",
                (ActivityLogTypeEnum.Section, ActivityLogClassificationEnum.Unarchive) => "Section {0} Unarchived",
                _ => string.Empty,
            };
    }
}
