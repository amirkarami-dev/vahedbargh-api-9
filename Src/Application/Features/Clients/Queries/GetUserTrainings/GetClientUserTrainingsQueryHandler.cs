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

namespace Coreapi.Application.Features.Clients.Queries.GetUserTrainings
{
    public class GetClientUserTrainingsQueryHandler : IRequestHandler<GetClientUserTrainingsQuery, IEnumerable<UserTrainingDto>>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;

        public GetClientUserTrainingsQueryHandler(IClientRepository repository, ICurrentUser currentUser)
        {
            this.repository = repository;
            this.currentUser = currentUser;
        }

        public async Task<IEnumerable<UserTrainingDto>> Handle(GetClientUserTrainingsQuery request, CancellationToken cancellationToken)
        {
            string userId = currentUser.Role.Contains("Analyzer") ? request.UserId : currentUser.UserId;
            Guid clientId = currentUser.Role.Contains("Analyzer") ? request.ClientId : Guid.Parse(currentUser.ClientId);
            var client = await repository.GetWithTrainings(request.ClientId);
            if (client is null)
                throw new NotFoundException(nameof(Client), request.ClientId);

            return client.UserTrainings.Where(u => u.UserId.Equals(userId)).Select(c => new UserTrainingDto
            {
                ClientId = clientId,
                UserId = c.UserId,
                Id = c.Id,
                Link = c.Link,
                Title = c.Title
            });
        }
    }
}
