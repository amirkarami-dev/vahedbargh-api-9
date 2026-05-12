using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg
{
    public class Invoice
    {
        private Invoice(){}

        public Invoice(Client client, ElectProject electProject, long amount, InvoiceStatusEnum invoiceStatus, InvoicePayTypeEnum invoicePayType)
        {
            Id = Guid.NewGuid();
            Client = client;
            ElectProject = electProject;
            ElectProjectProcess = null;
            Amount = amount;
            InvoiceStatus = InvoiceStatusEnum.Pending;
            InvoicePayType = invoicePayType;
            JulianCreated = DateTime.Now.Date;
            SolarCreated = Helper.MiladiToShamsi(DateTime.Now.Date);

        }


        public void Done(Transaction transaction)
        {
            if (transaction is null)
                throw new ArgumentNullException(nameof(transaction));

            Transaction = transaction;
            InvoiceStatus = InvoiceStatusEnum.Done;
        }

        public void Cancel()
        {
            InvoiceStatus = InvoiceStatusEnum.Cancelled;
        }

        public void UpdateElectProjectProcess(ElectProjectProcess electProjectProcess)
        {
            ElectProjectProcess = electProjectProcess ?? throw new ArgumentNullException(nameof(electProjectProcess));
        }

        public void UpdateInvoicePayType(InvoicePayTypeEnum invoicePayType)
        {
            InvoicePayType = invoicePayType;
        }

        public void UpdateAmount(long amount)
        {
            Amount = amount;
        }
        public void UpdateAmountSupervision(long amount)
        {
            AmountSupervision = amount;
        }

        public Guid Id { get; init; }
        public DateTime JulianCreated { get; set; }
        public string SolarCreated { get; set; }
        public Client Client { get; init; }
        public ElectProject ElectProject { get; init; }
        public ElectProjectProcess ElectProjectProcess { get; private set; }
        public long Amount { get; private set; }
        public long AmountSupervision { get; set; }
        public InvoiceStatusEnum InvoiceStatus { get; private set; }
        public InvoicePayTypeEnum InvoicePayType { get; private set; }
        public Transaction Transaction { get; private set; }



    }
}
