using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Defect;

public class UpdateDefectCommandHandler:IRequestHandler<UpdateDefectCommand,int>
{
    private readonly IElectProjectRepository electProjectRepository;

    public UpdateDefectCommandHandler(IElectProjectRepository electProjectRepository)
    {
        this.electProjectRepository = electProjectRepository;
    }
    public async Task<int> Handle(UpdateDefectCommand request, CancellationToken cancellationToken)
    {
        var electProject = await electProjectRepository.GetElectProjectById(request.ElectProjectId);
        if (electProject == null) throw new NotFoundException("این پرونده پیدا نشد");

        electProject.UpdateDefectDes(request.DefectDes);

        await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return 0;
    }
}