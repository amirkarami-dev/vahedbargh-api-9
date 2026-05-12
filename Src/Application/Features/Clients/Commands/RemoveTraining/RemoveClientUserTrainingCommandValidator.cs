using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.RemoveTraining
{
    public class RemoveClientUserTrainingCommandValidator : AbstractValidator<RemoveClientUserTrainingCommand>
    {
        public RemoveClientUserTrainingCommandValidator()
        {
            RuleFor(r => r.ClientId).NotEmpty();
            RuleFor(r => r.TrainingId).NotEmpty();
        }
    }
}
