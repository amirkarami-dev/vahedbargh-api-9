using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using System;
using AutoMapper;
using Coreapi.Common.Utility;

namespace Coreapi.Application.Features.Supports.Queries.GetListSupport;

public class ListSupportDto: IMapFrom<Support>
{

    public Guid Id { get; init; }
    public int TicketNumber { get; set; }
    public string UserId { get; set; }
    public string ToUserId { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string ToName { get; set; }
    public UserType UserType { get; set; }
    public string UserTypeName { get; set; }
    public string SolarCreate { get; set; }
    public DateTime JulianCreate { get; set; }
    public string SolarEndSupport { get; set; }
    public DateTime? JulianEndSupport { get; set; }
    public string Title { get; set; }
    public long FileNumber { get; set; }
    public int Rate { get; set; }
    public bool IsRead { get; set; }
    public bool Closed { get; set; }
    public string Field1 { get; set; }
    public string Field2 { get; set; }
    public int UnreadMessageCount { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Support,ListSupportDto>()
            .ForMember(destination => destination.UserTypeName,
                otp => otp.MapFrom(source => source.UserType.GetDisplayName()))
            .ForMember(destination => destination.UnreadMessageCount,
                opt => opt.Ignore()); // این فیلد را manual set می‌کنیم
    }
}