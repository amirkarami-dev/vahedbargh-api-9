using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using MediatR;

namespace Coreapi.Application.Features.Landing.Announcements
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string JalaliDate { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ImageUrl { get; set; }
        public bool Featured { get; set; }

        public static AnnouncementDto From(Announcement a) => new()
        {
            Id = a.Id, Slug = a.Slug, Title = a.Title, Excerpt = a.Excerpt, Content = a.Content,
            Category = a.Category, Priority = a.Priority, JalaliDate = a.JalaliDate,
            PublishedAt = a.PublishedAt, ImageUrl = a.ImageUrl, Featured = a.Featured,
        };
    }

    public class GetAllAnnouncementsQuery : IRequest<IEnumerable<AnnouncementDto>>
    {
        public string Priority { get; set; }
        public string Category { get; set; }
        public string Search { get; set; }
    }

    public class GetLatestAnnouncementsQuery : IRequest<IEnumerable<AnnouncementDto>>
    {
        public int Count { get; set; } = 4;
    }

    public class GetUrgentAnnouncementsQuery : IRequest<IEnumerable<AnnouncementDto>> { }

    public class GetAnnouncementBySlugQuery : IRequest<AnnouncementDto>
    {
        public string Slug { get; set; }
    }

    public class AnnouncementQueryHandlers(ILandingRepository repo) :
        IRequestHandler<GetAllAnnouncementsQuery, IEnumerable<AnnouncementDto>>,
        IRequestHandler<GetLatestAnnouncementsQuery, IEnumerable<AnnouncementDto>>,
        IRequestHandler<GetUrgentAnnouncementsQuery, IEnumerable<AnnouncementDto>>,
        IRequestHandler<GetAnnouncementBySlugQuery, AnnouncementDto>
    {
        public async Task<IEnumerable<AnnouncementDto>> Handle(GetAllAnnouncementsQuery r, CancellationToken ct) =>
            (await repo.GetAnnouncements(r.Priority, r.Category, r.Search)).Select(AnnouncementDto.From);

        public async Task<IEnumerable<AnnouncementDto>> Handle(GetLatestAnnouncementsQuery r, CancellationToken ct) =>
            (await repo.GetLatestAnnouncements(r.Count)).Select(AnnouncementDto.From);

        public async Task<IEnumerable<AnnouncementDto>> Handle(GetUrgentAnnouncementsQuery r, CancellationToken ct) =>
            (await repo.GetUrgentAnnouncements()).Select(AnnouncementDto.From);

        public async Task<AnnouncementDto> Handle(GetAnnouncementBySlugQuery r, CancellationToken ct)
        {
            var a = await repo.GetAnnouncementBySlug(r.Slug);
            return a is null ? null : AnnouncementDto.From(a);
        }
    }
}
