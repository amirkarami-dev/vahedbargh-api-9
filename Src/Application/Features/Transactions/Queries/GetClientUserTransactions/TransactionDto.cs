using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Application.Common.Models;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.FinanceAgg;

namespace Coreapi.Application.Features.Transactions.Queries.GetClientUserTransactions
{
    public class TransactionDto:IMapFrom<TransactionViewModel>
    {
        public Guid Id { get; set; }
        public long Amount { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public GatewayTypeEnum GatewayType { get; set; }
        public string GatewayTypeName { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
        public TransactionStatusEnum TransactionStatus { get; set; }
        public string TransactionStatusName { get; set; }
        public DateTime JulianCreated { get; set; }
        public string SolarCreated { get; set; }
        public string BankTransactionId { get; set; }
        public string Des { get; set; }
        public string FileNumber { get; set; }
        public UserData userData { get; set; }
        public int IdCity { get; set; }
        public int IdSection { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TransactionViewModel, TransactionDto>()
                .ForMember(destination => destination.GatewayTypeName,
                    opt => opt.MapFrom(source => Enum.GetName(source.GatewayType)))
                .ForMember(destination => destination.TransactionTypeName,
                    opt=>opt.MapFrom(source=> Enum.GetName(source.TransactionType)))
                .ForMember(destination=>destination.TransactionStatusName,
                    opt=>opt.MapFrom(source=> Enum.GetName(source.TransactionStatus)))
                ;
        }
    }
}
