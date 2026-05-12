using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.RemoveAnalyzer
{
    public class RemoveClientAnalyzerCommandValidator : AbstractValidator<RemoveClientAnalyzerCommand>
    {
        public RemoveClientAnalyzerCommandValidator()
        {
            RuleFor(r => r.UserId).NotEmpty().MaximumLength(40);
            RuleFor(r => r.ClientId).NotEmpty();
        }
    }
}
