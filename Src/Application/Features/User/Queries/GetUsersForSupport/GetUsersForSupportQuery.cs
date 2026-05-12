using MediatR;
using System.Collections.Generic;

namespace Coreapi.Application.Features.User.Queries.GetUsersForSupport
{
    public class GetUsersForSupportQuery : IRequest<List<UserDataDto>>
    {
    }
}
