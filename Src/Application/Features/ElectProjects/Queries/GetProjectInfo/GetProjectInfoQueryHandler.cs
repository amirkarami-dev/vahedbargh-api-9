using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetProjectInfo;

public class GetProjectInfoQueryHandler(
    IElectProjectRepository electProjectRepository,
    ITransactionRepository transactionRepository,
    IClientRepository clientRepository
    ):IRequestHandler<GetProjectInfoQuery,ProjectInfoDto>
{




    public async Task<ProjectInfoDto> Handle(GetProjectInfoQuery request, CancellationToken cancellationToken)
    {
        var returnObject = new ProjectInfoDto();

        var decodeId = Helper.DecodeGuid(request.ProjectId);

        var elecProject = await electProjectRepository.GetElectProjectById(decodeId);
        if (elecProject == null || elecProject.IsDelete) throw new NotFoundException("این پرونده وجود ندارد");

        var client = await clientRepository.GetById(elecProject.Client.Id);
        if (client is null)
        {
            throw new NotAllowedException("این پرونده مربوط به واحد برق نمی باشد");
        }

        var projectBalance = await transactionRepository.GetProjectBalance(elecProject.Id);

        returnObject.Id = elecProject.Id;
        returnObject.AmountPay = Math.Abs(projectBalance);
        returnObject.Address = elecProject.Address;
        returnObject.LandlordName = elecProject.LandlordName;
        returnObject.PostalCode = elecProject.PostalCode;
        returnObject.NaCode = elecProject.LandlordNaCode;
        returnObject.FileNumber = elecProject.FileNumber;
        returnObject.ElectRequestNumber = elecProject.ElectRequestNumber;


        return returnObject;

    }
}