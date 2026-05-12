using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.User.Queries.GetUserRoles
{
    public class GetUserRolesQuery : IRequest<List<string>>
    {
        public string UserId { get; set; }
    }
}
