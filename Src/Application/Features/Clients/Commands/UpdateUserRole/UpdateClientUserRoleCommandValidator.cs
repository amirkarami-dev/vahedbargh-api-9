using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpdateUserRole
{
    public class UpdateClientUserRoleCommandValidator : AbstractValidator<UpdateClientUserRoleCommand>
    {
        public UpdateClientUserRoleCommandValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.Roles).NotEmpty();
        }
    }
}
