using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.Identity.Commands.LoginByMicrosoft
{
    public class LoginByMicrosoftCommandValidator : AbstractValidator<LoginByMicrosoftCommand>
    {
        public LoginByMicrosoftCommandValidator()
        {
            RuleFor(b => b.TokenId).NotEmpty();
        }
    }
}
