using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.AddUsers
{
    public class AddClientUsersCommand : IRequest<UsersOutput>
    {
        public IEnumerable<UserInput> Users { get; set; }
    }

    public class UserInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
    }
}
