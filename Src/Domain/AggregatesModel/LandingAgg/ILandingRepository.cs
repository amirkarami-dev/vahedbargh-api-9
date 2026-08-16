using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coreapi.Domain.AggregatesModel.LandingAgg
{
    // Cohesive repository for the public landing/CMS content (global, anonymous).
    public interface ILandingRepository
    {
        // --- Announcements ---
        Task<IEnumerable<Announcement>> GetAnnouncements(string priority, string category, string search);
        Task<IEnumerable<Announcement>> GetLatestAnnouncements(int count);
        Task<IEnumerable<Announcement>> GetUrgentAnnouncements();
        Task<Announcement> GetAnnouncementBySlug(string slug);
        Task<Announcement> GetAnnouncementById(Guid id);
        Task<Announcement> AddAnnouncement(Announcement entity);
        Task<Announcement> UpdateAnnouncement(Announcement entity);
        Task<bool> DeleteAnnouncement(Guid id);

        // --- Meetings ---
        Task<IEnumerable<Meeting>> GetMeetings(string type, string status);
        Task<IEnumerable<Meeting>> GetLatestMeetings(int count);
        Task<Meeting> GetMeetingById(Guid id);
        Task<Meeting> AddMeeting(Meeting entity);
        Task<Meeting> UpdateMeeting(Meeting entity);
        Task<bool> DeleteMeeting(Guid id);

        // --- Documents ---
        Task<IEnumerable<Document>> GetDocuments(string category, string search, string sortBy);
        Task<IReadOnlyList<KeyValuePair<string, int>>> GetDocumentCategories();
        Task<int?> IncrementDownload(Guid id);
        Task<Document> GetDocumentById(Guid id);
        Task<Document> AddDocument(Document entity);
        Task<Document> UpdateDocument(Document entity);
        Task<bool> DeleteDocument(Guid id);

        // --- Stats ---
        Task<IEnumerable<StatItem>> GetStats();
        Task<StatItem> GetStatById(Guid id);
        Task<StatItem> AddStat(StatItem entity);
        Task<StatItem> UpdateStat(StatItem entity);
        Task<bool> DeleteStat(Guid id);

        // --- Process flows (guided procedures) ---
        Task<IEnumerable<ProcessFlow>> GetProcessFlows();
        Task<ProcessFlow> AddProcessFlow(ProcessFlow entity);
        Task<ProcessFlow> UpdateProcessFlow(ProcessFlow entity);
        Task<bool> DeleteProcessFlow(Guid id);

        // --- About page (singleton + its four child collections) ---
        Task<AboutContent> GetAboutContent();
        Task<AboutContent> UpdateAboutContent(AboutContent entity);

        // --- Expert requests (public form -> admin inbox) ---
        Task<ExpertRequest> AddExpertRequest(ExpertRequest entity);
        Task<IEnumerable<ExpertRequest>> GetExpertRequests();
        Task<bool> MarkExpertRequestRead(Guid id, bool isRead);
        Task<bool> DeleteExpertRequest(Guid id);

        // --- Contact messages (admin inbox) ---
        Task<ContactMessage> AddContactMessage(ContactMessage message);
        Task<IEnumerable<ContactMessage>> GetContactMessages();
        Task<bool> MarkContactMessageRead(Guid id, bool isRead);
        Task<bool> DeleteContactMessage(Guid id);
    }
}
