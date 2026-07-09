using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories
{
    // Read-mostly public landing content. Global (no ClientId scoping).
    public class LandingRepository : ILandingRepository
    {
        private readonly CoreapiDbContext context;

        public LandingRepository(CoreapiDbContext context) => this.context = context;

        public async Task<IEnumerable<Announcement>> GetAnnouncements(string priority, string category, string search)
        {
            var q = context.Announcements.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(priority)) q = q.Where(a => a.Priority == priority);
            if (!string.IsNullOrWhiteSpace(category)) q = q.Where(a => a.Category == category);
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(a => a.Title.Contains(search) || a.Excerpt.Contains(search));
            return await q.OrderByDescending(a => a.PublishedAt).ToListAsync();
        }

        public async Task<IEnumerable<Announcement>> GetLatestAnnouncements(int count) =>
            await context.Announcements.AsNoTracking()
                .OrderByDescending(a => a.PublishedAt).Take(count <= 0 ? 4 : count).ToListAsync();

        public async Task<IEnumerable<Announcement>> GetUrgentAnnouncements() =>
            await context.Announcements.AsNoTracking()
                .Where(a => a.Priority == "urgent").OrderByDescending(a => a.PublishedAt).ToListAsync();

        public async Task<Announcement> GetAnnouncementBySlug(string slug) =>
            await context.Announcements.AsNoTracking().FirstOrDefaultAsync(a => a.Slug == slug);

        public async Task<Announcement> GetAnnouncementById(Guid id) =>
            await context.Announcements.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Announcement> AddAnnouncement(Announcement entity)
        {
            context.Announcements.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Announcement> UpdateAnnouncement(Announcement entity)
        {
            var db = await context.Announcements.FirstOrDefaultAsync(a => a.Id == entity.Id);
            if (db is null) return null;
            db.Slug = entity.Slug;
            db.Title = entity.Title;
            db.Excerpt = entity.Excerpt;
            db.Content = entity.Content;
            db.Category = entity.Category;
            db.Priority = entity.Priority;
            db.JalaliDate = entity.JalaliDate;
            db.ImageUrl = entity.ImageUrl;
            db.Featured = entity.Featured;
            // PublishedAt is the sort key set at creation time; not edited here.
            await context.SaveChangesAsync();
            return db;
        }

        public async Task<bool> DeleteAnnouncement(Guid id)
        {
            var db = await context.Announcements.FirstOrDefaultAsync(a => a.Id == id);
            if (db is null) return false;
            context.Announcements.Remove(db);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Meeting>> GetMeetings(string type, string status)
        {
            var q = context.Meetings.AsNoTracking().Include(m => m.Resolutions).AsQueryable();
            if (!string.IsNullOrWhiteSpace(type)) q = q.Where(m => m.Type == type);
            if (!string.IsNullOrWhiteSpace(status)) q = q.Where(m => m.Status == status);
            return await q.OrderByDescending(m => m.SessionNumber).ToListAsync();
        }

        public async Task<IEnumerable<Meeting>> GetLatestMeetings(int count) =>
            await context.Meetings.AsNoTracking().Include(m => m.Resolutions)
                .OrderByDescending(m => m.Date).Take(count <= 0 ? 5 : count).ToListAsync();

        public async Task<Meeting> GetMeetingById(Guid id) =>
            await context.Meetings.AsNoTracking().Include(m => m.Resolutions).FirstOrDefaultAsync(m => m.Id == id);

        public async Task<Meeting> AddMeeting(Meeting entity)
        {
            context.Meetings.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Meeting> UpdateMeeting(Meeting entity)
        {
            var db = await context.Meetings.Include(m => m.Resolutions).FirstOrDefaultAsync(m => m.Id == entity.Id);
            if (db is null) return null;
            db.SessionNumber = entity.SessionNumber;
            db.Subject = entity.Subject;
            db.JalaliDate = entity.JalaliDate;
            db.Date = entity.Date;
            db.Status = entity.Status;
            db.Type = entity.Type;
            db.PdfUrl = entity.PdfUrl;
            db.Attendees = entity.Attendees;
            db.Notes = entity.Notes;
            // Replace resolutions wholesale (simplest correct semantics for a small child set).
            context.MeetingResolutions.RemoveRange(db.Resolutions);
            db.Resolutions = entity.Resolutions;
            await context.SaveChangesAsync();
            return db;
        }

        public async Task<bool> DeleteMeeting(Guid id)
        {
            var db = await context.Meetings.FirstOrDefaultAsync(m => m.Id == id);
            if (db is null) return false;
            context.Meetings.Remove(db); // resolutions cascade-delete
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Document>> GetDocuments(string category, string search, string sortBy)
        {
            var q = context.Documents.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(category)) q = q.Where(d => d.Category == category);
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(d => d.Title.Contains(search) || d.Description.Contains(search));
            q = sortBy switch
            {
                "name" => q.OrderBy(d => d.Title),
                "category" => q.OrderBy(d => d.Category),
                "downloads" => q.OrderByDescending(d => d.DownloadCount),
                _ => q.OrderByDescending(d => d.Date),
            };
            return await q.ToListAsync();
        }

        public async Task<IReadOnlyList<KeyValuePair<string, int>>> GetDocumentCategories()
        {
            var groups = await context.Documents.AsNoTracking()
                .GroupBy(d => d.Category)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToListAsync();
            return groups.Select(g => new KeyValuePair<string, int>(g.Name, g.Count)).ToList();
        }

        public async Task<int?> IncrementDownload(Guid id)
        {
            var doc = await context.Documents.FirstOrDefaultAsync(d => d.Id == id);
            if (doc is null) return null;
            doc.DownloadCount += 1;
            await context.SaveChangesAsync();
            return doc.DownloadCount;
        }

        public async Task<Document> GetDocumentById(Guid id) =>
            await context.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

        public async Task<Document> AddDocument(Document entity)
        {
            context.Documents.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Document> UpdateDocument(Document entity)
        {
            var db = await context.Documents.FirstOrDefaultAsync(d => d.Id == entity.Id);
            if (db is null) return null;
            db.Title = entity.Title;
            db.Category = entity.Category;
            db.JalaliDate = entity.JalaliDate;
            db.Date = entity.Date;
            db.Version = entity.Version;
            db.Description = entity.Description;
            db.FileSize = entity.FileSize;
            db.Tags = entity.Tags;
            db.FileUrl = entity.FileUrl;
            db.Featured = entity.Featured;
            // DownloadCount is not editable from the admin form; preserve it.
            await context.SaveChangesAsync();
            return db;
        }

        public async Task<bool> DeleteDocument(Guid id)
        {
            var db = await context.Documents.FirstOrDefaultAsync(d => d.Id == id);
            if (db is null) return false;
            context.Documents.Remove(db);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StatItem>> GetStats() =>
            await context.StatItems.AsNoTracking().OrderBy(s => s.SortOrder).ToListAsync();

        public async Task<StatItem> GetStatById(Guid id) =>
            await context.StatItems.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        public async Task<StatItem> AddStat(StatItem entity)
        {
            context.StatItems.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<StatItem> UpdateStat(StatItem entity)
        {
            var db = await context.StatItems.FirstOrDefaultAsync(s => s.Id == entity.Id);
            if (db is null) return null;
            db.Label = entity.Label;
            db.Value = entity.Value;
            db.Suffix = entity.Suffix;
            db.IconName = entity.IconName;
            db.SortOrder = entity.SortOrder;
            await context.SaveChangesAsync();
            return db;
        }

        public async Task<bool> DeleteStat(Guid id)
        {
            var db = await context.StatItems.FirstOrDefaultAsync(s => s.Id == id);
            if (db is null) return false;
            context.StatItems.Remove(db);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProcessFlow>> GetProcessFlows() =>
            await context.ProcessFlows.AsNoTracking()
                .Include(f => f.Steps)
                .OrderBy(f => f.SortOrder)
                .ToListAsync();

        public async Task<ContactMessage> AddContactMessage(ContactMessage message)
        {
            context.ContactMessages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<ContactMessage>> GetContactMessages() =>
            await context.ContactMessages.AsNoTracking().OrderByDescending(m => m.CreatedAt).ToListAsync();

        public async Task<bool> MarkContactMessageRead(Guid id, bool isRead)
        {
            var db = await context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);
            if (db is null) return false;
            db.IsRead = isRead;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContactMessage(Guid id)
        {
            var db = await context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);
            if (db is null) return false;
            context.ContactMessages.Remove(db);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
