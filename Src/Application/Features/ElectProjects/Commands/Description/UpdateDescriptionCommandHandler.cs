using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Description;

public class UpdateDescriptionCommandHandler : IRequestHandler<UpdateDescriptionCommand, int>
{
    private readonly IElectProjectRepository electProjectRepository;

    public UpdateDescriptionCommandHandler(IElectProjectRepository electProjectRepository)
    {
        this.electProjectRepository = electProjectRepository;
    }

    public async Task<int> Handle(UpdateDescriptionCommand request, CancellationToken cancellationToken)
    {
        var electProject = await electProjectRepository.GetElectProjectById(request.ElectProjectId);
        if (electProject == null) throw new NotFoundException("این پرونده پیدا نشد");

        electProject.UpdateDescription(request.Description);

        await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return 0;
    }
}
