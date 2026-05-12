using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Stop;

public class StopProjectCommandHandler : IRequestHandler<StopProjectCommand, int>
{
    private readonly IElectProjectRepository _repository;

    public StopProjectCommandHandler(IElectProjectRepository repository)
    {
        _repository = repository;
    }
    public async Task<int> Handle(StopProjectCommand request, CancellationToken cancellationToken)
    {
        var electProject = await _repository.GetById(request.GpId) ?? throw new NotFoundException("این پرونده وجود ندارد");



        if (electProject.ProjectLevelEnum is ProjectLevelEnum.ApproveStage)
            throw new NotFoundException("پرنده در انتظار تایید نقشه است ابتدا باید تخصیصی را حذف کنید");

        if (electProject.ProjectLevelEnum is ProjectLevelEnum.ApproveStage)
            throw new NotFoundException("پرنده در انتظار تایید کارشناسی است ابتدا تخصیصی را باید حذف کنید");

        if (electProject.ProjectLevelEnum is ProjectLevelEnum.NullStage)
            throw new NotFoundException("پرنده در مرحله قبل از تخصیص می باشد و مجری میتواند آن را حذف کند");

        electProject.StopProject(request.IsStop, request.StopDes + " - " + Helper.MiladiToShamsi(DateTime.Now));

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return 0;
    }
}