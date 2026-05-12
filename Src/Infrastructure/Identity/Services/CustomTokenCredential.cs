using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Coreapi.Infrastructure.Identity.Services;

public class CustomTokenCredential : TokenCredential
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