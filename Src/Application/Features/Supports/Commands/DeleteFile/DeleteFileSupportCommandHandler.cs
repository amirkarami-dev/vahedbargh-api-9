using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using MediatR;

namespace Coreapi.Application.Features.Supports.Commands.DeleteFile;

public class DeleteFileSupportCommandHandler:IRequestHandler<DeleteFileSupportCommand>
{
    private readonly ISupportFileRepository supportFileRepository;
    private readonly IS3ServicePublic s3Service;


    public DeleteFileSupportCommandHandler(ISupportFileRepository supportFileRepository, IS3ServicePublic s3Service)
    {
        this.supportFileRepository = supportFileRepository;
        this.s3Service = s3Service;
    }
    public async Task Handle(DeleteFileSupportCommand request, CancellationToken cancellationToken)
    {
        var supportFile = await supportFileRepository.GetById(Guid.Parse(request.Id));
        if (supportFile == null) throw new NotFoundException("فایل پیدا نشد");
        await s3Service.DeleteFile("Upload/GasProjects/" + supportFile.FolderName, supportFile.FileName);

         supportFileRepository.DeleteSupportFile(supportFile);

         await supportFileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}