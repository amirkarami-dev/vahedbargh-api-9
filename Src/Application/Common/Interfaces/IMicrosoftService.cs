using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Common.Interfaces
{
    public interface IMicrosoftService
    {
        Task<IEnumerable<MicrosoftUserModel>> GetUsers(string token);
    }
}
