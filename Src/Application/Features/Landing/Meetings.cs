using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using FluentValidation;
using MediatR;

namespace Coreapi.Application.Features.Landing.Meetings
{
    public class ResolutionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
    }

    public class MeetingDto
    {
        public Guid Id { get; set; }
        public int SessionNumber { get; set; }
        public string Subject { get; set; }
        public string JalaliDate { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string PdfUrl { get; set; }
        public List<string> Attendees { get; set; }
        public string Notes { get; set; }
        public List<ResolutionDto> Resolutions { get; set; }

        public static MeetingDto From(Meeting m) => new()
        {
            Id = m.Id, SessionNumber = m.SessionNumber, Subject = m.Subject, JalaliDate = m.JalaliDate,
            Date = m.Date, Status = m.Status, Type = m.Type, PdfUrl = m.PdfUrl, Notes = m.Notes,
            Attendees = string.IsNullOrWhiteSpace(m.Attendees)
                ? new List<string>()
                : m.Attendees.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
            Resolutions = (m.Resolutions ?? new List<MeetingResolution>())
                .Select(x => new ResolutionDto { Id = x.Id, Text = x.Text, Status = x.Status }).ToList(),
        };
    }

    public class GetAllMeetingsQuery : IRequest<IEnumerable<MeetingDto>>
    {
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class GetLatestMeetingsQuery : IRequest<IEnumerable<MeetingDto>>
    {
        public int Count { get; set; } = 5;
    }

    public class GetMeetingByIdQuery : IRequest<MeetingDto>
    {
        public Guid Id { get; set; }
    }

    public class MeetingQueryHandlers(ILandingRepository repo) :
        IRequestHandler<GetAllMeetingsQuery, IEnumerable<MeetingDto>>,
        IRequestHandler<GetLatestMeetingsQuery, IEnumerable<MeetingDto>>,
        IRequestHandler<GetMeetingByIdQuery, MeetingDto>
    {
        public async Task<IEnumerable<MeetingDto>> Handle(GetAllMeetingsQuery r, CancellationToken ct) =>
            (await repo.GetMeetings(r.Type, r.Status)).Select(MeetingDto.From);

        public async Task<IEnumerable<MeetingDto>> Handle(GetLatestMeetingsQuery r, CancellationToken ct) =>
            (await repo.GetLatestMeetings(r.Count)).Select(MeetingDto.From);

        public async Task<MeetingDto> Handle(GetMeetingByIdQuery r, CancellationToken ct)
        {
            var m = await repo.GetMeetingById(r.Id);
            return m is null ? null : MeetingDto.From(m);
        }
    }

    // ---------- Admin write commands ----------

    public class ResolutionInput
    {
        public string Text { get; set; }
        public string Status { get; set; }
    }

    public class UpsertMeetingCommand : IRequest<MeetingDto>
    {
        public Guid? Id { get; set; } // null = create, set = update
        public int SessionNumber { get; set; }
        public string Subject { get; set; }
        public string JalaliDate { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string PdfUrl { get; set; }
        public List<string> Attendees { get; set; } = new();
        public string Notes { get; set; }
        public List<ResolutionInput> Resolutions { get; set; } = new();
    }

    public class DeleteMeetingCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class UpsertMeetingCommandValidator : AbstractValidator<UpsertMeetingCommand>
    {
        public UpsertMeetingCommandValidator()
        {
            RuleFor(x => x.Subject).NotEmpty().MaximumLength(500);
            RuleFor(x => x.SessionNumber).GreaterThan(0);
        }
    }

    public class MeetingCommandHandlers(ILandingRepository repo) :
        IRequestHandler<UpsertMeetingCommand, MeetingDto>,
        IRequestHandler<DeleteMeetingCommand, bool>
    {
        private static List<MeetingResolution> BuildResolutions(UpsertMeetingCommand r, Guid meetingId) =>
            (r.Resolutions ?? new List<ResolutionInput>())
                .Where(x => !string.IsNullOrWhiteSpace(x.Text))
                .Select(x => new MeetingResolution
                {
                    Id = Guid.NewGuid(), MeetingId = meetingId, Text = x.Text, Status = x.Status,
                }).ToList();

        public async Task<MeetingDto> Handle(UpsertMeetingCommand r, CancellationToken ct)
        {
            var attendees = string.Join(",", (r.Attendees ?? new List<string>())
                .Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim()));

            if (r.Id is Guid id)
            {
                var updated = await repo.UpdateMeeting(new Meeting
                {
                    Id = id, SessionNumber = r.SessionNumber, Subject = r.Subject, JalaliDate = r.JalaliDate,
                    Date = r.Date, Status = r.Status, Type = r.Type, PdfUrl = r.PdfUrl, Attendees = attendees,
                    Notes = r.Notes, Resolutions = BuildResolutions(r, id),
                });
                return updated is null ? null : MeetingDto.From(updated);
            }

            var newId = Guid.NewGuid();
            var created = await repo.AddMeeting(new Meeting
            {
                Id = newId, SessionNumber = r.SessionNumber, Subject = r.Subject, JalaliDate = r.JalaliDate,
                Date = r.Date, Status = r.Status, Type = r.Type, PdfUrl = r.PdfUrl, Attendees = attendees,
                Notes = r.Notes, Resolutions = BuildResolutions(r, newId),
            });
            return MeetingDto.From(created);
        }

        public async Task<bool> Handle(DeleteMeetingCommand r, CancellationToken ct) =>
            await repo.DeleteMeeting(r.Id);
    }
}
