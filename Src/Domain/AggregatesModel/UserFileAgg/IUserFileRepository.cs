using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.UserFileAgg
{
    public interface IUserFileRepository:IRepository<UserFile>
    {
        Task<IEnumerable<UserFile>> GetUserFilesByUserId(string userId);
        void Delete(UserFile userFile);
    }
}
