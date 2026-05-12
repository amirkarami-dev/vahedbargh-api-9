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
    private readonly IS3Service s3ServiceLiara;


    public DeleteFileSupportCommandHandler(ISupportFileRepository supportFileRepository, IS3Service s3ServiceLiara)
    {
        this.supportFileRepository = supportFileRepository;
        this.s3ServiceLiara = s3ServiceLiara;
    }
    public async Task Handle(DeleteFileSupportCommand request, CancellationToken cancellationToken)
    {
        var supportFile = await supportFileRepository.GetById(Guid.Parse(request.Id));
        if (supportFile == null) throw new NotFoundException("فایل پیدا نشد");
        await s3ServiceLiara.DeleteFile("Upload/GasProjects/" + supportFile.FolderName, supportFile.FileName);

         supportFileRepository.DeleteSupportFile(supportFile);

         await supportFileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}