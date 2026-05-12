using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using Azure.Core;
using System.Threading;

namespace Coreapi.Infrastructure
{
    public class MicrosoftService : IMicrosoftService
    {
        private class CustomTokenCredential : TokenCredential
        {
            private readonly string _token;

            public CustomTokenCredential(string token)
            {
                _token = token;
            }

            public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
            {
                return new AccessToken(_token, DateTimeOffset.UtcNow.AddHours(1));
            }

            public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new AccessToken(_token, DateTimeOffset.UtcNow.AddHours(1)));
            }
        }

        public async Task<IEnumerable<MicrosoftUserModel>> GetUsers(string token)
        {
            // Use CustomTokenCredential for authentication
            var tokenCredential = new CustomTokenCredential(token);
            var graphClient = new GraphServiceClient(tokenCredential);

            // Retrieve users
            var users = await graphClient.Users.GetAsync();

            // Map results to MicrosoftUserModel
            return users.Value.Select(u => new MicrosoftUserModel
            {
                Id = u.Id,
                Email = u.Mail,
                Firstname = u.GivenName,
                Lastname = u.Surname
            });
        }
    }
}
