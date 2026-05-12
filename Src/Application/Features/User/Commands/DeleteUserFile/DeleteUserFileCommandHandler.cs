using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.UserFileAgg;
using MediatR;

namespace Coreapi.Application.Features.User.Commands.DeleteUserFile
{
    internal class DeleteUserFileCommandHandler:IRequestHandler<DeleteUserFileCommand>
    {
        private readonly IUserFileRepository _userFileRepository;
        private readonly IS3Service _s3Service;


        public DeleteUserFileCommandHandler(IUserFileRepository userFileRepository, IS3Service s3Service)
        {
            _userFileRepository = userFileRepository;
            _s3Service = s3Service;
        }
        public async Task Handle(DeleteUserFileCommand request, CancellationToken cancellationToken)
        {
            var userFile = await _userFileRepository.GetById(request.Id);
            if (userFile is null) throw new NotAllowedException("فایل وجود ندراد");

            await _s3Service.DeleteFile("Upload/UserFiles/" + userFile.UserId + "/" + userFile.FileName);
            _userFileRepository.Delete(userFile);
            await _userFileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
