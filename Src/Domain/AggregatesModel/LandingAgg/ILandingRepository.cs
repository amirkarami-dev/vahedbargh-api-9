using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coreapi.Domain.AggregatesModel.LandingAgg
{
    // Cohesive repository for the public landing/CMS content (global, anonymous).
    public interface ILandingRepository
    {
        Task<IEnumerable<Announcement>> GetAnnouncements(string priority, string category, string search);
        Task<IEnumerable<Announcement>> GetLatestAnnouncements(int count);
        Task<IEnumerable<Announcement>> GetUrgentAnnouncements();
        Task<Announcement> GetAnnouncementBySlug(string slug);

        Task<IEnumerable<Meeting>> GetMeetings(string type, string status);
        Task<IEnumerable<Meeting>> GetLatestMeetings(int count);
        Task<Meeting> GetMeetingById(Guid id);

        Task<IEnumerable<Document>> GetDocuments(string category, string search, string sortBy);
        Task<IReadOnlyList<KeyValuePair<string, int>>> GetDocumentCategories();
        Task<int?> IncrementDownload(Guid id);

        Task<IEnumerable<StatItem>> GetStats();

        Task<ContactMessage> AddContactMessage(ContactMessage message);
    }
}
