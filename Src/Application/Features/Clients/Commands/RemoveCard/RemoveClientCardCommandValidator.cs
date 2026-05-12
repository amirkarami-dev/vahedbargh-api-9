using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.RemoveCard
{
    public class RemoveClientCardCommandValidator : AbstractValidator<RemoveClientCardCommand>
    {
        public RemoveClientCardCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
        }
    }
}
