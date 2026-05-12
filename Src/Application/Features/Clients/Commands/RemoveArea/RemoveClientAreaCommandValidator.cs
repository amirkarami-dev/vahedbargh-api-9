using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.RemoveArea;

public class RemoveClientAreaCommandValidator : AbstractValidator<RemoveClientAreaCommand>
{
    public RemoveClientAreaCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}
