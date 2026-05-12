using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpdateUser
{
    public class UpdateClientUserCommand : IRequest
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public bool Active { get; set; }
       // public IFormFile Avatar { get; set; }

       // public List<string> Roles { get; set; } = new ();
        public string Role { get; set; }

        public string PhoneNumber { get; set; }
        //public string Linkedin { get; set; }
        //public string Facebook { get; set; }
        //public string Skype { get; set; }
        //public string Instagram { get; set; }
    }
}
