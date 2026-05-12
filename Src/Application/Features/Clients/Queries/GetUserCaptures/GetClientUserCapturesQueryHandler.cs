using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using Coreapi.Common.Models;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetUserCaptures;

public class GetClientUserCapturesQueryHandler : IRequestHandler<GetClientUserCapturesQuery, PaggingList<UserCaptureDto>>
{
    private readonly IClientRepository repository;
    private readonly ICurrentUser currentUser;
    private readonly IUserManager userManager;

    public GetClientUserCapturesQueryHandler(IClientRepository repository, ICurrentUser currentUser, IUserManager userManager)
    {
        this.repository = repository;
        this.currentUser = currentUser;
        this.userManager = userManager;
    }

    public async Task<PaggingList<UserCaptureDto>> Handle(GetClientUserCapturesQuery request, CancellationToken cancellationToken)
    {
        var captures = await repository.GetCaptures(Guid.Parse(currentUser.ClientId), request.UserId, request.PageNumber, request.RowCount);

        var userIds = captures.Select(t => t.UserId);

        List<UserData> users = new();
        if (userIds.Any())
            users = await userManager.GetUsers(userIds);

        return new PaggingList<UserCaptureDto>
        {
            Data = captures.Select(capture => new UserCaptureDto
            {
                Id = capture.Id,
                UserId = capture.UserId,
                EventType = capture.EventType,
                Latitude = capture.Latitude,
                Longitude = capture.Longitude,
                RefId = capture.RefId,
                RefType = capture.RefType,
                Status = capture.Status,
                Avatar = users.FirstOrDefault(u => u.Id.Equals(capture.UserId))?.Avatar ?? string.Empty,
                FirstName = users.FirstOrDefault(u => u.Id.Equals(capture.UserId))?.FirstName ?? string.Empty,
                LastName = users.FirstOrDefault(u => u.Id.Equals(capture.UserId))?.LastName ?? string.Empty,
                Created = capture.Created.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
            }).ToList(),
            TotalItems = await repository.CapturesCount(Guid.Parse(currentUser.ClientId), request.UserId)
        };

    }
}
