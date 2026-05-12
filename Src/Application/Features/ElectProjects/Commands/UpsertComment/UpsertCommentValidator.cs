using Coreapi.Common.Enums;
using FluentValidation;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpsertComment;

public class UpsertCommentValidator:AbstractValidator<UpsertCommentCommand>
{
    public UpsertCommentValidator()
    {
        RuleFor(r => r.Ampere).NotEmpty().GreaterThan(0);
        RuleFor(r => r.Power).NotEmpty().GreaterThan(0)
            .When(r => r.BranchingTypeEnum is not (BranchingTypeEnum.Bt4 or BranchingTypeEnum.Bt5));
        RuleFor(r => r.PowerSum).NotEmpty().GreaterThan(0)
            .When(r => r.BranchingTypeEnum is not (BranchingTypeEnum.Bt4 or BranchingTypeEnum.Bt5));

    }
}