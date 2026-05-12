using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Features.Engineers.Queries.GetListEngineer;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Quotas.Queries.GetEngQuotaBurns;

public class GetEngQuotaBurnQueryHandler(
    IClientRepository clientRepository,
    IUserManager userManager,
    ICurrentUser currentUser,
    IMapper mapper,
    IMediator mediator,
    IEngQuotaBurnRepository enuQuotaBurnRepository)
    : IRequestHandler<GetEngQuotaBurnQuery, IEnumerable<EngQuotaBurnDto>>
{
    private readonly IUserManager userManager = userManager;


    public async Task<IEnumerable<EngQuotaBurnDto>> Handle(GetEngQuotaBurnQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        var engQuotaBurn = await 
            enuQuotaBurnRepository.GetAllEngQuotaBurn(string.IsNullOrEmpty(request.EngId) ? null : Guid.Parse(request.EngId), Guid.Parse(request.QtId));


        var resultList =  mapper.Map<IEnumerable<EngQuotaBurnDto>>(engQuotaBurn);
        foreach (var result in resultList)
        {
            //var des = await mediator.Send(new GetListEngineerQuery()
            //{
            //    EngId = result.Engineer.Id.ToString(),
            //    EngineerGradeTypeEnum = EngineerGradeTypeEnum.None,
            //    FilterCertEnum = FilterCertEnum.Fc0
            //},cancellationToken);

            //result.QuotaDes = des.FirstOrDefault()!.FullDescription;
            result.QuotaDes = result.Engineer.FullName;
        }
        return resultList;

    }
}