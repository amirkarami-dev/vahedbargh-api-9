using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using FluentValidation;
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

    // ---------- Admin (Administrator) management ----------

    // Admin DTOs expose the real DB Id (the public ones use the Key as Id).
    public class AdminProcessStepDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Details { get; set; }
        public List<string> RequiredDocs { get; set; }
        public List<string> Tools { get; set; }
        public string Note { get; set; }
        public bool IsDecision { get; set; }
        public string DecisionYes { get; set; }
        public string DecisionNo { get; set; }

        private static List<string> Split(string s) =>
            string.IsNullOrWhiteSpace(s)
                ? new List<string>()
                : s.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        public static AdminProcessStepDto From(ProcessStep s) => new()
        {
            Title = s.Title, Description = s.Description, Note = s.Note, IsDecision = s.IsDecision,
            DecisionYes = s.DecisionYes, DecisionNo = s.DecisionNo,
            Details = Split(s.Details), RequiredDocs = Split(s.RequiredDocs), Tools = Split(s.Tools),
        };
    }

    public class AdminProcessFlowDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string GlowColor { get; set; }
        public string Icon { get; set; }
        public int SortOrder { get; set; }
        public List<AdminProcessStepDto> Steps { get; set; }

        public static AdminProcessFlowDto From(ProcessFlow f) => new()
        {
            Id = f.Id, Key = f.Key, Title = f.Title, Subtitle = f.Subtitle, Description = f.Description,
            Color = f.Color, GlowColor = f.GlowColor, Icon = f.Icon, SortOrder = f.SortOrder,
            Steps = (f.Steps ?? new List<ProcessStep>())
                .OrderBy(s => s.SortOrder).ThenBy(s => s.Number)
                .Select(AdminProcessStepDto.From).ToList(),
        };
    }

    public class GetProcessFlowsAdminQuery : IRequest<IEnumerable<AdminProcessFlowDto>> { }

    public class GetProcessFlowsAdminQueryHandler(ILandingRepository repo)
        : IRequestHandler<GetProcessFlowsAdminQuery, IEnumerable<AdminProcessFlowDto>>
    {
        public async Task<IEnumerable<AdminProcessFlowDto>> Handle(GetProcessFlowsAdminQuery r, CancellationToken ct) =>
            (await repo.GetProcessFlows()).Select(AdminProcessFlowDto.From);
    }

    public class ProcessStepInput
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Details { get; set; } = new();
        public List<string> RequiredDocs { get; set; } = new();
        public List<string> Tools { get; set; } = new();
        public string Note { get; set; }
        public bool IsDecision { get; set; }
        public string DecisionYes { get; set; }
        public string DecisionNo { get; set; }
    }

    public class UpsertProcessFlowCommand : IRequest<AdminProcessFlowDto>
    {
        public Guid? Id { get; set; } // null = create, set = update
        public string Key { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string GlowColor { get; set; }
        public string Icon { get; set; }
        public int SortOrder { get; set; }
        public List<ProcessStepInput> Steps { get; set; } = new();
    }

    public class DeleteProcessFlowCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class UpsertProcessFlowCommandValidator : AbstractValidator<UpsertProcessFlowCommand>
    {
        public UpsertProcessFlowCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Key).NotEmpty().MaximumLength(80)
                .Matches("^[a-zA-Z0-9-_]+$").WithMessage("کلید فقط شامل حروف انگلیسی، عدد و خط‌تیره باشد.");
        }
    }

    public class ProcessFlowCommandHandlers(ILandingRepository repo) :
        IRequestHandler<UpsertProcessFlowCommand, AdminProcessFlowDto>,
        IRequestHandler<DeleteProcessFlowCommand, bool>
    {
        private static string J(List<string> items) =>
            string.Join("\n", (items ?? new List<string>()).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()));

        // Steps are numbered by their order in the form (1-based).
        private static List<ProcessStep> BuildSteps(UpsertProcessFlowCommand r, Guid flowId) =>
            (r.Steps ?? new List<ProcessStepInput>())
                .Where(s => !string.IsNullOrWhiteSpace(s.Title))
                .Select((s, i) => new ProcessStep
                {
                    Id = Guid.NewGuid(), ProcessFlowId = flowId, Number = i + 1, SortOrder = i + 1,
                    Title = s.Title, Description = s.Description, Details = J(s.Details),
                    RequiredDocs = J(s.RequiredDocs), Tools = J(s.Tools), Note = s.Note,
                    IsDecision = s.IsDecision,
                    DecisionYes = s.IsDecision ? s.DecisionYes : null,
                    DecisionNo = s.IsDecision ? s.DecisionNo : null,
                }).ToList();

        public async Task<AdminProcessFlowDto> Handle(UpsertProcessFlowCommand r, CancellationToken ct)
        {
            if (r.Id is Guid id)
            {
                var updated = await repo.UpdateProcessFlow(new ProcessFlow
                {
                    Id = id, Key = r.Key, Title = r.Title, Subtitle = r.Subtitle, Description = r.Description,
                    Color = r.Color, GlowColor = r.GlowColor, Icon = r.Icon, SortOrder = r.SortOrder,
                    Steps = BuildSteps(r, id),
                });
                return updated is null ? null : AdminProcessFlowDto.From(updated);
            }

            var newId = Guid.NewGuid();
            var created = await repo.AddProcessFlow(new ProcessFlow
            {
                Id = newId, Key = r.Key, Title = r.Title, Subtitle = r.Subtitle, Description = r.Description,
                Color = r.Color, GlowColor = r.GlowColor, Icon = r.Icon, SortOrder = r.SortOrder,
                Steps = BuildSteps(r, newId),
            });
            return AdminProcessFlowDto.From(created);
        }

        public async Task<bool> Handle(DeleteProcessFlowCommand r, CancellationToken ct) =>
            await repo.DeleteProcessFlow(r.Id);
    }
}
