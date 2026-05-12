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

namespace Coreapi.Application.Features.Clients.Commands.UpdateUserSetting
{
    public class UpdateClientUserSettingCommandHandler : IRequestHandler<UpdateClientUserSettingCommand>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;

        public UpdateClientUserSettingCommandHandler(IClientRepository repository, ICurrentUser currentUser)
        {
            this.repository = repository;
            this.currentUser = currentUser;
        }

        public async Task Handle(UpdateClientUserSettingCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithSetting(Guid.Parse(currentUser.ClientId));
            if (client is null || client.Setting is null)
                throw new BadRequestException("No Setting Provided");

            var userSetting = client.UserSettings.FirstOrDefault(ur => ur.UserId.Equals(request.UserId));

            if (request.IsCustom)
            {
                if (userSetting is null)
                    client.AddUserSetting(new ClientUserSetting(request.UserId, request.FirstOffDay, request.SecondOffDay));
                else
                {
                    userSetting.Update(request.FirstOffDay, request.SecondOffDay);
                    userSetting.UpdateType(Coreapi.Common.Enums.UserSettingTypeEnum.Custom);
                }
            }
            else
            {
                if (userSetting is null)
                    client.AddUserSetting(new ClientUserSetting(request.UserId, client.Setting.DefaultFirstOffDay, client.Setting.DefaultSecondOffDay));
                else
                {
                    userSetting.Update(client.Setting.DefaultFirstOffDay, client.Setting.DefaultSecondOffDay);
                    userSetting.UpdateType(Coreapi.Common.Enums.UserSettingTypeEnum.Default);
                }
            }

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
