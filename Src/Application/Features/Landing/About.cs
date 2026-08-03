using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using FluentValidation;
using MediatR;

namespace Coreapi.Application.Features.Landing.About
{
    // The /about page is one row, so — unlike Processes — the public and admin shapes are the
    // same DTO. Children arrive already ordered from the repository; SortOrder is an
    // implementation detail and is never exposed.

    public class AboutMissionDto
    {
        public string IconName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public static AboutMissionDto From(AboutMission m) => new()
        {
            IconName = m.IconName, Title = m.Title, Description = m.Description,
        };
    }

    public class AboutOrgNodeDto
    {
        public int Level { get; set; }
        public string Title { get; set; }

        public static AboutOrgNodeDto From(AboutOrgNode n) => new()
        {
            Level = n.Level, Title = n.Title,
        };
    }

    public class AboutBoardMemberDto
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }

        public static AboutBoardMemberDto From(AboutBoardMember m) => new()
        {
            Name = m.Name, Role = m.Role, Description = m.Description,
        };
    }

    public class AboutContentDto
    {
        public Guid Id { get; set; }
        public string PageTitle { get; set; }
        public string Intro { get; set; }
        public string MissionsTitle { get; set; }
        public string OrgChartTitle { get; set; }
        public string BoardTitle { get; set; }
        public string DutiesTitle { get; set; }
        public List<AboutMissionDto> Missions { get; set; }
        public List<AboutOrgNodeDto> OrgNodes { get; set; }
        public List<AboutBoardMemberDto> BoardMembers { get; set; }
        public List<string> Duties { get; set; }

        public static AboutContentDto From(AboutContent a) => new()
        {
            Id = a.Id,
            PageTitle = a.PageTitle,
            Intro = a.Intro,
            MissionsTitle = a.MissionsTitle,
            OrgChartTitle = a.OrgChartTitle,
            BoardTitle = a.BoardTitle,
            DutiesTitle = a.DutiesTitle,
            Missions = (a.Missions ?? new List<AboutMission>())
                .OrderBy(m => m.SortOrder).Select(AboutMissionDto.From).ToList(),
            OrgNodes = (a.OrgNodes ?? new List<AboutOrgNode>())
                .OrderBy(n => n.Level).ThenBy(n => n.SortOrder).Select(AboutOrgNodeDto.From).ToList(),
            BoardMembers = (a.BoardMembers ?? new List<AboutBoardMember>())
                .OrderBy(m => m.SortOrder).Select(AboutBoardMemberDto.From).ToList(),
            Duties = (a.Duties ?? new List<AboutDuty>())
                .OrderBy(d => d.SortOrder).Select(d => d.Text).ToList(),
        };
    }

    public class GetAboutQuery : IRequest<AboutContentDto> { }

    public class GetAboutQueryHandler(ILandingRepository repo) : IRequestHandler<GetAboutQuery, AboutContentDto>
    {
        public async Task<AboutContentDto> Handle(GetAboutQuery r, CancellationToken ct)
        {
            var content = await repo.GetAboutContent();
            return content is null ? null : AboutContentDto.From(content);
        }
    }

    // ---------- Admin write command ----------

    public class AboutMissionInput
    {
        public string IconName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class AboutOrgNodeInput
    {
        public int Level { get; set; }
        public string Title { get; set; }
    }

    public class AboutBoardMemberInput
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
    }

    // One command replaces the whole page — there is nothing to create or delete, so there is
    // no Id and no Delete counterpart.
    public class UpsertAboutCommand : IRequest<AboutContentDto>
    {
        public string PageTitle { get; set; }
        public string Intro { get; set; }
        public string MissionsTitle { get; set; }
        public string OrgChartTitle { get; set; }
        public string BoardTitle { get; set; }
        public string DutiesTitle { get; set; }
        public List<AboutMissionInput> Missions { get; set; } = new();
        public List<AboutOrgNodeInput> OrgNodes { get; set; } = new();
        public List<AboutBoardMemberInput> BoardMembers { get; set; } = new();
        public List<string> Duties { get; set; } = new();
    }

    public class UpsertAboutCommandValidator : AbstractValidator<UpsertAboutCommand>
    {
        public UpsertAboutCommandValidator()
        {
            RuleFor(x => x.PageTitle).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Intro).MaximumLength(4000);
            RuleFor(x => x.MissionsTitle).MaximumLength(200);
            RuleFor(x => x.OrgChartTitle).MaximumLength(200);
            RuleFor(x => x.BoardTitle).MaximumLength(200);
            RuleFor(x => x.DutiesTitle).MaximumLength(200);
            RuleForEach(x => x.Missions).ChildRules(m =>
            {
                m.RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
                m.RuleFor(x => x.Description).MaximumLength(1000);
            });
            RuleForEach(x => x.OrgNodes).ChildRules(n =>
            {
                n.RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
                n.RuleFor(x => x.Level).InclusiveBetween(0, 4)
                    .WithMessage("سطح باید بین ۰ تا ۴ باشد.");
            });
            RuleForEach(x => x.BoardMembers).ChildRules(m =>
            {
                m.RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
                m.RuleFor(x => x.Role).MaximumLength(200);
                m.RuleFor(x => x.Description).MaximumLength(1000);
            });
            RuleForEach(x => x.Duties).MaximumLength(1000);
        }
    }

    public class AboutCommandHandlers(ILandingRepository repo) : IRequestHandler<UpsertAboutCommand, AboutContentDto>
    {
        // Blank rows are the natural residue of a dynamic form — drop them rather than
        // persisting empties. SortOrder follows the order the admin arranged them in.
        public async Task<AboutContentDto> Handle(UpsertAboutCommand r, CancellationToken ct)
        {
            var entity = new AboutContent
            {
                PageTitle = r.PageTitle,
                Intro = r.Intro,
                MissionsTitle = r.MissionsTitle,
                OrgChartTitle = r.OrgChartTitle,
                BoardTitle = r.BoardTitle,
                DutiesTitle = r.DutiesTitle,
                Missions = (r.Missions ?? new List<AboutMissionInput>())
                    .Where(m => !string.IsNullOrWhiteSpace(m.Title))
                    .Select((m, i) => new AboutMission
                    {
                        Id = Guid.NewGuid(), SortOrder = i + 1,
                        IconName = m.IconName, Title = m.Title.Trim(), Description = m.Description,
                    }).ToList(),
                OrgNodes = (r.OrgNodes ?? new List<AboutOrgNodeInput>())
                    .Where(n => !string.IsNullOrWhiteSpace(n.Title))
                    .Select((n, i) => new AboutOrgNode
                    {
                        Id = Guid.NewGuid(), SortOrder = i + 1, Level = n.Level, Title = n.Title.Trim(),
                    }).ToList(),
                BoardMembers = (r.BoardMembers ?? new List<AboutBoardMemberInput>())
                    .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                    .Select((m, i) => new AboutBoardMember
                    {
                        Id = Guid.NewGuid(), SortOrder = i + 1,
                        Name = m.Name.Trim(), Role = m.Role, Description = m.Description,
                    }).ToList(),
                Duties = (r.Duties ?? new List<string>())
                    .Where(d => !string.IsNullOrWhiteSpace(d))
                    .Select((d, i) => new AboutDuty
                    {
                        Id = Guid.NewGuid(), SortOrder = i + 1, Text = d.Trim(),
                    }).ToList(),
            };

            var saved = await repo.UpdateAboutContent(entity);
            return saved is null ? null : AboutContentDto.From(saved);
        }
    }
}
