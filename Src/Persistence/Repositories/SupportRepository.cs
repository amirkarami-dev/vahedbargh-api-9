using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class SupportRepository(CoreapiDbContext context) : BaseRepository<Support>(context), ISupportRepository
{
    public async Task<IEnumerable<Support>> GetByClosed(bool closed, string fromUserId, int ticketNumber, string userId, UserType userType)
    {
        return await context.Supports
            .Where(c => 
                (c.UserId == fromUserId || (c.ToUserId == fromUserId))
                && (string.IsNullOrEmpty(userId) || c.UserId == userId || (c.ToUserId == userId))
                 && (ticketNumber != 0 || !string.IsNullOrEmpty(userId) || c.Closed == closed)
                 && (ticketNumber==0 || c.TicketNumber ==ticketNumber)
                && (userType == UserType.None || c.UserType == userType)
                )
            .OrderByDescending(o=>o.JulianCreate).ToListAsync();
    }

    public async Task<IEnumerable<Support>> GetSupportList(
        bool closed, 
        string fromUserId, 
        int ticketNumber, 
        string userId,
        UserType userType,
        bool isEngineer,
        SupportListTypeEnum supportListTypeEnum)
    {
        var query = context.Supports.AsQueryable();

        // Base filter - user must be involved in the support (either as sender or receiver)
        query = query.Where(c => c.UserId == fromUserId || c.ToUserId == fromUserId);

        // Filter by specific userId if provided
        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(c => c.UserId == userId || c.ToUserId == userId);
        }

        // Filter by ticket number if provided
        if (ticketNumber != 0)
        {
            query = query.Where(c => c.TicketNumber == ticketNumber);
        }
        else
        {
            // For Engineers: ignore closed filter, show all tickets
            // For non-Engineers: apply closed filter if no specific ticket number or userId is provided
            if (!isEngineer && string.IsNullOrEmpty(userId))
            {
                query = query.Where(c => c.Closed == closed);
            }
        }

        // Filter by user type if specified
        if (userType != UserType.None)
        {
            query = query.Where(c => c.UserType == userType);
        }

        // Apply SupportListType filter
        // For Engineers: always show all (ignore supportListTypeEnum)
        if (!isEngineer)
        {
            query = supportListTypeEnum switch
            {
                SupportListTypeEnum.Inbox => query.Where(c => c.ToUserId == fromUserId),
                SupportListTypeEnum.Send => query.Where(c => c.UserId == fromUserId),
                _ => query // SupportListTypeEnum.All
            };
        }

        return await query.OrderByDescending(o => o.JulianCreate).ToListAsync();
    }

    public async Task<int> GetLastTicketNumber(Guid clientId)
    {
        var row = await context.Supports.Where(t => EF.Property<Guid>(t, "ClientId") == clientId).OrderByDescending(p => p.TicketNumber).FirstOrDefaultAsync();

        return row?.TicketNumber ?? 0;
    }

    public async Task<int> GetCountUnreadMessage(string toUserId)
    {
        return await context.Supports.CountAsync(c => !c.IsRead && c.ToUserId == toUserId);

    }

}