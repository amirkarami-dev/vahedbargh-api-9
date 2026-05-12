using MediatR;

namespace Coreapi.Application.Features.User.Queries.GetUserInfo
{
    public class GetUserInfoQuery : IRequest<UserInfoDto>
    {
    }
}
