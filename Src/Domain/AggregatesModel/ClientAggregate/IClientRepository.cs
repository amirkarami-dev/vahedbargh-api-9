using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate
{
    public interface IClientRepository : IRepository<Client>
    {
        void Add(ClientUserCapture capture);

        Task<bool> ApiKeyExist(string apiKey);
        Task<bool> SubdomainExist(string subdomain);
        Task<Client> GetByApiKey(string apiKey);

        Task<Client> GetWithPackages(Guid id);
        Task<Client> GetWithCards(Guid id);
        Task<Client> GetWithSettingAndPackage(Guid id);
        Task<Client> GetWithSetting(Guid id);
        Task<Client> GetWithTrainings(Guid clientId);
        Task<Client> GetWithAreas(Guid id);

        Task<IEnumerable<ClientUserCapture>> GetCaptures(Guid clientId, string userId, int pageNumber, int rowCount);
        Task<IEnumerable<ClientArea>> GetUserAreas(Guid clientId, string userId);
        Task<IEnumerable<Client>> GetAnalyzerClients(string userId);
        Task<IEnumerable<Client>> GetAll(IEnumerable<Guid> clientIds);
        Task<bool> IsCardExist(string cardNumber, CardTypeEnum type);
        Task<IEnumerable<Client>> GetAll(string name, int pageNumber, int rowCount);
        Task<int> Count(string name);
        Task<int> CapturesCount(Guid clientId, string userId);
    }
}
