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

namespace Coreapi.Application.Features.Clients.Commands.SyncMicrosoftUsers
{
    public class SyncMicrosoftUsersCommandHandler : IRequestHandler<SyncMicrosoftUsersCommand, UsersOutput>
    {
        private readonly IClientRepository repository;
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;
        private readonly IMicrosoftService microsoftService;

        public SyncMicrosoftUsersCommandHandler(IClientRepository repository, IUserManager userManager, ICurrentUser currentUser,
            IMicrosoftService microsoftService)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.currentUser = currentUser;
            this.microsoftService = microsoftService;
        }

        public async Task<UsersOutput> Handle(SyncMicrosoftUsersCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithPackages(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var result = await microsoftService.GetUsers(request.Token);

            if (result is null)
                throw new BadRequestException("UnsuccessfulSync");

            var userCount = await userManager.GetClientUsersCount(currentUser.UserId, string.Empty, new List<string>());
        
            var output = new UsersOutput
            {
                SyncedUsers = new List<UserOutput>(),
                NotSyncedUsers = new List<UserOutput>()
            };

            string defaultPass = "Abcd@1234";
            var defaultRole = new List<string> { "Employee" };

            foreach (var item in result)
            {
                if (item.Email is null)
                    continue;

                if (item.Firstname is null)
                    item.Firstname = item.Email.Split('@').First();

                var userId = await userManager.CreateUserAsync(currentUser.ClientId, item.Id, defaultPass, item.Email
                    , Coreapi.Common.Enums.CurrentUserTypeEnum.Rocket, item.Firstname, item.Lastname);
                if (string.IsNullOrEmpty(userId))
                {
                    output.NotSyncedUsers.Add(new UserOutput
                    {
                        Email = item.Email,
                        FirstName = item.Firstname,
                        LastName = item.Lastname
                    });
                    continue;
                }

                await userManager.AddToRolesAsync(userId, defaultRole);


                output.SyncedUsers.Add(new UserOutput
                {
                    Email = item.Email,
                    FirstName = item.Firstname,
                    LastName = item.Lastname
                });

            }

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return output;
        }
    }
}
