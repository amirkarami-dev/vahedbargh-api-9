using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg
{
    public interface IElectProjectFileRepository : IRepository<ElectProjectFile>
    {
        Task<IEnumerable<ElectProjectFile>> GetByIdElectProject(Guid electProjectId);
		Task<IEnumerable<ElectProjectFile>> GetByFileType(FileTypeEnum fileTypeEnum);
        Task<ElectProjectFile?> GetFileById(Guid id);
	}
}
