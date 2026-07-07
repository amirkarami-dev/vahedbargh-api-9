using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using FluentValidation;
using MediatR;

namespace Coreapi.Application.Features.Landing.Stats
{
    public class StatItemDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public int Value { get; set; }
        public string Suffix { get; set; }
        public string IconName { get; set; }
        public int SortOrder { get; set; }
    }

    public class GetStatsQuery : IRequest<IEnumerable<StatItemDto>> { }

    public class GetStatsQueryHandler(ILandingRepository repo) : IRequestHandler<GetStatsQuery, IEnumerable<StatItemDto>>
    {
        public async Task<IEnumerable<StatItemDto>> Handle(GetStatsQuery r, CancellationToken ct) =>
            (await repo.GetStats()).Select(StatMap.From);
    }

    internal static class StatMap
    {
        public static StatItemDto From(StatItem s) => new()
        {
            Id = s.Id, Label = s.Label, Value = s.Value, Suffix = s.Suffix,
            IconName = s.IconName, SortOrder = s.SortOrder,
        };
    }

    // ---------- Admin write commands ----------

    public class UpsertStatCommand : IRequest<StatItemDto>
    {
        public Guid? Id { get; set; } // null = create, set = update
        public string Label { get; set; }
        public int Value { get; set; }
        public string Suffix { get; set; }
        public string IconName { get; set; }
        public int SortOrder { get; set; }
    }

    public class DeleteStatCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class UpsertStatCommandValidator : AbstractValidator<UpsertStatCommand>
    {
        public UpsertStatCommandValidator()
        {
            RuleFor(x => x.Label).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Value).GreaterThanOrEqualTo(0);
        }
    }

    public class StatCommandHandlers(ILandingRepository repo) :
        IRequestHandler<UpsertStatCommand, StatItemDto>,
        IRequestHandler<DeleteStatCommand, bool>
    {
        public async Task<StatItemDto> Handle(UpsertStatCommand r, CancellationToken ct)
        {
            if (r.Id is Guid id)
            {
                var updated = await repo.UpdateStat(new StatItem
                {
                    Id = id, Label = r.Label, Value = r.Value, Suffix = r.Suffix,
                    IconName = r.IconName, SortOrder = r.SortOrder,
                });
                return updated is null ? null : StatMap.From(updated);
            }

            var created = await repo.AddStat(new StatItem
            {
                Id = Guid.NewGuid(), Label = r.Label, Value = r.Value, Suffix = r.Suffix,
                IconName = r.IconName, SortOrder = r.SortOrder,
            });
            return StatMap.From(created);
        }

        public async Task<bool> Handle(DeleteStatCommand r, CancellationToken ct) =>
            await repo.DeleteStat(r.Id);
    }
}
