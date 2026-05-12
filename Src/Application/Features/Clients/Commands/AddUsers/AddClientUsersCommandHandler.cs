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

namespace Coreapi.Application.Features.Clients.Commands.AddUsers
{
    public class AddClientUsersCommandHandler : IRequestHandler<AddClientUsersCommand, UsersOutput>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository repository;

        public AddClientUsersCommandHandler(IUserManager userManager, ICurrentUser currentUser, IClientRepository repository)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
            this.repository = repository;
        }

        public async Task<UsersOutput> Handle(AddClientUsersCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithPackages(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var userCount = await userManager.GetClientUsersCount(currentUser.UserId, string.Empty, new List<string>());
           

            var output = new UsersOutput
            {
                SyncedUsers = new List<UserOutput>(),
                NotSyncedUsers = new List<UserOutput>()
            };

            string defaultPass = "Abcd@1234";
            var defaultRole = new List<string> { "Employee" };

            foreach (var item in request.Users)
            {
                var userId = await userManager.CreateUserAsync(currentUser.ClientId, item.Id, defaultPass, item.Email
                    , Coreapi.Common.Enums.CurrentUserTypeEnum.Rocket, item.FirstName, item.LastName);
                
                if (string.IsNullOrEmpty(userId))
                {
                    output.NotSyncedUsers.Add(new UserOutput
                    {
                        Email = item.Email,
                        FirstName = item.FirstName,
                        LastName = item.LastName
                    });
                    continue;
                }

                await userManager.AddToRolesAsync(userId, defaultRole);

                output.SyncedUsers.Add(new UserOutput
                {
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName
                });
            }

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return output;
        }
    }
}
