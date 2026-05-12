using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.SyncMicrosoftUsers
{
    public class SyncMicrosoftUsersCommandValidator : AbstractValidator<SyncMicrosoftUsersCommand>
    {
        public SyncMicrosoftUsersCommandValidator()
        {
            RuleFor(r => r.Token).NotEmpty();
        }
    }
}
