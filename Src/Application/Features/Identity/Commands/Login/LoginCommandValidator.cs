using FluentValidation;

namespace Coreapi.Application.Features.Identity.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {

        public LoginCommandValidator()
        {
            RuleFor(a => a.Password).Length(8, 50);
        }

    }
}
