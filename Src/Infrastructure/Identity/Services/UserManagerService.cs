using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Coreapi.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Coreapi.Common.Enums;

namespace Coreapi.Infrastructure.Identity
{
    public class UserManagerService : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IdentityAppDbContext context;
        private readonly IFileService fileService;
        private readonly ISmsService _smsService;

        public UserManagerService(UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager,
             IdentityAppDbContext context,
             IFileService fileService,
             ISmsService smsService
            )
        {
            _userManager = userManager;
            this.context = context;
            this.roleManager = roleManager;
            this.fileService = fileService;
            _smsService = smsService;
        }

        public async Task<string> CreateUserAsync(string userName, string password, string email, string firstName = "", string lastName = "")
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Created = DateTime.Now,
                Active = true,
                Type = CurrentUserTypeEnum.JWT
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return null;

            return user.Id;
        }

        public async Task<string> CreateUserAsync(string clientId, string id, string password, string email, CurrentUserTypeEnum userType
            , string firstName = "", string lastName = "",string phoneNumber="",bool phoneNumberConfirmed=false,string nickName=""
            )
        {
            var user = new ApplicationUser
            {
                UserName = email.Split("@")[0],
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Created = DateTime.Now,
                Active = true,
                ClientId = clientId,
                IntegrateId = id,
                Type = userType,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = phoneNumberConfirmed,
                NickName = nickName,
                
            };

            var result = await _userManager.CreateAsync(user, password);

            return !result.Succeeded ? null : user.Id;
        }


        public async Task<UserData> GetUserAsync(string userId)
        {
            var user = await context.Users.AsQueryable().FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return null;

            return new UserData
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Score = user.Score,
                Active = user.Active,
                Avatar = user.Avatar,
                ClientId = user.ClientId,
                NickName = user.NickName,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                IdSection = user.IdSection,
                IdCity = user.IdCity
                
            
            };
        }
        public async Task<UserData> GetUserByEmailAsync(string email)
        {
            var user = await context.Users.AsQueryable().FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                return null;

            return new UserData
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Score = user.Score,
                Active = user.Active,
                ClientId = user.ClientId,
                IdSection = user.IdSection,
                IdCity = user.IdCity
            };
        }

        public async Task<UserData> GetUserByUserNameAsync(string userName)
        {
            var user = await context.Users.AsQueryable().FirstOrDefaultAsync(u => u.UserName == userName);

            if (user is null)
                return null;

            return new UserData
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Score = user.Score,
                Active = user.Active,
                ClientId = user.ClientId,
                IdSection = user.IdSection,
                IdCity = user.IdCity
            };
        }


        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.Active ? await _userManager.GetRolesAsync(user) : new List<string>();
        }

        public async Task AddToRolesAsync(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task RemoveFromRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
        }
        public async Task RemoveUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.DeleteAsync(user);
        }

        public async Task RemoveFromRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRolesAsync(user, new List<string>() { role });
        }

        public async Task ChangePassword(string userId, string oldPass, string newPass)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                var result = await _userManager.ChangePasswordAsync(user, oldPass, newPass);

                if (!result.Succeeded)
                    throw new AuthenticationException(result.Errors.Select(s => s.Description).ToArray());
            }

        }

        public async Task UpdateUser(string userId, string firstName, string lastName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task UpdateUser(string userId, string firstName, string lastName, string nickName, string phoneNumber, bool active)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.NickName = nickName;
                user.PhoneNumber = phoneNumber;
                user.Active = active;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task UpdateUser(string userId, string phoneNumber, bool active)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                user.PhoneNumber = phoneNumber;
                user.Active = true;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task UpdateUser(string userId, string firstName, string lastName, string avatar)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                if (string.IsNullOrEmpty(avatar))
                {
                    avatar = user.Avatar;
                }
                else if (!string.IsNullOrEmpty(user.Avatar))
                {
                    fileService.DeleteFile(user.Avatar);
                }

                user.FirstName = firstName;
                user.LastName = lastName;
                user.Avatar = avatar;
                await _userManager.UpdateAsync(user);
            }
        }
        public async Task UpdateUser(string userId, string clientId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                user.ClientId = clientId;
                await _userManager.UpdateAsync(user);
            }
        }



        public async Task UpdateScore(string userId, double score)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                user.Score += score;
                await _userManager.UpdateAsync(user);
            }
        }


        public async Task UpdateActivate(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                user.Active = !user.Active;
                await _userManager.UpdateAsync(user);
            }
        }


        public async Task<bool> ResetPassword(string userName, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userName);
            if (user is null)
                return false;

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            return true;
        }

        public async Task<bool> ResetDefaultPassword(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return false;

            //var newPassword = $"Abcd@{new Random().Next(1000, 9999)}";
            var newPassword = $"Abcd@{user.PhoneNumber.Substring(user.PhoneNumber.Length-4, 4)}";

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = $"u={userName}&p={newPassword}";
            var resultSms = await _smsService.SendSms3Params(user.PhoneNumber, 5519, userName,newPassword.ToString(), param);
            // if (resultSms.Status != "success") return false;
            await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            return true;
        }

        public async Task<List<UserData>> GetUsers(string role)
        {
            var user = await _userManager.GetUsersInRoleAsync(role);

            return user.Select(u => new UserData
            {
                UserName = u.UserName,
                Email = u.Email,
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Created = u.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Score = u.Score,
                Active = u.Active,
                ClientId = u.ClientId,
                Avatar = u.Avatar
            }).ToList();
        }

        public async Task<List<UserData>> GetLeaderboard(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            return users.OrderByDescending(o => o.Score).Take(15).Select(u => new UserData
            {
                UserName = u.UserName,
                Email = u.Email,
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Created = u.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Score = u.Score,
                Active = u.Active,
                ClientId = u.ClientId
            }).ToList();
        }

        public async Task<List<UserData>> GetClientUsers(string clientId)
        {
            return await context.Users.AsQueryable().Where(user => user.ClientId.Equals(clientId) && user.Active)
                .Select(user => new UserData
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName =user.FirstName,
                    LastName = user.LastName,
                    Avatar = user.Avatar,
                    Role = string.Join(',', context.Roles.AsQueryable().Where(r => context.UserRoles.Any(ur => ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))).Select(r => r.Name)),
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    ExpiryDate = user.ExpiryDate.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                    NickName = user.NickName,
                    Active = user.Active,
                    
                    


                }).ToListAsync();
        }

        public async Task<List<UserData>> GetClientAdmins(string clientId)
        {
            return await context.Users.AsQueryable().Where(user => user.ClientId.Equals(clientId)
            && context.Roles.Any(r =>  r.Name.Equals("Administrator") && context.UserRoles.Any(ur => ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))))
                .Select(user => new UserData
                {
                    Id = user.Id,
                    FirstName =user.FirstName,
                    LastName = user.LastName,
                    Role = string.Empty, 
                    IsAdminSupport = user.IsAdminSupport
                }).ToListAsync();
        }

        public async Task<List<UserData>> GetClientUsersByAdministrator(string userId)
        {
            return await context.Users.AsQueryable().Where(user => context.Users.AsQueryable().Any(u => u.Id == userId && user.ClientId == u.ClientId))
                .Select(user => new UserData
                {
                    Email = user.Email,
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Created = user.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                    Role = string.Join(',', context.Roles.AsQueryable().Where(r => context.UserRoles.Any(ur => ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))).Select(r => r.Name)),
                    Avatar = user.Avatar
                })
            .OrderBy(r => r.FirstName)
            .ToListAsync();
        }

        public async Task<List<UserData>> GetClientUsersByAdministrator(string adminId, IEnumerable<string> userIds)
        {
            return await context.Users.AsQueryable().Where(user => userIds.Any(id=>id.Equals(user.Id))
            && context.Users.AsQueryable().Any(u => u.Id == adminId && user.ClientId == u.ClientId))
                .Join(context.UserRoles, u => u.Id, ur => ur.UserId, (user, role) => new { user, role })
                .Join(context.Roles, u => u.role.RoleId, r => r.Id, (userRole, role) => new UserData
                {
                    Id = userRole.user.Id,
                    RoleId = role.Id
                }).ToListAsync();
        }

        public async Task<List<UserData>> GetClientUsersByAdministrator(string userId, string name, IEnumerable<string> roles, int pageNumber, int rowCount)
        {
            return await context.Users.AsQueryable().Where(user => (string.IsNullOrEmpty(name) || user.FirstName.Contains(name) || user.LastName.Contains(name))
            && context.Users.AsQueryable().Any(u => u.Id == userId && user.ClientId == u.ClientId)            
            && context.Roles.Any(r => !roles.Any() || (roles.Any(rr => r.Name.Equals(rr)) && context.UserRoles.Any(ur=>ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))
            ))).Select(user => new UserData
            {
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Role = string.Join(',', context.Roles.AsQueryable().Where(r=> context.UserRoles.Any(ur => ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))).Select(r=>r.Name)),
                Avatar = user.Avatar,
                Active = user.Active
            })
            .OrderBy(r=>r.FirstName)
            .Skip(pageNumber * rowCount).Take(rowCount)
            .ToListAsync();

        }

        public async Task<List<UserData>> GetClientUsersByAdministrator(string userId, string name, IEnumerable<string> roles)
        {
            return await context.Users.AsQueryable().Where(user => (string.IsNullOrEmpty(name) || user.FirstName.Contains(name) || user.LastName.Contains(name))
            && context.Users.AsQueryable().Any(u => u.Id == userId && user.ClientId == u.ClientId)            
            && context.Roles.Any(r => !roles.Any() || (roles.Any(rr => r.Name.Equals(rr)) && context.UserRoles.Any(ur=>ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))
            ))).Select(user => new UserData
            {
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Role = string.Join(',', context.Roles.AsQueryable().Where(r=> context.UserRoles.Any(ur => ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))).Select(r=>r.Name)),
                Avatar = user.Avatar,
                Active = user.Active
            })
            .ToListAsync();

        }

        public async Task<List<UserData>> GetUsers(IEnumerable<string> ids, string name, IEnumerable<string> roles, int pageNumber, int rowCount)
        {
            return await context.Users.AsQueryable().Where(user => (string.IsNullOrEmpty(name) || user.FirstName.Contains(name) || user.LastName.Contains(name))
            && (!ids.Any() || ids.Any(id=>user.Id.Equals(id)))
            && context.Roles.Any(r => !roles.Any() || (roles.Any(rr => r.Name.Equals(rr)) && context.UserRoles.Any(ur=>ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))
            ))).Select(user => new UserData
            {
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Role = string.Join(',', context.Roles.AsQueryable().Where(r=> context.UserRoles.Any(ur => ur.RoleId.Equals(r.Id) && ur.UserId.Equals(user.Id))).Select(r=>r.Name)),
                Avatar = user.Avatar,
                Active = user.Active
            })
            .OrderBy(r=>r.FirstName)
            .Skip(pageNumber * rowCount).Take(rowCount)
            .ToListAsync();
        }

        public async Task<bool> IsInClient(string adminId, string userId)
        {
            return await context.Users.AsQueryable().AnyAsync(admin => admin.Id.Equals(adminId)
            && context.Users.AsQueryable().Any(user => user.Id.Equals(userId) && admin.ClientId.Equals(user.ClientId)));
        }

        public async Task<int> GetClientUsersCount(string userId, string name, IEnumerable<string> roles)
        {
            return await context.Users.AsQueryable().Where(user => (string.IsNullOrEmpty(name) || user.FirstName.Contains(name) || user.LastName.Contains(name))
            && context.Users.AsQueryable().Any(u => u.Id == userId && user.ClientId == u.ClientId))
                .Join(context.UserRoles
                , u => u.Id, ur => ur.UserId, (user, role) => new { user, role })
                .Join(context.Roles.AsQueryable().Where(ur => !roles.Any() || roles.Any(r => ur.Name.Equals(r)))
                , u => u.role.RoleId, r => r.Id, (userRole, role) => new { userRole, role})
                  .CountAsync();
        }

        public async Task<int> GetClientUsersCount(string clientId)
        {
            return await context.Users.AsQueryable().Where(user => user.ClientId.Equals(clientId))
                  .CountAsync();
        }

        public async Task<int> GetUsersCount(IEnumerable<string> ids, string name, IEnumerable<string> roles)
        {
            return await context.Users.AsQueryable().Where(user => (string.IsNullOrEmpty(name) || user.FirstName.Contains(name) || user.LastName.Contains(name))
            && (!ids.Any() || ids.Any(id => user.Id.Equals(id))))
                .Join(context.UserRoles
                , u => u.Id, ur => ur.UserId, (user, role) => new { user, role })
                .Join(context.Roles.AsQueryable().Where(ur => !roles.Any() || roles.Any(r => ur.Name.Equals(r)))
                , u => u.role.RoleId, r => r.Id, (userRole, role) => new { userRole, role })
                    .CountAsync();
        }

        public async Task<List<KeyValue>> GetRoles()
        {
            return await roleManager.Roles.Select(r =>
                    new KeyValue { Key = r.Id, Value = r.Name }).ToListAsync();
        }
        public async Task<string> GetRoleId(string roleName)
        {
            return await roleManager.Roles.Where(r => r.Name == roleName)
                        .Select(v => v.Id).FirstOrDefaultAsync() ?? string.Empty;
        }

        public async Task<bool> IsRoleExist(string roleName)
        {
            return await roleManager.Roles.AnyAsync(r=>r.NormalizedName.Equals(roleName.ToUpper()));
        }

        public async Task AddRole(string roleName)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName) { NormalizedName = roleName.ToUpper() });
        }

        public async Task<UserData> GetUser(string integrateId, string clientId)
        {
            var user = await context.Users.AsQueryable().FirstOrDefaultAsync(u => u.IntegrateId.Equals(integrateId) && u.ClientId.Equals(clientId));

            if (user is null)
                return null;

            return new UserData
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
                Score = user.Score,
                Active = user.Active,
                ClientId = user.ClientId
            };
        }

        public async Task<List<UserData>> GetUsers(IEnumerable<string> ids)
        {
            return await context.Users.AsQueryable().Where(u => ids.Any(id => id.Equals(u.Id)))
                .Select(u => new UserData
                {
                    Id = u.Id,
                    Avatar = u.Avatar,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).ToListAsync();
        }
    }
}
