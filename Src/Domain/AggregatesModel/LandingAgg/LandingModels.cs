using System;
using System.Collections.Generic;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.LandingAgg
{
    // Public landing/CMS content — GLOBAL (no ClientId): the office's public site
    // (kurdnezambargh2.ir). Served anonymously; replaces the old Supabase tables.

    public class Announcement : BaseModel<Guid>
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; } // urgent | important | info | news
        public string JalaliDate { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ImageUrl { get; set; }
        public bool Featured { get; set; }
    }

    public class Meeting : BaseModel<Guid>
    {
        public int SessionNumber { get; set; }
        public string Subject { get; set; }
        public string JalaliDate { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string PdfUrl { get; set; }
        public string Attendees { get; set; } // comma-separated
        public string Notes { get; set; }
        public List<MeetingResolution> Resolutions { get; set; } = new();
    }

    public class MeetingResolution : BaseModel<Guid>
    {
        public Guid MeetingId { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
    }

    public class Document : BaseModel<Guid>
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string JalaliDate { get; set; }
        public DateTime Date { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string FileSize { get; set; }
        public int DownloadCount { get; set; }
        public string Tags { get; set; } // comma-separated
        public string FileUrl { get; set; }
        public bool Featured { get; set; }
    }

    public class StatItem : BaseModel<Guid>
    {
        public string Label { get; set; }
        public int Value { get; set; }
        public string Suffix { get; set; }
        public string IconName { get; set; }
        public int SortOrder { get; set; }
    }

    public class ContactMessage : BaseModel<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
