using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Infrastructure.Identity.Models;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using AuthenticationException = Coreapi.Application.Common.Exceptions.AuthenticationException;
using Coreapi.Infrastructure.Identity.Services;

namespace Coreapi.Infrastructure.Identity
{
    public class SignInManagerService : ISignInManager
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IClientRepository repository;
        private readonly ISmsService smsService;
       // private readonly IConnectionMultiplexer _redis;
        // private readonly IDatabase _database;

        public SignInManagerService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration
            , IClientRepository repository, ISmsService smsService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.repository = repository;
            this.smsService = smsService;
            // _redis = redis;
            // _database = _redis.GetDatabase();
        }

        public async Task<LoginOutput> Login(string userName, string password, string deviceToken, PlatformTypeEnum platformType)
        {
            var user = await userManager.FindByNameAsync(userName);
          //  await userManager.SetTwoFactorEnabledAsync(user, true);
            if (user is not null && user.Active)
            {
                await signInManager.SignOutAsync();
                // var result = await signInManager.PasswordSignInAsync(user, password, false, true);
                
                var result = await signInManager.PasswordSignInAsync(user, password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    
                    string subdomain = string.Empty;
                    bool havePackage = false;
                    if (!string.IsNullOrEmpty(user.ClientId))
                    {
                        var client = await repository.GetWithPackages(Guid.Parse(user.ClientId));
                      
                        subdomain = client.Subdomain;
                    }

                    var refreshToken = Guid.NewGuid().ToString();
                    await userManager.SetAuthenticationTokenAsync(user, "Identity", string.Format("{0}{1}", user.Id, refreshToken), refreshToken);

                    if (!string.IsNullOrEmpty(deviceToken))
                    {
                        await AddDeviceToken(user.Id, deviceToken, platformType);
                   

                    }
                    //  await AddSessionToken(user.Id, refreshToken);
                    
                    return new LoginOutput()
                    {
                        Token = await GenerateJwtTokenAsync(user, havePackage, subdomain),
                        RefreshToken = refreshToken,
                        UrlRedirect = "/dashboard"
                    };
                }

                if (result.RequiresTwoFactor)
                {

                        if (user.PhoneNumber != null)
                        {
                       
                            var code = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
                            var succeeded = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, code);


                            if (succeeded)
                            {
                                await smsService.SendSmsCodeMfa(user.PhoneNumber, 9, "واحد گاز", code);
                            }
                        }

                        return new LoginOutput()
                        {
                            Token = null,
                            RefreshToken = null,
                            UrlRedirect = "/submit-sms"
                        };
                }
                else
                {
                    string[] errors = { "Wrong Username or Password" };
                    throw new AuthenticationException(Errors: errors);
                }
            }
            else
            {
                string[] errors = { "Wrong Username or Password" };
                throw new AuthenticationException(Errors: errors);
            }
        }

        public async Task<LoginOutput> Login(string mobileCode, string userName)
        {
    
            var code = mobileCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            // var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            var user = await userManager.FindByNameAsync(userName);
            if (user is not null && user.Active)
            {

               
               // var result = await signInManager.TwoFactorSignInAsync(TokenOptions.DefaultPhoneProvider, code, false, false);
               var result = await smsService.SmsOtpVerify(user.PhoneNumber, code);
               
                if (result.Status == "success")
                {

                    string subdomain = string.Empty;
                    bool havePackage = false;
                    await signInManager.SignInAsync(user, true, null);
                    var refreshToken = Guid.NewGuid().ToString();
                    await userManager.SetAuthenticationTokenAsync(user, "Identity", string.Format("{0}{1}", user.Id, refreshToken), refreshToken);

                    if (!string.IsNullOrEmpty(user.ClientId))
                    {
                        var client = await repository.GetWithPackages(Guid.Parse(user.ClientId));
                        subdomain = client.Subdomain;
                    }
                    return new LoginOutput()
                    {
                        Token = await GenerateJwtTokenAsync(user, havePackage, subdomain),
                        RefreshToken = refreshToken,
                        UrlRedirect = "/dashboard"
                    };
                }
                else
                {
                    string[] errors = { $"رمز ورود اشتباه می باشد دو باره ورود را بزنید" };
                    throw new AuthenticationException(Errors: errors);
                }
            }

            else
            {
                string[] errors = { "این کاربری وجود ندارد" };
                throw new AuthenticationException(Errors: errors);
            }

        }

        public async Task<LoginOutput> OneMinuteLogin(string userName, string password, string deviceToken, PlatformTypeEnum platformType)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user is not null && user.Active)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, password, true);
                if (result.Succeeded)
                {

                    var refreshToken = Guid.NewGuid().ToString();
                    await userManager.SetAuthenticationTokenAsync(user, "Identity", string.Format("{0}{1}", user.Id, refreshToken), refreshToken);
                    
                    if (!string.IsNullOrEmpty(deviceToken))
                        await AddDeviceToken(user.Id, deviceToken, platformType);

                    return new LoginOutput()
                    {
                        Token = await GenerateJwtTokenOneMinAsync(user, true),
                        RefreshToken = refreshToken
                    };
                }
                else
                {
                    string[] errors = { "Wrong Username or Password" };
                    throw new AuthenticationException(Errors: errors);
                }
            }
            else
            {
                string[] errors = { "Wrong Username or Password" };
                throw new AuthenticationException(Errors: errors);
            }
        }


        public async Task<LoginOutput> Login(string userName, string password, string mac)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user is not null && user.Active)
            {
                if(string.IsNullOrEmpty(user.MacAddress))
                {
                    user.MacAddress = mac;
                    await userManager.UpdateAsync(user);
                }
                if (user.MacAddress.Equals(mac))
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, password, true);
                    if (result.Succeeded)
                    {
                        
                        string subdomain = string.Empty;
                        if (!string.IsNullOrEmpty(user.ClientId))
                        {
                            var client = await repository.GetWithPackages(Guid.Parse(user.ClientId));
                        }

                        var refreshToken = Guid.NewGuid().ToString();
                        await userManager.SetAuthenticationTokenAsync(user, "Identity", string.Format("{0}{1}", user.Id, refreshToken), refreshToken);

                        return new LoginOutput()
                        {
                            Token = await GenerateJwtTokenAsync(user, true, subdomain),
                            RefreshToken = refreshToken
                        };
                    }
                }
            }

            string[] errors = { "Wrong Username or Password" };
            throw new AuthenticationException(Errors: errors);
        }


        public async Task<LoginOutput> RefreshToken(string UserId, string RefreshToken, bool oneMinute = false)
        {
            var user = await userManager.FindByIdAsync(UserId);

            var token = await userManager.GetAuthenticationTokenAsync(user, "Identity", string.Format("{0}{1}",user?.Id, RefreshToken));

            if (token != RefreshToken || !user.Active)
                throw new UnauthorizedAccessException();

            await userManager.RemoveAuthenticationTokenAsync(user, "Identity", string.Format("{0}{1}", user.Id, RefreshToken));
            string subdomian = string.Empty;
            if (!string.IsNullOrEmpty(user.ClientId))
            {
                var client = await repository.GetWithPackages(Guid.Parse(user.ClientId));
                subdomian = client.Subdomain;
            }

            var refreshToken = Guid.NewGuid().ToString();
            await userManager.SetAuthenticationTokenAsync(user, "Identity", string.Format("{0}{1}", user.Id, refreshToken), refreshToken);
            return new LoginOutput()
            {
                Token = !oneMinute ? await GenerateJwtTokenAsync(user, true, subdomian) : await GenerateJwtTokenOneMinAsync(user, true),
                RefreshToken = refreshToken
            };


        }
       

        public async Task<LoginOutput> GoogleAuthenticate(string tokenId)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(tokenId, new GoogleJsonWebSignature.ValidationSettings());
            var user = await userManager.FindByNameAsync(payload.Email);
            if(user is null)
            {
                user = new ApplicationUser
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    UserName = payload.Email
                };

                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    string[] errors = { "Invalid Google Authentication" };
                    throw new AuthenticationException(Errors: errors);
                }

                await userManager.AddToRolesAsync(user, new string[] { "Employee" });

            }

            if(!user.Active)
                throw new UnauthorizedAccessException();

            var refreshToken = Guid.NewGuid().ToString();
            await userManager.SetAuthenticationTokenAsync(user, "Identity", string.Format("{0}{1}", user.Id, refreshToken), refreshToken);
            return new LoginOutput()
            {
                Token = await GenerateJwtTokenAsync(user,false, string.Empty),
                RefreshToken = refreshToken
            };

        }


        public async Task<LoginOutput> MicrosoftAuthenticate(string tokenId)
        {
            try
            {
                // Use CustomTokenCredential for authentication
                var customTokenCredential = new CustomTokenCredential(tokenId);
                var graphClient = new GraphServiceClient(customTokenCredential);

                // Retrieve user information
                var payload = await graphClient.Me.GetAsync(); // Corrected API call

                var user = await userManager.FindByNameAsync(payload.Mail);

                if (user is null || !user.Active)
                    throw new UnauthorizedAccessException();

                var client = await repository.GetWithPackages(Guid.Parse(user.ClientId));

                var refreshToken = Guid.NewGuid().ToString();
                await userManager.SetAuthenticationTokenAsync(user, "Identity", $"{user.Id}{refreshToken}", refreshToken);

                return new LoginOutput
                {
                    Token = await GenerateJwtTokenAsync(user,true , client.Subdomain),
                    RefreshToken = refreshToken
                };
            }
            catch
            {
                throw new UnauthorizedAccessException();
            }
        }
        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user, bool havePacakge, string subdomain)
        {
            var tokenmange = configuration.GetSection("tokenManagement").Get<TokenManagement>();
            var claims = new List<Claim>()
            {
              new Claim(JwtRegisteredClaimNames.Sid,user.Id),          
              new Claim(JwtRegisteredClaimNames.Iss,tokenmange.Issuer),
              new Claim(JwtRegisteredClaimNames.Aud,tokenmange.Audience),
              new Claim(JwtRegisteredClaimNames.Name, string.Format("{0} {1}", user.FirstName, user.LastName))
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            if (string.IsNullOrEmpty(subdomain) && roles.Any(r => r.Equals("SuperUser") || r.Equals("Analyzer")))
                subdomain = "admin";

            if (roles.Any(r => r.Equals("SuperUser") || r.Equals("Analyzer")))
                claims.Add(new Claim("pkg", "true"));
            else
                claims.Add(new Claim("pkg", havePacakge.ToString()));

            if (!string.IsNullOrEmpty(user.ClientId))
                claims.Add(new Claim("cid", user.ClientId));

            if (!string.IsNullOrEmpty(subdomain))
                claims.Add(new Claim("sbd", subdomain));

            if (!string.IsNullOrEmpty(user.Avatar))
                claims.Add(new Claim("avatar", user.Avatar));
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenmange.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(120),
             
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
         
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GenerateJwtTokenOneMinAsync(ApplicationUser user, bool havePacakge)
        {
            var tokenmange = configuration.GetSection("tokenManagement").Get<TokenManagement>();
            var claims = new List<Claim>()
            {
              new Claim(JwtRegisteredClaimNames.Sid,user.Id),          
              new Claim(JwtRegisteredClaimNames.Iss,tokenmange.Issuer),
              new Claim(JwtRegisteredClaimNames.Aud,tokenmange.Audience),
              new Claim("pkg", havePacakge.ToString()),
              new Claim(JwtRegisteredClaimNames.GivenName, string.Format("{0} {1}", user.FirstName, user.LastName))
            };

            if (!string.IsNullOrEmpty(user.ClientId))
                claims.Add(new Claim("cid", user.ClientId));

            if (!string.IsNullOrEmpty(user.Avatar))
                claims.Add(new Claim("avatar", user.Avatar));

            var roles = await userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenmange.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(120),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task AddDeviceToken(string userId,string deviceToken, PlatformTypeEnum platformType)
        {
            //SortedSetEntry? token = await _database.SortedSetScanAsync(userId + ":DeviceTokens")
            //    .FirstOrDefaultAsync(i => JsonConvert.DeserializeObject<UserToken>(i.Element).Token.Equals(deviceToken));

            //if(token is null || !token.HasValue || token.Value.Element.IsNullOrEmpty)
            //    await _database.SortedSetAddAsync(userId + ":DeviceTokens", JsonConvert.SerializeObject(new UserToken { Token = deviceToken, Platform = platformType}), DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }

        private async Task AddSessionToken(string userId,string refreshToken)
        {
            var sessionToken = new SessionToken
            {

                Valid = true,
                RefreshToken = refreshToken,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };


           //await _database.HashSetAsync(userId + ":Session", sessionToken.AsHashEntries().ToArray());
           //var dd = await _database.HashGetAsync(userId + ":Session", "RefreshToken");
           


        }
        public async Task RemoveSessionToken(string userId)
        {
            var sessionToken = new SessionToken
            {

                Valid = false,
                RefreshToken = null,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };

            //var hasHash = await _database.HashGetAsync(userId + ":Session", "RefreshToken");
            //if (hasHash.HasValue)
            //    await _database.HashSetAsync(userId + ":Session", sessionToken.AsHashEntries().ToArray());
            



        }
    }
}
