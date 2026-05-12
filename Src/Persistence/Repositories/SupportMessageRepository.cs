using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class SupportMessageRepository(CoreapiDbContext context)
    : BaseRepository<SupportMessage>(context), ISupportMessageRepository
{
    public async Task<IEnumerable<SupportMessage>> getBySupportId(string supportId)
    {
        return await context.SupportMessages
   .Include(i=>i.Support)
            .Where(w=>w.Support.Id == Guid.Parse(supportId))
            .OrderByDescending(o => o.JulianCreated)
      .ToListAsync();
    }

  public async Task MarkMessagesAsReadByReceiver(string supportId, string receiverUserId)
    {
        var unreadMessages = await context.SupportMessages
       .Where(m => m.Support.Id == Guid.Parse(supportId) 
                && m.ToUserId == receiverUserId 
      && !m.IsReadByReceiver)
      .ToListAsync();

  foreach (var message in unreadMessages)
        {
      message.MarkAsReadByReceiver();
        }
    }

    public async Task<int> GetUnreadMessageCountBySupportId(Guid supportId, string currentUserId)
    {
      return await context.SupportMessages
          .Where(m => m.Support.Id == supportId 
       && m.ToUserId == currentUserId 
        && !m.IsReadByReceiver)
       .CountAsync();
    }

 public async Task<Dictionary<Guid, int>> GetUnreadMessageCountsBySupportIds(IEnumerable<Guid> supportIds, string currentUserId)
    {
    var supportIdsList = supportIds.ToList();
        
        // یک Query برای دریافت تعداد پیام‌های خوانده نشده برای همه Supportها
 var unreadCounts = await context.SupportMessages
  .Where(m => supportIdsList.Contains(m.Support.Id)
                && m.ToUserId == currentUserId
    && !m.IsReadByReceiver)
            .GroupBy(m => m.Support.Id)
          .Select(g => new 
         { 
        SupportId = g.Key, 
    Count = g.Count() 
            })
    .ToListAsync();

        // تبدیل به Dictionary
        return unreadCounts.ToDictionary(x => x.SupportId, x => x.Count);
    }
}