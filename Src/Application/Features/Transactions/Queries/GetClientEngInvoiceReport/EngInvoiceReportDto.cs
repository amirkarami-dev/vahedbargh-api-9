using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Application.Features.Transactions.Queries.GetEngClientInvoices;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Transactions.Queries.GetClientEngInvoiceReport
{
    public class EngInvoiceReportDto : IMapFrom<Invoice>
    {
        public Guid Id { get; set; }
        public long FileNumber { get; set; }
        public DateTime JulianRegisterDate { get; set; }
        public string SolarRegisterDate { get; set; }
        public long Amount { get; set; }
        public string TransactionSolarCreated { get; set; }
        public DateTime TransactionJulianCreated { get; set; }
        public string TransactionDes { get; set; }
        public int QuarterTariffYear { get; set; }
        public int QuarterTypeEnum { get; set; }
        public int AllotmentRoundTypeEnum { get; set; }
        public DateTime JulianAllotmentDate { get; set; }
        public string SolarAllotmentDateS { get; set; }
        public InvoiceStatusEnum InvoiceStatusEnum { get; set; }
        public string InvoiceStatusName { get; set; }
        public InvoicePayTypeEnum InvoicePayTypeEnum { get; set; }
        public string InvoicePayTypeName { get; set; }
        public string SolarDateDeliverOffice { get; set; }
        public int SortIndex { get; set; }
        public Engineer Engineer { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Invoice, EngInvoiceReportDto>()
                .ForMember(destination => destination.InvoiceStatusName,
                    opt => opt.MapFrom(source => Enum.GetName(source.InvoiceStatus)))
                .ForMember(destination => destination.InvoicePayTypeName, opt =>
                    opt.MapFrom(source => Enum.GetName(source.InvoicePayType)))
                .ForMember(destination => destination.QuarterTariffYear,
                    otp => otp.MapFrom(source => source.ElectProjectProcess.QuarterTariff.Year))
                .ForMember(destination => destination.QuarterTypeEnum,
                    otp => otp.MapFrom(source => source.ElectProjectProcess.QuarterTariff.QuarterTypeEnum))
                .ForMember(destination => destination.AllotmentRoundTypeEnum,
                    otp => otp.MapFrom(source => source.ElectProjectProcess.QuarterTariff.AllotmentRoundTypeEnum))
                .ForMember(destination => destination.JulianAllotmentDate,
                    otp => otp.MapFrom(source => source.ElectProjectProcess.QuarterTariff.JulianAllotmentDate))
                .ForMember(destination => destination.SolarAllotmentDateS,
                    otp => otp.MapFrom(source => source.ElectProjectProcess.QuarterTariff.SolarAllotmentDate))
                .ForMember(destination => destination.TransactionSolarCreated,
                    otp => otp.MapFrom(source => source.Transaction.SolarCreated))
                .ForMember(destination => destination.TransactionJulianCreated,
                    otp => otp.MapFrom(source => source.Transaction.JulianCreated))
                .ForMember(destination => destination.TransactionDes,
                    otp => otp.MapFrom(source => source.Transaction.Des))
                .ForMember(destination => destination.FileNumber,
                    otp => otp.MapFrom(source => source.ElectProject.FileNumber))
   
                .ForMember(destination => destination.SolarRegisterDate,
                    otp => otp.MapFrom(source => source.ElectProject.SolarRegisterDate))
                .ForMember(destination => destination.JulianRegisterDate,
                    otp => otp.MapFrom(source => source.ElectProject.JulianRegisterDate))
      
                .ForMember(destination => destination.SolarDateDeliverOffice,
                    otp => otp.MapFrom(source => source.ElectProjectProcess.SolarDateDeliverOffice))
                .ForMember(destination => destination.Engineer,
                    otp => otp.MapFrom(source => source.ElectProjectProcess.Engineer))
                .ForMember(destination => destination.SortIndex,
                    otp => otp.MapFrom(source => source.ElectProjectProcess.Engineer.SortIndex))


                ;
        }
    }
}
