using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Commands.AddUser
{
    public class AddClientUserCommandHandler : IRequestHandler<AddClientUserCommand, string>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository repository;

        public AddClientUserCommandHandler(IUserManager userManager, ICurrentUser currentUser, IClientRepository repository)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
            this.repository = repository;
        }

        public async Task<string> Handle(AddClientUserCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithSettingAndPackage(Guid.Parse(currentUser.ClientId));
           
            var userCount = await userManager.GetClientUsersCount(currentUser.UserId, string.Empty, new List<string>());
            
         

            var userId = await userManager.CreateUserAsync(currentUser.ClientId, null, request.Password, request.Email, Coreapi.Common.Enums.CurrentUserTypeEnum.JWT, request.FirstName, request.LastName,request.PhoneNumber,false,request.NickName);
            if (string.IsNullOrEmpty(userId))
            {
                string[] errors = { "Have Problem!" };
                throw new AuthenticationException(Errors: errors);
            }
            await userManager.AddToRolesAsync(userId, new List<string>() { request.Role });

            
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            if(request.SendEmail)
            { 
                //Todo: Send Notif Email
            }

            return userId;

        }
    }
}
