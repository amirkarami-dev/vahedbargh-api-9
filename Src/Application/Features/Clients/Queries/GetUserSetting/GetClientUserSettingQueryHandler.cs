using AutoMapper;
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

namespace Coreapi.Application.Features.Clients.Queries.GetUserSetting
{
    public class GetClientUserSettingQueryHandler : IRequestHandler<GetClientUserSettingQuery, UserSettingDto>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;
        private readonly IMapper mapper;

        public GetClientUserSettingQueryHandler(IClientRepository repository, ICurrentUser currentUser, IMapper mapper)
        {
            this.repository = repository;
            this.currentUser = currentUser;
            this.mapper = mapper;
        }

        public async Task<UserSettingDto> Handle(GetClientUserSettingQuery request, CancellationToken cancellationToken)
        {
            if(!currentUser.Role.Equals("Administrator"))
                request.UserId = currentUser.UserId;

            var client = await repository.GetWithSetting(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var userSetting = client.UserSettings.FirstOrDefault(ur => ur.UserId.Equals(request.UserId));

            return mapper.Map<UserSettingDto>(userSetting);
        }
    }
}
