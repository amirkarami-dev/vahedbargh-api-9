using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.GenerateInvoice
{
    public class GenerateClientPackageInvoiceCommandValidator : AbstractValidator<GenerateClientPackageInvoiceCommand>
    {
        public GenerateClientPackageInvoiceCommandValidator()
        {
            RuleFor(r => r.CardId).NotEmpty();
        }
    }
}
