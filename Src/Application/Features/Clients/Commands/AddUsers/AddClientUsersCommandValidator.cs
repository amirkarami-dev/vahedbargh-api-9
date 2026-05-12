using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.AddUsers
{
    public class AddClientUsersCommandValidator : AbstractValidator<AddClientUsersCommand>
    {
        public AddClientUsersCommandValidator()
        {
            RuleFor(r => r.Users).NotEmpty();
            RuleForEach(r => r.Users).ChildRules(b =>
            {
                b.RuleFor(r => r.Id).NotEmpty();
                b.RuleFor(r => r.Email).NotEmpty().EmailAddress().MaximumLength(100);
                b.RuleFor(r => r.FirstName).NotEmpty().MaximumLength(100);
                b.RuleFor(r => r.LastName).MaximumLength(100);
            });
        }
    }
}
