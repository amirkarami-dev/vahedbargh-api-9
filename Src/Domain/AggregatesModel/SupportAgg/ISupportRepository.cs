using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.SupportAgg;

public interface ISupportRepository:IRepository<Support>
{
    Task<IEnumerable<Support>> GetByClosed(bool closed, string fromUserId, int ticketNumber, string fromName, UserType userType);

    Task<IEnumerable<Support>> GetSupportList(
        bool closed,
        string fromUserId,
        int ticketNumber,
        string userId,
        UserType userType,
        bool isEngineer,
     SupportListTypeEnum supportListTypeEnum);

    Task<int> GetLastTicketNumber(Guid clientId);

    Task<int> GetCountUnreadMessage(string toUserId);

}