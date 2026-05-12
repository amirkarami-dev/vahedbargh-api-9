using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.SupportAgg;

public interface ISupportMessageRepository:IRepository<SupportMessage>
{
    Task<IEnumerable<SupportMessage>> getBySupportId(string  supportId);
    
    Task MarkMessagesAsReadByReceiver(string supportId, string receiverUserId);

    Task<int> GetUnreadMessageCountBySupportId(Guid supportId, string currentUserId);

    Task<Dictionary<Guid, int>> GetUnreadMessageCountsBySupportIds(IEnumerable<Guid> supportIds, string currentUserId);
}