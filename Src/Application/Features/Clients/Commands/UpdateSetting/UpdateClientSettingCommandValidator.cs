using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpdateSetting
{
    public class UpdateClientSettingCommandValidator : AbstractValidator<UpdateClientSettingCommand>
    {
        public UpdateClientSettingCommandValidator()
        {
            RuleFor(r => r.InMessage).NotEmpty().MaximumLength(500);
            RuleFor(r => r.OutMessage).NotEmpty().MaximumLength(500);
            RuleFor(r => r.BreakInMessage).NotEmpty().MaximumLength(500);
            RuleFor(r => r.BreakOutMessage).NotEmpty().MaximumLength(500);

            RuleFor(r => r.AllowedBreak).GreaterThan(0);
            RuleFor(r => r.AllowedOvertime).GreaterThan(0);
            RuleFor(r => r.AllowedWorkHour).GreaterThan(r => r.AllowedBreak);
            RuleFor(r => r.AutoOutCycle).ExclusiveBetween(0, 24);
        }
    }
}
