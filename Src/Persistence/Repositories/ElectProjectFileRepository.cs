using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Coreapi.Persistence.Repositories
{
    public class ElectProjectFileRepository(CoreapiDbContext context)
        : BaseRepository<ElectProjectFile>(context), IElectProjectFileRepository
    {
        public async Task<IEnumerable<ElectProjectFile>> GetByIdElectProject(Guid electProjectId)
        {
            return await context.ElectProjectFiles
                .Where(c => EF.Property<Guid>(c, "ElectProjectId") == electProjectId)
                .Include(i => i.ElectProject)
                .ToListAsync();
        }

        public async Task<IEnumerable<ElectProjectFile>> GetByFileType(FileTypeEnum fileTypeEnum)
        {
            return await context.ElectProjectFiles
                .Where(c => c.FileTypeEnum == fileTypeEnum)
                .ToListAsync();
        }

        public async Task<ElectProjectFile?> GetFileById(Guid id)
        {
            return await context.ElectProjectFiles
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
