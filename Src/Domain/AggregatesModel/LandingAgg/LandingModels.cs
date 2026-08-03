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

    // A guided process (بازرسی برق / سیستم ارت / تست و تحویل) shown on /processes.
    public class ProcessFlow : BaseModel<Guid>
    {
        public string Key { get; set; } // stable slug used by the UI (e.g. "inspection")
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Color { get; set; } // tailwind gradient classes
        public string GlowColor { get; set; } // rgba() used for glow/borders
        public string Icon { get; set; } // emoji
        public int SortOrder { get; set; }
        public List<ProcessStep> Steps { get; set; } = new();
    }

    public class ProcessStep : BaseModel<Guid>
    {
        public Guid ProcessFlowId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Details { get; set; } // newline-separated list
        public string RequiredDocs { get; set; } // newline-separated list
        public string Tools { get; set; } // newline-separated list
        public string Note { get; set; }
        public bool IsDecision { get; set; }
        public string DecisionYes { get; set; }
        public string DecisionNo { get; set; }
        public int SortOrder { get; set; }
    }

    // The /about page. A singleton row (stable seed Id) that owns every section of the page:
    // the intro, plus four ordered child collections. Section headings live here so the whole
    // page is editable, not just its lists.
    public class AboutContent : BaseModel<Guid>
    {
        public string PageTitle { get; set; }
        public string Intro { get; set; }
        public string MissionsTitle { get; set; }
        public string OrgChartTitle { get; set; }
        public string BoardTitle { get; set; }
        public string DutiesTitle { get; set; }
        public List<AboutMission> Missions { get; set; } = new();
        public List<AboutOrgNode> OrgNodes { get; set; } = new();
        public List<AboutBoardMember> BoardMembers { get; set; } = new();
        public List<AboutDuty> Duties { get; set; } = new();
    }

    public class AboutMission : BaseModel<Guid>
    {
        public Guid AboutContentId { get; set; }
        public string IconName { get; set; } // key into the lucide map in AboutPage.tsx
        public string Title { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }

    // One box in the org chart. Level is the tier it sits on (0 = top), and every node
    // sharing a level renders side by side, connected down to the next level.
    public class AboutOrgNode : BaseModel<Guid>
    {
        public Guid AboutContentId { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }
    }

    public class AboutBoardMember : BaseModel<Guid>
    {
        public Guid AboutContentId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }

    public class AboutDuty : BaseModel<Guid>
    {
        public Guid AboutContentId { get; set; }
        public string Text { get; set; }
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
        public bool IsRead { get; set; } // admin inbox read/unread flag
    }
}
