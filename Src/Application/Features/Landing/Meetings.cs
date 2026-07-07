using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
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
}
