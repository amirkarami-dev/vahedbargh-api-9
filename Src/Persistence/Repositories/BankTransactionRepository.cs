using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class BankTransactionRepository:BaseRepository<BankTransaction>,IBankTransactionRepository
    {
        public BankTransactionRepository(CoreapiDbContext context) : base(context)
        {
        }


        public async Task<BankTransaction> GetByPaymentIdAndToken(string paymentId, string token)
        {

            return await context.BankTransactions.Where(c => c.PaymentId == paymentId && c.Token == token).Include(e=>e.Client).FirstOrDefaultAsync();
        }
    }
}
