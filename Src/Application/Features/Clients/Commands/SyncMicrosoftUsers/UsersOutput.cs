using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.SyncMicrosoftUsers
{
    public class UsersOutput
    {
        public List<UserOutput> SyncedUsers { get; set; }
        public List<UserOutput> NotSyncedUsers { get; set; }
    }

    public class UserOutput
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
