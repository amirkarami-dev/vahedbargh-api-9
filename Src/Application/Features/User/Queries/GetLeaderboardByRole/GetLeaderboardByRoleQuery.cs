using MediatR;
using System.Collections.Generic;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Features.User.Queries.GetLeaderboardByRole
{
    public class GetLeaderboardByRoleQuery : IRequest<List<UserData>>
    {
        public string RoleName { get; set; }
    }
}
