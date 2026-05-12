using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Status;

public class UpdateElectProjectStatusCommandHandler:IRequestHandler<UpdateElectProjectStatusCommand,int>
{
    private readonly IElectProjectRepository electProjectRepository;

    public UpdateElectProjectStatusCommandHandler(IElectProjectRepository electProjectRepository)
    {
        this.electProjectRepository = electProjectRepository;
    }
    public async Task<int> Handle(UpdateElectProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var electProject = await electProjectRepository.GetElectProjectById(request.ElectProjectId);
        if (electProject == null) throw new NotFoundException("این پرونده پیدا نشد");
        electProject.UpdateStatus(request.ElectProjectStatusEnum);

        await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return 0;

    }
}