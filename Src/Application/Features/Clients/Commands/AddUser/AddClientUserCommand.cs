using MediatR;

namespace Coreapi.Application.Features.Clients.Commands.AddUser
{
    public class AddClientUserCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool SendEmail { get; set; } = false;
        public string PhoneNumber { get; set; }
        public string NickName { get; set; }
    }
}
