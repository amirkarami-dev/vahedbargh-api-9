using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<string> CreateUserAsync(string userName, string password, string email, string firstName, string lastName);

        Task<string> CreateUserAsync(string clientId, string id, string password, string email, CurrentUserTypeEnum userType
            , string firstName = "", string lastName = "", string phoneNumber = "", bool phoneNumberConfirmed = false,
            string nickName="");

        Task<UserData> GetUserAsync(string userId);

        Task<IList<string>> GetUserRolesAsync(string userId);

        Task AddToRolesAsync(string userId, List<string> roles);

        Task RemoveFromRoleAsync(string userId);
        Task RemoveUserAsync(string userId);
        Task RemoveFromRoleAsync(string userId, string role);
        
        Task ChangePassword(string userId, string oldPass, string newPass);

        Task<bool> ResetPassword(string userName, string newPassword);
        Task<bool> ResetDefaultPassword(string userName);

        Task<List<UserData>> GetUsers(string role);
        Task<List<UserData>> GetUsers(IEnumerable<string> ids);
        
        Task<List<UserData>> GetLeaderboard(string role);
        
        Task<List<UserData>> GetClientUsersByAdministrator(string userId);
        
        Task<List<UserData>> GetClientUsersByAdministrator(string adminId, IEnumerable<string> userIds);

        Task<List<UserData>> GetClientUsersByAdministrator(string userId, string name, IEnumerable<string> roles, int pageNumber, int rowCount);
        Task<List<UserData>> GetClientUsersByAdministrator(string userId, string name, IEnumerable<string> roles);
        Task<List<UserData>> GetUsers(IEnumerable<string> ids, string name, IEnumerable<string> roles, int pageNumber, int rowCount);
        Task<bool> IsInClient(string adminId, string userId);
        Task<int> GetClientUsersCount(string userId, string name, IEnumerable<string> roles);
        Task<int> GetClientUsersCount(string clientId);
        Task<int> GetUsersCount(IEnumerable<string> ids, string name, IEnumerable<string> roles);
        Task<List<KeyValue>> GetRoles();
        Task<string> GetRoleId(string roleName);
        Task<bool> IsRoleExist(string roleName);
        Task AddRole(string roleName);

        Task<UserData> GetUserByEmailAsync(string email);
        Task<UserData> GetUserByUserNameAsync(string userName);
        
        Task UpdateUser(string userId, string firstName, string lastName);
        Task UpdateUser(string userId, string firstName, string lastName, string nickName, string phoneNumber, bool active);
        Task UpdateUser(string naCode, string phoneNumber, bool active);
        Task UpdateUser(string userId, string firstName, string lastName, string avatar);
        Task UpdateUser(string userId, string clientId);

        Task UpdateScore(string userId, double score);
        Task UpdateActivate(string userId);
        Task<UserData> GetUser(string integrateId, string clientId);
        Task<List<UserData>> GetClientUsers(string clientId);
        Task<List<UserData>> GetClientAdmins(string clientId);
    }
}
