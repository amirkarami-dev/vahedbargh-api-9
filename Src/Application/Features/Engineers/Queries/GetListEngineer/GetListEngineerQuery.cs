using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.ViewModels;
using MediatR;

namespace Coreapi.Application.Features.Engineers.Queries.GetListEngineer
{
    public class GetListEngineerQuery:IRequest<IEnumerable<ClientEngineersViewModel>>
    {
        public FilterCertEnum FilterCertEnum { get; set; }
        public EngineerGradeTypeEnum EngineerGradeTypeEnum { get; set; } = EngineerGradeTypeEnum.None;

        public string EngId { get; set; }
    }
}
