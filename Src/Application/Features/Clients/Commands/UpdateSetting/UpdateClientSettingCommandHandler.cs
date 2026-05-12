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

namespace Coreapi.Application.Features.Clients.Commands.UpdateSetting
{
    public class UpdateClientSettingCommandHandler : IRequestHandler<UpdateClientSettingCommand>
    {
        private readonly IClientRepository repository;
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;

        public UpdateClientSettingCommandHandler(IClientRepository repository, IUserManager userManager, ICurrentUser currentUser)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.currentUser = currentUser;
        }

        public async Task Handle(UpdateClientSettingCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithSetting(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            if (client.Setting is null)
                client.SetStting(new ClientSetting(request.InMessage, request.OutMessage, request.BreakInMessage, request.BreakOutMessage
                    , request.AllowedBreak, request.AllowedWorkHour, request.AllowedOvertime, request.AutoOutCycle, request.FirstOffDay, request.SecondOffDay));
            else
                client.Setting.Update(request.InMessage, request.OutMessage, request.BreakInMessage, request.BreakOutMessage
                    , request.AllowedBreak, request.AllowedWorkHour, request.AllowedOvertime, request.AutoOutCycle, request.FirstOffDay, request.SecondOffDay);

            var users = await userManager.GetClientUsersByAdministrator(currentUser.UserId);

            foreach (var clientUser in users)
            {
                var userSetting = client.UserSettings.FirstOrDefault(u => u.UserId.Equals(clientUser.Id));
                if (userSetting is null)
                    client.AddUserSetting(new ClientUserSetting(clientUser.Id, request.FirstOffDay, request.SecondOffDay));
                else if (userSetting.Type == Coreapi.Common.Enums.UserSettingTypeEnum.Default)
                    userSetting.Update(request.FirstOffDay, request.SecondOffDay);
            }

            repository.Update(client);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
