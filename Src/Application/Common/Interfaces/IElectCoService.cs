using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models.ElectCo;

namespace Coreapi.Application.Common.Interfaces
{
    public interface IElectCoService
    {
        Task<string> SendElectProjectFile(ElectCoMapModel electCoMapModel);
        Task<string> SendElectProjectFileAttach(ElectCoMapAttachModel electCoMapAttachModel);
    }
}
