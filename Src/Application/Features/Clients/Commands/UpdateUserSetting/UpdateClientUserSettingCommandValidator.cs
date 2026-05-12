using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpdateUserSetting
{
    public class UpdateClientUserSettingCommandValidator : AbstractValidator<UpdateClientUserSettingCommand>
    {
        public UpdateClientUserSettingCommandValidator()
        {
            RuleFor(r => r.UserId).Length(36);
        }
    }
}
