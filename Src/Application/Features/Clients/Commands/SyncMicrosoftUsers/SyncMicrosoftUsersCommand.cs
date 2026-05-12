using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.SyncMicrosoftUsers
{
    public class SyncMicrosoftUsersCommand : IRequest<UsersOutput>
    {
        public string Token { get; set; }
    }
}
