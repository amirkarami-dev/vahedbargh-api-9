using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpsertArea;

public class UpsertClientAreaCommandValidator : AbstractValidator<UpsertClientAreaCommand>
{
    public UpsertClientAreaCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(200);
        RuleFor(r => r.Description).MaximumLength(200);

        RuleFor(r => r.Area).NotNull().NotEmpty();

        When(r => r.Type == Coreapi.Common.Enums.SegmentTypeEnum.Polygon, () =>
        {
            RuleFor(r => r.Area).Must(a => a.Count() > 2).WithMessage("Area Must Have At Least 3 Points");
        });

        When(r => r.Type == Coreapi.Common.Enums.SegmentTypeEnum.Circle, () =>
        {
            RuleFor(r => r.Radius).GreaterThan(0);
        });
    }
}
