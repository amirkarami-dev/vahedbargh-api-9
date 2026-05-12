using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Clients.Commands.UpdateUser
{
    public class UpdateClientUserCommandHandler : IRequestHandler<UpdateClientUserCommand>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;
        private readonly IFileService fileService;

        public UpdateClientUserCommandHandler(IUserManager userManager, ICurrentUser currentUser, IFileService fileService)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
            this.fileService = fileService;
        }

        public async Task Handle(UpdateClientUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserAsync(request.Id);
            if (user is null)
                throw new NotFoundException("User", request.Id);

            if (!user.ClientId.Equals(currentUser.ClientId))
                throw new BadRequestException("Not Your User");

            //string file = user.Avatar;
            //if (request.Avatar is not null)
            //{
            //    file = await fileService.UploadAvatar(request.Avatar);
            //}

            await userManager.UpdateUser(request.Id, request.FirstName, request.LastName,request.NickName,request.PhoneNumber,request.Active);

            //if (request.Role.Any())
            //{
                await userManager.RemoveFromRoleAsync(request.Id);
                await userManager.AddToRolesAsync(request.Id, new List<string>() { request.Role });
                //await userManager.AddToRolesAsync(request.SearchUserId, request.Roles);
           // }

        }
    }
}
