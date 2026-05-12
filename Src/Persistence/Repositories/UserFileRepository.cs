using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.UserFileAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class UserFileRepository:BaseRepository<UserFile>,IUserFileRepository
    {
        public UserFileRepository(CoreapiDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserFile>> GetUserFilesByUserId(string userId)
        {
            return await context.UserFiles.Where(c => c.UserId == userId).ToListAsync();
        }

        public void Delete(UserFile userFile)
        {
            context.UserFiles.Remove(userFile);
        }
    }
}
