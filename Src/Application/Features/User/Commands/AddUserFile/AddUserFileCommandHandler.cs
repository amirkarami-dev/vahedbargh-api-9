using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.UserFileAgg;
using MediatR;

namespace Coreapi.Application.Features.User.Commands.AddUserFile
{
    public class AddUserFileCommandHandler:IRequestHandler<AddUserFileCommand,int>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IS3Service _s3Service;

        private readonly IUserFileRepository _userFileRepository;

        public AddUserFileCommandHandler(IClientRepository clientRepository, ICurrentUser currentUser, IS3Service s3Service, IUserFileRepository userFileRepository)
        {
            _clientRepository = clientRepository;
            _currentUser = currentUser;
            _s3Service = s3Service;
            _userFileRepository = userFileRepository;
        }
        public async Task<int> Handle(AddUserFileCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetById(Guid.Parse(_currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), _currentUser.ClientId);

            }
            if (request.File is null) throw new NotAllowedException("فایل آپلود نشده است");
            if (string.IsNullOrEmpty(request.UserId) ) throw new NotAllowedException("کاربری وجود ندارد");

            var userFiles = await _userFileRepository.GetUserFilesByUserId(request.UserId);
            var userFileExist = userFiles.FirstOrDefault(c =>
                c.Title == request.Title && c.FileName == request.FileName &&
                c.UserFileTypeEnum == request.UserFileTypeEnum);
            if (userFileExist == null)
            {
                await _s3Service.UploadFileAttach(request.File,request.FileName, request.FolderName, request.UserId);

                var userFile = new UserFile(
                    request.Name,
                    request.Des,
                    request.Title,
                    request.UserFileTypeEnum,
                    request.UserId,
                    request.FileName,
                    request.UserId,
                    request.ToUserId,
                    client);
                _userFileRepository.Add(userFile);


                await _userFileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NotAllowedException("نوع ،نام و موضوع فایل تکراری می باشد");
            }

          


            return 1;
        }
    }
}
