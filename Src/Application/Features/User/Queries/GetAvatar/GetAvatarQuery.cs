using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.User.Queries.GetAvatar
{
    public class GetAvatarQuery : IRequest<byte[]>
    {
    }
}
