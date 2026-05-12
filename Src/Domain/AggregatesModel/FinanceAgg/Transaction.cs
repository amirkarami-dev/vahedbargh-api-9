using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg
{
    public class Transaction
    {
        
        private Transaction(){}

        public Transaction(long amount, Client client, string userId, GatewayTypeEnum gatewayType, TransactionTypeEnum transactionType, TransactionStatusEnum transactionStatus, DateTime julianCreated, string solarCreated, string bankTransactionId, string des, string projectId)
        {
            Id = Guid.NewGuid();
            Amount = amount;
            Client = client;
            UserId = userId;
            GatewayType = gatewayType;
            TransactionType = transactionType;
            TransactionStatus = transactionStatus;
            JulianCreated = julianCreated;
            SolarCreated = solarCreated;
            BankTransactionId = bankTransactionId;
            Des = des;
            ProjectId = projectId;
        }

        public void UpdateAmount(long amount)
        {
            Amount = amount;
        }

        public void UpdateDes(string des)
        {
            Des = des;
        }
        public Guid Id { get; init; }
        public long Amount { get; private set; }
        public Client Client { get; init; }
        public string UserId { get; private set; }
        public string ProjectId { get; set; }
        public GatewayTypeEnum GatewayType { get; init; }
        public TransactionTypeEnum TransactionType { get; private set; }
        public TransactionStatusEnum TransactionStatus { get; init; }
        public DateTime JulianCreated { get; init; }
        public string SolarCreated { get; init; }
        public string BankTransactionId { get; private set; }
        public string Des { get; private set; }
        public Invoice Invoice { get; private set; }
        public EngPaymentList EngPaymentList { get; private set; }


    }
}
