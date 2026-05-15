using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Common.RequestModel;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public interface IElectProjectRepository:IRepository<ElectProject>
{
    Task<long> GetLastFileNumber(Guid clientId,int startNumber);
    Task<long> CheckIndexElectRequestNumber(Guid clientId, long electRequestNumber);
    Task<ElectProject> GetElectProjectByFileNumber(long fileNumber);
    Task<ElectProject> GetElectProjectById(Guid id);
    Task<long> GetProjectBalance(Guid id);
    Task<ElectProjectViewMainModel> GetElectProjectsFullFilter(Guid clientId,Guid? panelMakerId, int idSection, ElectProjectsFullFilterModel filterModel);
    Task<List<ElectProjectFile>> GetFilesByFileNumber(long fileNumber);
    Task<List<ElectProject>> GetElectProjectsByLandlordNaCode(string naCode);
    Task<ElectProject?> GetElectProjectByElectRequestNumber(long electRequestNumber);

}