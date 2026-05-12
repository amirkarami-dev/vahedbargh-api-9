#nullable enable
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Threading.Tasks;
using Coreapi.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Coreapi.Api.Controllers
{

    public class MetaBaseController(IOptions<MetaBaseSettings> metaBaseSettings) : BaseController
    {
        
        private readonly MetaBaseSettings _metaBaseSettings = metaBaseSettings.Value;


        [HttpGet]
        public async Task<IActionResult> GetDashboardToken([FromQuery] int dashboardId)
        {
            // Build the JWT payload manually using dictionary
            var payload = new Dictionary<string, object?>
            {
                ["resource"] = new Dictionary<string, int> { ["dashboard"] = dashboardId },
                ["params"] = new Dictionary<string, object?>(),
                ["exp"] = DateTimeOffset.Now.ToUnixTimeSeconds() + (10 * 60) // 10 minutes
            };

            // Setup signing credentials
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_metaBaseSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create JWT header and payload
            var header = new JwtHeader(credentials);
            var jwtPayload = new JwtPayload(null, null, null, null, null);

            foreach (var kvp in payload)
            {
                jwtPayload.Add(kvp.Key, kvp.Value);
            }

            var token = new JwtSecurityToken(header, jwtPayload);
            var handler = new JwtSecurityTokenHandler();
            var encodedToken = handler.WriteToken(token);

            var iframeUrl = $"{_metaBaseSettings.SiteUrl}/embed/dashboard/{encodedToken}#bordered=true&titled=false&hide_parameters=footer";

            return Ok(new { iframeUrl });
        }

    }
}