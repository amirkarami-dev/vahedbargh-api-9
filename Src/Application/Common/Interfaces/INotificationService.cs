using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendConfirmationToUser(string userId);
        Task SendChangeStateToUser(string userId, string clientId, string status);
        Task SendMobileNotification(string userId, string title, string message, string body);
        Task SendMobileNotification(IEnumerable<string> userIds, string title, string message, string body);
        Task SendWebNotification(string userId, string title, string message, string body);
        Task SendWebNotification(IEnumerable<string> userIds, string title, string message, string body);
        Task SendMessageNotification(string userId, string message, int countUnreadMessage);
    }
}
