using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.AssignAreaUsers;

public class AssignClientAreaUsersCommandValidator : AbstractValidator<AssignClientAreaUsersCommand>
{
    public AssignClientAreaUsersCommandValidator()
    {
        RuleFor(r => r.AreaId).NotEmpty();
    }
}
