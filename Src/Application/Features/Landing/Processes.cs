using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using MediatR;

namespace Coreapi.Application.Features.Landing.Processes
{
    public class ProcessDecisionDto
    {
        public string Yes { get; set; }
        public string No { get; set; }
    }

    public class ProcessStepDto
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Details { get; set; }
        public List<string> RequiredDocs { get; set; }
        public List<string> Tools { get; set; }
        public string Note { get; set; }
        public bool IsDecision { get; set; }
        public ProcessDecisionDto Decision { get; set; }

        private static List<string> Split(string s) =>
            string.IsNullOrWhiteSpace(s)
                ? new List<string>()
                : s.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        public static ProcessStepDto From(ProcessStep s) => new()
        {
            Id = s.Id.ToString(),
            Number = s.Number,
            Title = s.Title,
            Description = s.Description,
            Details = Split(s.Details),
            RequiredDocs = Split(s.RequiredDocs),
            Tools = Split(s.Tools),
            Note = s.Note,
            IsDecision = s.IsDecision,
            Decision = s.IsDecision ? new ProcessDecisionDto { Yes = s.DecisionYes, No = s.DecisionNo } : null,
        };
    }

    public class ProcessFlowDto
    {
        public string Id { get; set; } // the stable Key ("inspection" ...)
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string GlowColor { get; set; }
        public string Icon { get; set; }
        public List<ProcessStepDto> Steps { get; set; }

        public static ProcessFlowDto From(ProcessFlow f) => new()
        {
            Id = f.Key,
            Title = f.Title,
            Subtitle = f.Subtitle,
            Description = f.Description,
            Color = f.Color,
            GlowColor = f.GlowColor,
            Icon = f.Icon,
            Steps = (f.Steps ?? new List<ProcessStep>())
                .OrderBy(s => s.SortOrder).ThenBy(s => s.Number)
                .Select(ProcessStepDto.From).ToList(),
        };
    }

    public class GetProcessFlowsQuery : IRequest<IEnumerable<ProcessFlowDto>> { }

    public class GetProcessFlowsQueryHandler(ILandingRepository repo)
        : IRequestHandler<GetProcessFlowsQuery, IEnumerable<ProcessFlowDto>>
    {
        public async Task<IEnumerable<ProcessFlowDto>> Handle(GetProcessFlowsQuery r, CancellationToken ct) =>
            (await repo.GetProcessFlows()).Select(ProcessFlowDto.From);
    }
}
