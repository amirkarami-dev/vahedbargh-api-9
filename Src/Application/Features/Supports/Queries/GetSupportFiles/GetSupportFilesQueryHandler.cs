using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using MediatR;

namespace Coreapi.Application.Features.Supports.Queries.GetSupportFiles;

public class GetSupportFilesQueryHandler:IRequestHandler<GetSupportFilesQuery,IEnumerable<SupportFileDto>>
{

    private readonly IMapper mapper;
    private readonly ICurrentUser currentUser;
    private readonly IClientRepository clientRepository;
    private readonly ISupportFileRepository supportFileRepository;

    public GetSupportFilesQueryHandler(IMapper mapper, ICurrentUser currentUser, IClientRepository clientRepository, ISupportFileRepository supportFileRepository)
    {
        this.mapper = mapper;
        this.currentUser = currentUser;
        this.clientRepository = clientRepository;
        this.supportFileRepository = supportFileRepository;
    }
    public async Task<IEnumerable<SupportFileDto>> Handle(GetSupportFilesQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetWithSetting(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);


        var supportFiles = await supportFileRepository.GetBySupportId(Guid.Parse(request.SupportId));
        if (supportFiles is null) throw new NotFoundException("فایل تیکت وجود ندارد");

        return mapper.Map<IEnumerable<SupportFileDto>>(supportFiles);
    }
}