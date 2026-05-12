using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.AddUser
{
    public class AddClientUserCommandValidator : AbstractValidator<AddClientUserCommand>
    {
        public AddClientUserCommandValidator()
        {
            RuleFor(a => a.Email).EmailAddress().NotEmpty().MaximumLength(50);
            RuleFor(a => a.Password).Length(8, 50).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])")
                .WithMessage("Password Is Not Strong Enough");
            RuleFor(a => a.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.LastName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.Role).NotEmpty().MaximumLength(50);
        }
    }
}
