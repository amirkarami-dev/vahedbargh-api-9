using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetSetting
{
    public class GetClientSettingQueryHandler : IRequestHandler<GetClientSettingQuery, SettingDto>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;
        private readonly IMapper mapper;

        public GetClientSettingQueryHandler(IClientRepository repository, ICurrentUser currentUser
            , IMapper mapper)
        {
            this.repository = repository;
            this.currentUser = currentUser;
            this.mapper = mapper;
        }

        public async Task<SettingDto> Handle(GetClientSettingQuery request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithSetting(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            if (client.Setting is null)
                return null;

            var setting = mapper.Map<SettingDto>(client.Setting);

            var userSetting = client.UserSettings.FirstOrDefault(u => u.UserId.Equals(currentUser.UserId));

            setting.FirstOffDay = userSetting is null ? client.Setting.DefaultFirstOffDay : userSetting.FirstOffDay;
            setting.SecondOffDay = userSetting is null ? client.Setting.DefaultSecondOffDay : userSetting.SecondOffDay;

            return setting;
        }
    }
}
