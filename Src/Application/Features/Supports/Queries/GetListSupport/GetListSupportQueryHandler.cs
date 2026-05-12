using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using MediatR;

namespace Coreapi.Application.Features.Supports.Queries.GetListSupport;

public class GetListSupportQueryHandler(
    IMapper mapper,
    IClientRepository clientRepository,
    ICurrentUser currentUser,
    ISupportRepository supportRepository,
    ISupportMessageRepository supportMessageRepository)
    : IRequestHandler<GetListSupportQuery, IEnumerable<ListSupportDto>>
{
    public async Task<IEnumerable<ListSupportDto>> Handle(GetListSupportQuery request, CancellationToken cancellationToken)
    {
        // Validate client
        var client = await clientRepository.GetWithSetting(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        // Determine if current user is an Engineer
        var isEngineer = currentUser.Role.Contains("Engineer");

        // Override request parameters for Engineer role
        var supportListTypeEnum = isEngineer ? SupportListTypeEnum.All : request.SupportListTypeEnum;
        var closedFilter = request.Closed;

        // Get supports using the new repository method
        var supports = await supportRepository.GetSupportList(
            closedFilter,
            currentUser.UserId,
            request.TicketNumber,
            request.SearchUserId,
            request.UserType, 
            isEngineer, 
            supportListTypeEnum
            );



        // Map to DTO
        var supportDtos = mapper.Map<IEnumerable<ListSupportDto>>(supports).ToList();

      // بهینه‌سازی: دریافت تعداد پیام‌های خوانده نشده برای همه Supportها در یک Query
    var supportIds = supportDtos.Select(s => s.Id).ToList();
     var unreadCounts = await supportMessageRepository
      .GetUnreadMessageCountsBySupportIds(supportIds, currentUser.UserId);

        // Set unread message count for each support
        foreach (var supportDto in supportDtos)
        {
// اگر برای این Support پیام خوانده نشده وجود داشت، مقدار را set کن، وگرنه 0 بگذار
            supportDto.UnreadMessageCount = unreadCounts.TryGetValue(supportDto.Id, out var count) ? count : 0;
        }

        return supportDtos;
    }
}