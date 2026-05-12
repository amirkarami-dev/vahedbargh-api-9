using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models;
using Coreapi.Application.Common.Models.RocketChat;

namespace Coreapi.Application.Common.Interfaces
{
    public interface IRocketChatService
    {
        Task<RocketChatUserInfoDto> GetAllUsers(string url, string token, string userId);
        Task<bool> CheckToken(string chatUrl, string token, string userId);
        Task<RocketChatLoggedInUserDto> Login(string url, string username, string password);
    }
}
