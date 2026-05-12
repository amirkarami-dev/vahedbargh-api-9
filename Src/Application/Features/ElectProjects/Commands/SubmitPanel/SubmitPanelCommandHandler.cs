using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.SubmitPanel;

public class SubmitPanelCommandHandler:IRequestHandler<SubmitPanelCommand,string>
{
    private readonly IElectProjectRepository repository;
    private readonly IElectProjectFileRepository electProjectFileRepository;

    public SubmitPanelCommandHandler(IElectProjectRepository repository, IElectProjectFileRepository electProjectFileRepository)
    {
        this.repository = repository;
        this.electProjectFileRepository = electProjectFileRepository;
    }
    public async Task<string> Handle(SubmitPanelCommand request, CancellationToken cancellationToken)
    {
        var electProject = await repository.GetElectProjectById(Guid.Parse(request.Id));
        if (electProject == null) throw new NotFoundException("این پرونده یافت نشد");

        var projectFiles = await electProjectFileRepository.GetByIdElectProject(electProject.Id);
        var checkListBoardFile = projectFiles.FirstOrDefault(f => f.FileTypeEnum == FileTypeEnum.CheckListBoard);

        if (checkListBoardFile is null ) throw new NotFoundException("ابتدا باید چک لیست تابلو آپلود شود ");


        electProject.SubmitPanel(request.PanelSerialNumber, !electProject.PanelMakerSubmit);

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return request.Id;
    }
}