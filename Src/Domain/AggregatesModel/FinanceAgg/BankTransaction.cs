using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg
{
    public class BankTransaction
    {
        private BankTransaction(){}

        public BankTransaction(
            GatewayTypeEnum gatewayTypeEnum, Client client, string userId, 
            PaymentTypeEnum paymentTypeEnum, string token, string paymentId, 
            string projectId, long amount, string solarLocalDateTime
             )
        {
            Id = Guid.NewGuid();
            GatewayTypeEnum = gatewayTypeEnum;
            Client = client;
            UserId = userId;
            PaymentTypeEnum = paymentTypeEnum;
            Token = token;
            PaymentId = paymentId;
            ProjectId = projectId;
            Amount = amount;
            SolarLocalDateTime = solarLocalDateTime;
            Confirm = false;
        }

        public void UpdateWhenReturn( 
            string acceptorId, string requestId, string retrievalReferenceNumber,string systemTraceAuditNumber,
            string maskedPan, string sha256OfPan, string description)
        {

            AcceptorId=acceptorId;
            RequestId=requestId;
            RetrievalReferenceNumber =retrievalReferenceNumber;
            SystemTraceAuditNumber = systemTraceAuditNumber;
            MaskedPan = maskedPan;
            Sha256OfPan = sha256OfPan;
            Description = description;
            Confirm = true;

        }



        public Guid Id { get; init; }
        public GatewayTypeEnum  GatewayTypeEnum { get; private set; }
        public Client Client { get; init; }
        public string UserId { get; private set; }
        public string ProjectId { get; set; }
        public PaymentTypeEnum PaymentTypeEnum { get; private set; }
        public string Token { get; private set; }
        public string PaymentId { get; private set; }
        public string RequestId { get; private set; }
        public long Amount { get; private set; }
        public string Description { get; private set; }
        public string AcceptorId { get; private set; }
        public string Sha256OfPan { get; private set; }
        public string ResDescription { get; private set; }
        public string RetrievalReferenceNumber { get; private set; }
        public string SystemTraceAuditNumber { get; private set; }
        public string MaskedPan { get; set; }
        public DateTime JulianLocalDateTime { get; private set; } = DateTime.Now;
        public string SolarLocalDateTime { get; private set; }
        public bool Confirm { get; private set; }

    }
}
