using Microsoft.AspNetCore.Http;
using System;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;

namespace Coreapi.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                UserId = httpContextAccessor.HttpContext?.User?.FindFirst("sid").Value;
                Role = httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
                if (!Role.Equals("SuperUser") && !Role.Equals("Analyzer"))
                {
                    ClientId = httpContextAccessor.HttpContext?.User?.FindFirst("cid").Value;
                    Subdomain = httpContextAccessor.HttpContext?.User?.FindFirst("sbd").Value;
                }
            }
            catch
            {
                UserId = null;
                Role = "";
                ClientId = null;
            }
            try
            {
                Ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            catch { }
        }

        public string UserId { get; }
        public string Ip { get; }
        public string ClientId { get; }
        public string Subdomain { get; }
        public string Role { get; }
    }
}
