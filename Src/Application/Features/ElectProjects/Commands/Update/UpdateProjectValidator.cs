using Coreapi.Application.Common.Interfaces;
using FluentValidation;

namespace Coreapi.Application.Features.ElectProjects.Commands.Update;

public class UpdateProjectValidator:AbstractValidator<UpdateProjectCommand>
{
    private readonly ICurrentUser currentUser;

    public UpdateProjectValidator(ICurrentUser currentUser)
    {
        this.currentUser = currentUser;
        ConfigureRules();
    }
    private void ConfigureRules()
    {
        if (currentUser.Role.Contains("Administrator"))
        {
            RuleFor(r => r.Area).GreaterThan(0).WithMessage("مساحت نمی تواند صفر باشد");
            RuleFor(r => r.NumberOfFloor).GreaterThan(0).WithMessage("طبقه باید بیشتر از صفر باشد");
        }
		RuleFor(r => r.Address)
        .NotEmpty().WithMessage("آدرس نمی‌تواند خالی باشد")
        .MinimumLength(10).WithMessage("آدرس نمی‌تواند کمتر از 10 کاراکتر باشد");
		// Add other validation rules that apply to all users
	}

}