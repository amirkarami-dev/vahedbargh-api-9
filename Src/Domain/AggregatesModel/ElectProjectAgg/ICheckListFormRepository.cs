using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public interface ICheckListFormRepository:IRepository<CheckListForm>
{
    Task<IEnumerable<CheckListForm>> GetAllCheckListForm();
    Task<IEnumerable<CheckListForm>> GetAllCheckListForm(Guid engId);
    Task<IEnumerable<Common.ViewModels.CheckListForm>> GetCheckLIstEngForm(Guid electProjectId, Guid engId);



}