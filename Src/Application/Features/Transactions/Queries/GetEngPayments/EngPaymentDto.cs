using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using System;
using System.Collections.Generic;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using System.ComponentModel;
using Coreapi.Common.ViewModels;

namespace Coreapi.Application.Features.Transactions.Queries.GetEngPayments;

public class EngPaymentDto:IMapFrom<EngPaymentList>
{
    public Guid Id { get; init; }
    public Engineer Engineer { get; set; }
    public string FullName { get; set; }
    public string SolarCreated { get; set; }
    public int Period { get; set; }
    public string FromSolar { get; set; }
    public string ToSolar { get; set; }
    public Transaction Transaction { get; set; }
    public string SolarPay { get; set; }
    public string Des { get; set; }
    public string FilePaymentName { get; set; }
    public bool Approved { get; set; }
    public int sortIndex { get; set; }

    [Description("this is amount result of system")]
    public long AmountSystem { get; set; }
    [Description("deduction 5 % sazman")]
    public long Deduction1 { get; set; }
    [Description("Deduction 1% sandogh ")]
    public long Deduction2 { get; set; }
    [Description("Deduction 7% vahed bargh")]
    public long Deduction3 { get; set; }
    [Description("Deduction 10% afzodeh ")]
    public long Deduction4 { get; set; }
    [Description("this is fish Addition")]
    public long Addition1 { get; set; }
    [Description("this is Other Addition")]
    public long Addition2 { get; set; }
    public long SumAmountSystem { get; set; }
    public long SumAmountWithFish { get; set; }
    public bool PayByBankReceipt { get; set; }
    public IEnumerable<Invoice> Invoices { get; set; }
    public IEnumerable<Transaction> TransactionIn { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EngPaymentList, EngPaymentDto>()
            .ForMember(destination => destination.FullName, opt => opt.MapFrom(source => source.Engineer.FullName))
            .ForMember(destination => destination.SolarCreated, opt => opt.MapFrom(source => source.EngPaymentTask.SolarCreated))
            .ForMember(destination => destination.Period, opt => opt.MapFrom(source => source.EngPaymentTask.Period))
            .ForMember(destination => destination.FromSolar, opt => opt.MapFrom(source => source.EngPaymentTask.FromSolar))
            .ForMember(destination => destination.ToSolar, opt => opt.MapFrom(source => source.EngPaymentTask.ToSolar))
            .ForMember(destination => destination.SolarPay, opt => opt.MapFrom(source => source.EngPaymentTask.SolarPay))
            .ForMember(destination => destination.Des, opt => opt.MapFrom(source => source.EngPaymentTask.Des))
            .ForMember(destination => destination.FilePaymentName, opt => opt.MapFrom(source => source.EngPaymentTask.FilePaymentName))
            .ForMember(destination => destination.Approved, opt => opt.MapFrom(source => source.EngPaymentTask.Approved))
            .ForMember(destination => destination.sortIndex, opt => opt.MapFrom(source => source.Engineer.SortIndex))
            ;
    }

}