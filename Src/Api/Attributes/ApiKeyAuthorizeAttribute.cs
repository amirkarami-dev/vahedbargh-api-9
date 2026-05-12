using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Api.Attributes
{
    public class ApiKeyAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IClientRepository clientRepository;
        private readonly IUserManager userManager;
        private readonly IConfiguration configuration;

        public ApiKeyAuthorizeFilter(IClientRepository clientRepository, IUserManager userManager, IConfiguration configuration)
        {
            this.clientRepository = clientRepository;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if ((!context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Api-Key"))
            || (!context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Rocket-Id"))
            && !context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Microsoft-Id"))))
            && string.IsNullOrEmpty(context.HttpContext.User?.FindFirst("sid")?.Value))
            {
                context.Result = new JsonResult(new { }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            if ((!context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Api-Key"))
            || (!context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Rocket-Id"))
            && !context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Microsoft-Id"))))
            && (string.IsNullOrEmpty(context.HttpContext.User?.FindFirst("pkg")?.Value) || !bool.Parse(context.HttpContext.User?.FindFirst("pkg")?.Value)))
            {
                context.Result = new JsonResult(new { }) { StatusCode = StatusCodes.Status402PaymentRequired };
                return;
            }

            if(!string.IsNullOrEmpty(context.HttpContext.User?.FindFirst("sbd")?.Value))
            {
                if (context.HttpContext.Request.Headers.TryGetValue("Origin", out StringValues originValues))
                {
                    var jwtSubDomain = context.HttpContext.User?.FindFirst("sbd")?.Value;
                    string exeptableUrls = configuration["ExeptableUrls"];

                    try
                    {
                        if (!originValues.Any(origin => (origin.ToLower().StartsWith("http") ? origin.Split("://").Last().Split(".").First().ToLower().Equals(jwtSubDomain.ToLower())
                        : origin.Split(".").First().ToLower().Equals(jwtSubDomain.ToLower())) || exeptableUrls.Split(',').Any(url => origin.ToLower().StartsWith(url.Trim().ToLower()) || url.Trim().ToLower().StartsWith(origin.ToLower()))))
                        {
                            context.Result = new JsonResult("Invalid Subdomain") { StatusCode = StatusCodes.Status403Forbidden };
                            return;
                        }
                    }
                    catch
                    {
                        context.Result = new JsonResult("Invalid Subdomain") { StatusCode = StatusCodes.Status403Forbidden };
                        return;
                    }
                }
            }

            if (context.HttpContext.User is null)
                context.HttpContext.User = new ClaimsPrincipal();

            if (context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Api-Key"))
            && (context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Rocket-Id"))
            || context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Microsoft-Id"))))
            {
                string userId = string.Empty;

                if (context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Rocket-Id")))
                    userId = context.HttpContext.Request.Headers["X-Rocket-Id"].ToString();

                if (context.HttpContext.Request.Headers.Any(k => k.Key.Equals("X-Microsoft-Id")))
                    userId = context.HttpContext.Request.Headers["X-Microsoft-Id"].ToString();

                var client = await clientRepository.GetByApiKey(context.HttpContext.Request.Headers["X-Api-Key"].ToString());
                if (client is null)
                {
                    context.Result = new JsonResult("Invalid Api Key") { StatusCode = StatusCodes.Status400BadRequest };
                    return;
                }



                var user = await userManager.GetUser(userId, client.Id.ToString());
                if (user is null)
                {
                    context.Result = new JsonResult("Invlaid User Id") { StatusCode = StatusCodes.Status400BadRequest };
                    return;
                }

                var roles = await userManager.GetUserRolesAsync(user.Id);

                ((ClaimsIdentity)context.HttpContext.User.Identity).AddClaim(new Claim("cid", client.Id.ToString()));
                ((ClaimsIdentity)context.HttpContext.User.Identity).AddClaim(new Claim("sbd", client.Subdomain.ToString()));
                ((ClaimsIdentity)context.HttpContext.User.Identity).AddClaim(new Claim("sid", user.Id));
                ((ClaimsIdentity)context.HttpContext.User.Identity).AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", string.Join(',', roles)));
            }

        }
    }

    public class ApiKeyAuthorizeAttribute : TypeFilterAttribute, IAllowAnonymous
    {
        public ApiKeyAuthorizeAttribute() : base(typeof(ApiKeyAuthorizeFilter))
        {
        }
    }
}
