using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Coreapi.Api.Attributes
{
    public class AutoGenTokenFilter : IAsyncAuthorizationFilter
    {
        private readonly IUserManager userManager;
        private readonly ISignInManager sinInManager;

        public AutoGenTokenFilter(IUserManager userManager, ISignInManager sinInManager)
        {
            this.userManager = userManager;
            this.sinInManager = sinInManager;
        }
        public  async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {


          
            if (context.HttpContext.Request.Headers.Any(k => k.Key.Equals("x-refresh")))
            {
               
               
            
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                authHeader = authHeader.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                //context.Result = new JsonResult("request refresh") { StatusCode = StatusCodes.Status202Accepted };
                var xrefresh = context.HttpContext.Request.Headers["x-refresh"].ToString();
              
            
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                var userId = tokenS.Claims.First(claim => claim.Type == "sid").Value;
             
                var expDate = tokenS.ValidTo;
                if (expDate < DateTime.Now.AddMinutes(30))
                {
                    var token = await sinInManager.RefreshToken(userId, xrefresh, false);
                    // context.HttpContext.Response.Headers["x-access-token"] = token.Token;
                    // context.HttpContext.Response.Headers["x-refresh-token"] = token.RefreshToken;

                    context.HttpContext.Response.Headers.Add("x-access-token", token.Token);
                    context.HttpContext.Response.Headers.Add("x-refresh-token", token.RefreshToken);
         
                    context.HttpContext.Request.Headers.Add("Authorization", new[] { string.Format("Bearer {0}", token.Token) });
                }


            }
          
       

        }
    }
    public class AutoGenTokenAttribute:TypeFilterAttribute,IAllowAnonymous
    {
        public AutoGenTokenAttribute() : base(typeof(AutoGenTokenFilter))
        {
            
        }
    }
}
