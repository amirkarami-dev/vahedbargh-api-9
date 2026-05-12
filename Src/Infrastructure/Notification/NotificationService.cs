using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Coreapi.Infrastructure.Identity.Models;
using Coreapi.Common.Utility;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Infrastructure.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly string _pushBaseUrl;
        private readonly NotificationDbContext _context;

        public NotificationService(IHubContext<NotificationHub> hubContext, NotificationDbContext context, IConfiguration configuration)
        {
            _hubContext = hubContext;
            _context = context;
            _pushBaseUrl = configuration["PushNotificationBaseUrl"];
        }

        public async Task SendChangeStateToUser(string userId, string clientId, string status)
        {
            await _hubContext.Clients.Group(userId).SendAsync("ChangeState");
            await _hubContext.Clients.Group(clientId).SendAsync("ChangeStateGroup", new { userId, status });
        }

        public async Task SendConfirmationToUser(string userId)
        {
            await _hubContext.Clients.Group(userId).SendAsync("StateConfirm");

            var tokens = await _context.UserTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            var url = $"{_pushBaseUrl}/api/push";

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), url);

            var body = JsonConvert.SerializeObject(new
            {
                notifications = tokens.GroupBy(t => t.Platform)
                    .Select(t => new
                    {
                        tokens = t.Select(token => token.Token),
                        platform = (int)t.Key,
                        title = "Alert",
                        message = "Are You Still Working?",
                        data = JsonConvert.SerializeObject(new { method = "StateConfirm" })
                    }).ToList()
            });

            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await httpClient.SendAsync(request);
        }

        public async Task SendMobileNotification(string userId, string title, string message, string data)
        {
            var tokens = await _context.UserTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            var url = $"{_pushBaseUrl}/api/push";

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), url);

            var body = JsonConvert.SerializeObject(new
            {
                notifications = tokens.GroupBy(t => t.Platform)
                    .Select(t => new
                    {
                        tokens = t.Select(token => token.Token),
                        platform = (int)t.Key,
                        title,
                        message,
                        data
                    }).ToList()
            });

            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await httpClient.SendAsync(request);
        }

        public async Task SendMobileNotification(IEnumerable<string> userIds, string title, string message, string data)
        {
            var tokens = await _context.UserTokens
                .Where(t => userIds.Contains(t.UserId))
                .ToListAsync();

            var url = $"{_pushBaseUrl}/api/push";

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), url);

            var body = JsonConvert.SerializeObject(new
            {
                notifications = tokens.GroupBy(t => t.Platform)
                    .Select(t => new
                    {
                        tokens = t.Select(token => token.Token),
                        platform = (int)t.Key,
                        title,
                        message,
                        data
                    }).ToList()
            });

            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await httpClient.SendAsync(request);
        }

        public async Task SendWebNotification(string userId, string title, string message, string body)
        {
            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotif", new { title, message, body });
        }

        public async Task SendWebNotification(IEnumerable<string> userIds, string title, string message, string body)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotif", new { title, message, body });
            }
        }

        public async Task SendMessageNotification(string userId,string message, int countUnreadMessage)
        {
            await _hubContext.Clients.Group(userId).SendAsync("NewMessage",new {message, countUnreadMessage});

        }
    }
}