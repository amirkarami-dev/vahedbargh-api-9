using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpsertCard
{
    public class UpsertClientCardCommandValidator : AbstractValidator<UpsertClientCardCommand>
    {
        public UpsertClientCardCommandValidator()
        {
            RuleFor(r => r.NameOnCard).NotEmpty().MaximumLength(50);
            RuleFor(r => r.CardNumber).NotEmpty().MaximumLength(32);
            RuleFor(r => r.CCV).NotEmpty().MaximumLength(10);
            RuleFor(r => r.ExpireDate).NotEmpty().MaximumLength(10);

        }
    }
}
