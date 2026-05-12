using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpdateUser
{
    public class UpdateClientUserCommandValidator : AbstractValidator<UpdateClientUserCommand>
    {
        public UpdateClientUserCommandValidator()
        {
            RuleFor(r => r.Id).Length(36);
            RuleFor(r => r.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(r => r.LastName).MaximumLength(50);

        }
    }
}
