using MediatR;
using System.Collections.Generic;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Features.User.Queries.GetUserListByRole
{
    public class GetUserListByRoleQuery:IRequest<List<UserData>>
    {
        public string RoleName { get; set; }
    }
}
