using FluentValidation;

namespace Coreapi.Application.Features.Clients.Commands.SignupClient
{
    public class SignupClientCommandValidator : AbstractValidator<SignupClientCommand>
    {
        public SignupClientCommandValidator()
        {
            RuleFor(r => r.PackageId).NotEmpty();
            RuleFor(a => a.Email).EmailAddress().NotEmpty().MaximumLength(50);
            RuleFor(a => a.Password).Length(8, 50).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])")
                .WithMessage("Password Is Not Strong Enough");
            RuleFor(a => a.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.LastName).NotEmpty().MaximumLength(50);
            RuleFor(r => r.NameOnCard).NotEmpty().MaximumLength(50);
            RuleFor(r => r.CardNumber).NotEmpty().MaximumLength(32);
            RuleFor(r => r.CCV).NotEmpty().MaximumLength(10);
            RuleFor(r => r.ExpireDate).NotEmpty().MaximumLength(10);
            RuleFor(r => r.LicenseCount).GreaterThan(0);

            RuleFor(p => p.CompanyName)
            .NotEmpty()
            .MaximumLength(200);
        }
    }
}
