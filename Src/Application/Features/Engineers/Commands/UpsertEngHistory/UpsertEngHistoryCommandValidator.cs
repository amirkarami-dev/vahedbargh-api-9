using Coreapi.Common.Enums;
using FluentValidation;

namespace Coreapi.Application.Features.Engineers.Commands.UpsertEngHistory;

public class UpsertEngHistoryCommandValidator:AbstractValidator<UpsertEngHistoryCommand>
{
    public UpsertEngHistoryCommandValidator()
    {
        RuleFor(r => r.SolarValidityDate).NotEmpty();
        RuleFor(r => r.SolarIssueDate).NotEmpty();
        RuleFor(r => r.WorkPermitNum).NotEmpty();

    }
}