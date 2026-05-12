using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetProjectFiles
{
    public class GetElectProjectFileQueryHandler(IMapper mapper, IElectProjectFileRepository electProjectFileRepository)
        : IRequestHandler<GetElectProjectFileQuery, IEnumerable<ElectProjectFileDto>>
    {
        public async Task<IEnumerable<ElectProjectFileDto>> Handle(GetElectProjectFileQuery request, CancellationToken cancellationToken)
        {


            var projectFiles = await electProjectFileRepository.GetByIdElectProject(Guid.Parse(request.ElectProjectId));

            if (projectFiles is null) throw new NotFoundException("فایل پرونده وجود ندارد");

            return mapper.Map<IEnumerable<ElectProjectFileDto>>(projectFiles);
        }
    }
}
