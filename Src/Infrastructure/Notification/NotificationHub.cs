using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Infrastructure.Notification
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ICurrentUser _currentUser;
        private readonly NotificationDbContext _context;

        public NotificationHub(ICurrentUser currentUser, NotificationDbContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task AddToGroup(string connectionId, string groupName)
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task RemoveFromGroup(string connectionId, string groupName)
        {
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
        }

        public override async Task OnConnectedAsync()
        {
            var userConnection = await _context.UserConnections.FindAsync(_currentUser.UserId);
            if (userConnection != null)
            {
                userConnection.ConnectionId = Context.ConnectionId;
                _context.UserConnections.Update(userConnection);
            }
            else
            {
                await _context.UserConnections.AddAsync(new UserConnection
                {
                    UserId = _currentUser.UserId,
                    ConnectionId = Context.ConnectionId
                });
            }

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(_currentUser.ClientId))
                await AddToGroup(Context.ConnectionId, _currentUser.ClientId);
            if (!string.IsNullOrEmpty(_currentUser.UserId))
                await AddToGroup(Context.ConnectionId, _currentUser.UserId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userConnection = await _context.UserConnections.FindAsync(_currentUser.UserId);
            if (userConnection != null)
            {
                var connectionId = userConnection.ConnectionId;
                _context.UserConnections.Remove(userConnection);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(_currentUser.ClientId))
                    await RemoveFromGroup(Context.ConnectionId, _currentUser.ClientId);
                if (!string.IsNullOrEmpty(_currentUser.UserId))
                    await RemoveFromGroup(connectionId, _currentUser.UserId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}