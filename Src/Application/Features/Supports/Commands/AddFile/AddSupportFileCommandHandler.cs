using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using MediatR;

namespace Coreapi.Application.Features.Supports.Commands.AddFile;

public class AddSupportFileCommandHandler:IRequestHandler<AddSupportFileCommand, Guid>
{
    private readonly ISupportRepository supportRepository;
    private readonly ISupportFileRepository supportFileRepository;
    private readonly ICurrentUser currentUser;
    private readonly IS3Service s3Service;

    public AddSupportFileCommandHandler(ISupportRepository supportRepository, ISupportFileRepository supportFileRepository, ICurrentUser currentUser, IS3Service s3Service)
    {
        this.supportRepository = supportRepository;
        this.supportFileRepository = supportFileRepository;
        this.currentUser = currentUser;
        this.s3Service = s3Service;
    }
    public async Task<Guid> Handle(AddSupportFileCommand request, CancellationToken cancellationToken)
    {
 
        var supportId = JsonSerializer.Deserialize<Guid>(request.SupportId);

        var support = await supportRepository.GetById(supportId);
        if (support == null) throw new NotFoundException("این تیکت وجود ندارد");

        var fileName = request.FileName.Split('.')[0] + Helper.MiladiToShamsiForName(DateTime.Now) + "." + request.FileName.Split('.')[1];

        await s3Service.UploadFileAttach(request.File, fileName, request.FolderName, supportId.ToString());

        var supportFile = new SupportFile(
            request.Name + "-" + supportId,
            request.Des,
            request.FileTypeEnum,
            supportId.ToString(),
            fileName.Split('.')[0] + "." + fileName.Split('.')[1],
            currentUser.UserId,
            request.ToUserId,
            support
        );
        supportFileRepository.Add(supportFile);

        await supportFileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return supportFile.Id;

    }
}