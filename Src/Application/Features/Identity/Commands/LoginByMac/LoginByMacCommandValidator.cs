using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.Identity.Commands.LoginByMac
{
    public class LoginByMacCommandValidator : AbstractValidator<LoginByMacCommand>
    {
        public LoginByMacCommandValidator()
        {
            RuleFor(a => a.UserName).EmailAddress().NotEmpty().MaximumLength(50);
            RuleFor(a => a.Password).Length(8, 50);
            RuleFor(a => a.MacAddress).NotEmpty();
        }
    }
}
