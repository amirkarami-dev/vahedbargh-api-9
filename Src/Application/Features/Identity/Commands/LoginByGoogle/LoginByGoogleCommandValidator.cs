using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.Identity.Commands.LoginByGoogle
{
    public class LoginByGoogleCommandValidator : AbstractValidator<LoginByGoogleCommand>
    {
        public LoginByGoogleCommandValidator()
        {
            RuleFor(b => b.TokenId).NotEmpty();
        }
    }
}
