using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg
{
    public interface IBankTransactionRepository:IRepository<BankTransaction>
    {
        Task<BankTransaction> GetByPaymentIdAndToken(string paymentId, string token);
    }
}
