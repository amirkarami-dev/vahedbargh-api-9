using System;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Domain.AggregatesModel.FinanceAgg;

namespace Coreapi.Application.Features.Transactions.Queries.GetEngPaymentTasks;

public class EngPaymentTaskDto:IMapFrom<EngPaymentTask>
{
    public Guid Id { get; set; }
    public string SolarCreated { get; set; }
    public int Period { get; set; }
    public string FromSolar { get; set; }
    public string ToSolar { get; set; }
    public string SolarPay { get; set; }
    public string Des { get; set; }
    public string FilePaymentName { get; set; }
    public bool Approved { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EngPaymentTask, EngPaymentTaskDto>()
            .ForMember(destination => destination.SolarPay, opt => opt.MapFrom(source => source.SolarPay??"پرداخت نشده"))

            ;
    }
}