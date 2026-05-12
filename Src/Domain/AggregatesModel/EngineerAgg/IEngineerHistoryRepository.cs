using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.EngineerAgg
{
    public interface IEngineerHistoryRepository:IRepository<EngineerHistory>
    {
        Task<EngineerHistory> GetByEngId (Guid engId);
    }
}
