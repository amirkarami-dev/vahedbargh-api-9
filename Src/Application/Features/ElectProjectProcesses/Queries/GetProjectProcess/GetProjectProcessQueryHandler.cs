using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Queries.GetProjectProcess
{
    public class GetProjectProcessQueryHandler:IRequestHandler<GetProjectProcessQuery,IEnumerable<GetProjectProcessDto>>
    {
        private readonly IElectProjectProcessRepository _repository;
        private readonly IMapper mapper;

        public GetProjectProcessQueryHandler(IElectProjectProcessRepository repository, IMapper mapper)
        {
            _repository = repository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<GetProjectProcessDto>> Handle(GetProjectProcessQuery request, CancellationToken cancellationToken)
        {
            var projectProcess = await _repository.GetElectProjectProcessByEpId(request.EpId);
    
            return mapper.Map<IEnumerable<GetProjectProcessDto>>(projectProcess);
        }
    }
}
