using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.AddAnalyzer
{
    public class AddClientAnalyzerCommandValidator : AbstractValidator<AddClientAnalyzerCommand>
    {
        public AddClientAnalyzerCommandValidator()
        {
            RuleFor(r => r.UserId).NotEmpty().MaximumLength(40);
            RuleFor(r => r.Clients).NotEmpty();
        }
    }
}
