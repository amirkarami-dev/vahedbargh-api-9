using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpsertTraining
{
    public class UpsertClientUserTrainingCommandValidator : AbstractValidator<UpsertClientUserTrainingCommand>
    {
        public UpsertClientUserTrainingCommandValidator()
        {
            RuleFor(r => r.ClientId).NotEmpty();
            RuleFor(r => r.UserId).NotEmpty().MaximumLength(40);
            RuleFor(r => r.Title).NotEmpty().MaximumLength(100);
            RuleFor(r => r.Link).NotEmpty();
        }
    }
}
