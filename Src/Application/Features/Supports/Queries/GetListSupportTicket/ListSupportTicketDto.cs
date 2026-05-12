using Coreapi.Domain.AggregatesModel.SupportAgg;
using System;
using AutoMapper;
using Coreapi.Application.Common.Mappings;

namespace Coreapi.Application.Features.Supports.Queries.GetListSupportTicket;

public class ListSupportTicketDto:IMapFrom<SupportMessage>
{
    public Guid Id { get; init; }
    public string Message { get; set; }
    public string SolarCreated { get; set; }
    public DateTime JulianCreated { get; set; }
    public string UserId { get; set; }
    public string FileName { get; set; }
    public string ToUserId { get; set; }
    public string Name { get; set; }
    public string ToName { get; set; }
    public bool IsSend { get; set; }
    public bool IsReceive { get; set; }
    public bool IsReadByReceiver { get; set; }
    public Support Support { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SupportMessage, ListSupportTicketDto>();
    }
}