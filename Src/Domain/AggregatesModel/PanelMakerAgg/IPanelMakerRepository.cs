using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.PanelMakerAgg;

public interface IPanelMakerRepository:IRepository<PanelMaker>
{
        Task<IEnumerable<PanelMaker>> GetClientPanelMakers(Guid clientId);
        Task<PanelMaker> GetByUserid(string userid);

}