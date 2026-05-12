using System.Threading.Tasks;
using Coreapi.Application.Common.Models;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Common.Interfaces
{
    public interface ISignInManager
    {
        Task<LoginOutput> Login(string userName, string password, string deviceToken, PlatformTypeEnum platformType);
        Task<LoginOutput> Login(string mobileCode, string userName);
        Task<LoginOutput> OneMinuteLogin(string userName, string password, string deviceToken, PlatformTypeEnum platformType);
        Task<LoginOutput> Login(string userName, string password, string macAddress);
        Task<LoginOutput> GoogleAuthenticate(string tokenId);
        Task<LoginOutput> MicrosoftAuthenticate(string tokenId);
        Task<LoginOutput> RefreshToken(string UserId, string RefreshToken, bool oneMinute = false);
        Task RemoveSessionToken(string UserId);
    }
}
