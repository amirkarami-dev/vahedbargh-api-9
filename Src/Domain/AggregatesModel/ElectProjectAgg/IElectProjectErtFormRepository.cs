using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public interface IElectProjectErtFormRepository:IRepository<ElectProjectErtForm>
{
    Task<ElectProjectErtForm> GetEpeFormById(Guid id);
    Task<ElectProjectErtForm> GetEpeFormByElectProjectId(Guid electProjectId);
    Task<IEnumerable<ElectProjectErtForm>> GetAllErtForm();
}