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

        public async Task<IEnumerable<StatItem>> GetStats() =>
            await context.StatItems.AsNoTracking().OrderBy(s => s.SortOrder).ToListAsync();

        public async Task<ContactMessage> AddContactMessage(ContactMessage message)
        {
            context.ContactMessages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}
