using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(CoreapiDbContext context) : base(context)
        {
        }

        public void Add(ClientUserCapture capture)
        {
            context.ClientUserCaptures.Add(capture);
        }

        public async Task<bool> IsCardExist(string cardNumber, CardTypeEnum cardType)
        {
            return await context.ClientCards.AnyAsync(c => c.CardNumber.Equals(cardNumber) && c.Type == cardType);
        }

        public async Task<bool> ApiKeyExist(string apiKey)
        {
            return await context.Clients.AnyAsync(c => c.ApiKey.Equals(apiKey));
        }

        public async Task<bool> SubdomainExist(string subdomain)
        {
            return await context.Clients.AnyAsync(c => c.Subdomain.Equals(subdomain));
        }

        public async Task<Client> GetByApiKey(string apiKey)
        {
            return await context.Clients
                .FirstOrDefaultAsync(c => c.ApiKey.Equals(apiKey));
        }

        public async Task<Client> GetWithPackages(Guid id)
        {
            return await context.Clients
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> GetWithCards(Guid id)
        {
            return await context.Clients
                .Include(c => c.Cards)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> GetWithSetting(Guid id)
        {
            return await context.Clients.Include(c => c.Setting)
                .Include(c=>c.UserSettings)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> GetWithSettingAndPackage(Guid id)
        {
            return await context.Clients.Include(c => c.Setting)
                .Include(c=>c.UserSettings)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> GetWithTrainings(Guid id)
        {
            return await context.Clients
                .Include(c => c.UserTrainings)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> GetWithAreas(Guid id)
        {
            return await context.Clients
                .Include(c => c.Areas)
                .ThenInclude(a => a.Users)
                .Include(c => c.Areas)
                .ThenInclude(a => a.Points)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ClientUserCapture>> GetCaptures(Guid clientId, string userId, int pageNumber, int rowCount)
        {
            return await context.ClientUserCaptures
                .Where(c => EF.Property<Guid>(c, "ClientId") == clientId
                    && (string.IsNullOrEmpty(userId) || c.UserId.Equals(userId)))
                .OrderByDescending(c => c.Created)
                .Skip(pageNumber * rowCount).Take(rowCount)
                .ToListAsync();
        }

        public async Task<int> CapturesCount(Guid clientId, string userId)
        {
            return await context.ClientUserCaptures
                .Where(c => EF.Property<Guid>(c, "ClientId") == clientId
                    && (string.IsNullOrEmpty(userId) || c.UserId.Equals(userId)))
                .CountAsync();
        }

        public async Task<IEnumerable<Client>> GetAnalyzerClients(string userId)
        {
            return await context.Clients
                .Where(c => c.Analyzers.Any(ca => ca.UserId.Equals(userId)))
                .Include(c => c.Analyzers)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClientArea>> GetUserAreas(Guid clientId, string userId)
        {
            return await context.ClientAreas
                .Where(c => EF.Property<Guid>(c, "ClientId") == clientId && c.Users.Any(ca => ca.UserId.Equals(userId)))
                .Include(c => c.Points)
                .ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetAll(IEnumerable<Guid> ids)
        {
            return await context.Clients
                .Where(c => ids.Any(id => id == c.Id))
                .Include(c => c.Analyzers)
                .ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetAll(string name, int pageNumber, int rowCount)
        {
            return await context.Clients
                .Where(c => string.IsNullOrEmpty(name) || c.Name.Contains(name))
                .OrderByDescending(c => EF.Property<DateTime>(c, "LastModified"))
                .Skip(pageNumber * rowCount).Take(rowCount)
                .ToListAsync();
        }

        public async Task<int> Count(string name)
        {
            return await context.Clients
                .Where(c => string.IsNullOrEmpty(name) || c.Name.Contains(name))
                .CountAsync();
        }
    }
}
