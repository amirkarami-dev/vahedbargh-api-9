using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
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
            (await repo.GetStats()).Select(s => new StatItemDto
            {
                Id = s.Id, Label = s.Label, Value = s.Value, Suffix = s.Suffix,
                IconName = s.IconName, SortOrder = s.SortOrder,
            });
    }
}
