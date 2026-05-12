using Coreapi.Common.Enums;
using Coreapi.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Coreapi.Domain.AggregatesModel.SupportAgg;

public interface ISupportFileRepository:IRepository<SupportFile>
{
    Task<IEnumerable<SupportFile>> GetBySupportId(Guid supportId);

    void DeleteSupportFile(SupportFile file);
}