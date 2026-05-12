using Coreapi.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public interface ICheckListEdcRepository :IRepository<CheckListEdc>
{
    Task<IEnumerable<CheckListEdc>> GetAllCheckLisEdc();
    Task<IEnumerable<Common.ViewModels.CheckListEdc>> GetCheckListEdcForm(Guid electProjectId);
}