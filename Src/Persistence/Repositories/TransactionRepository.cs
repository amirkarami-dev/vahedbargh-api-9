using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.RequestModel;
using Coreapi.Common.Utility;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;


namespace Coreapi.Persistence.Repositories
{
    public class TransactionRepository(CoreapiDbContext context)
        : BaseRepository<Transaction>(context), ITransactionRepository
    {
        public async Task<long> GetBalance(string userId)
        {
            var query = context.Transactions.Where(t => t.UserId == userId);
            return (await query.Where(t => t.TransactionStatus == TransactionStatusEnum.In).SumAsync(t => t.Amount))
                   - (await query.Where(t => t.TransactionStatus == TransactionStatusEnum.Out).SumAsync(t => t.Amount));
        }

        public async Task<IEnumerable<UserBalanceViewModel>> GetBalances()
        {
            var query = await context.Transactions
                .GroupBy(c=>new
                {
                    c.UserId
                }).Select(c=> new UserBalanceViewModel()
                    {
                        UserId = c.Key.UserId,
                        Balance = c.Where(t=>t.TransactionStatus == TransactionStatusEnum.In).Sum(ss=>ss.Amount)-
                                  c.Where(t => t.TransactionStatus == TransactionStatusEnum.Out).Sum(ss => ss.Amount)
                    }).ToListAsync();

            return query;
        }

        public async Task<long> GetCountUserTransaction(string userId)
        {
            var count = await context.Transactions.Where(t => t.UserId == userId).CountAsync();
            return Convert.ToInt64(count);

        }

        public async Task<BaseModelWithTotalRow<TransactionViewModel>> GetClientUserTransactions(Guid clientId, string userId, int pageNumber, int rowCount)
        {
            var mainModel = new BaseModelWithTotalRow<TransactionViewModel>();

            var result = context.Transactions
                .Include(p => p.Invoice)
                .Join(
                    context.ElectProjects,
                    transaction => transaction.ProjectId,
                    electProject => electProject.Id.ToString(),
                    (transaction, electProject) => new { Transaction = transaction, ElectProject = electProject }
                )
                .Where(joinResult => joinResult.ElectProject.IsDelete == false)
                .Where(joinResult => joinResult.Transaction.UserId == userId)
                .Where(joinResult => EF.Property<Guid>(joinResult.Transaction, "ClientId") == clientId)
                .OrderByDescending(c => c.Transaction.JulianCreated)

                .Select(joinResult => new TransactionViewModel()
                {
                    Amount = joinResult.Transaction.Amount,
                    BankTransactionId = joinResult.Transaction.BankTransactionId,
                    Des = joinResult.Transaction.Des,
                    FileNumber = joinResult.ElectProject.FileNumber.ToString(),
                    GatewayType = joinResult.Transaction.GatewayType,
                    Id = joinResult.Transaction.Id,
                    ProjectId = joinResult.Transaction.ProjectId,
                    JulianCreated = joinResult.Transaction.JulianCreated,
                    SolarCreated = joinResult.Transaction.SolarCreated,
                    TransactionStatus = joinResult.Transaction.TransactionStatus,
                    TransactionStatusName = joinResult.Transaction.TransactionStatus.GetDisplayName(),
                    TransactionType = joinResult.Transaction.TransactionType,
                    TransactionTypeName = joinResult.Transaction.TransactionType.GetDisplayName(),
                    UserId = joinResult.Transaction.UserId,
                    IdCity = joinResult.ElectProject.IdCity,
                    IdSection = joinResult.ElectProject.IdSection,
                    GatewayTypeName = joinResult.Transaction.GatewayType.GetDisplayName()

                });


            var endResult = await result.ToListAsync();
            var endResultWithPagination = endResult.Skip(pageNumber * rowCount)
                .Take(rowCount);

            mainModel.AggregateModel = endResultWithPagination;
            mainModel.TotalItem = endResult.Count;
            return mainModel;


        }

        public async Task<BaseModelWithTotalRow<TransactionViewModel>> GetClientTransactions(Guid clientId, int pageNumber, int rowCount,long fileNumber, int idCity, TransactionStatusEnum transactionStatusEnum, DateTime? julianCreated)
        {
            
            var mainModel = new BaseModelWithTotalRow<TransactionViewModel>();

            var result1 = context.Transactions
                .Include(p => p.Invoice)
                .Join(
                    context.ElectProjects,
                    transaction => transaction.ProjectId,
                    electProject => electProject.Id.ToString(),
                    (transaction, electProject) => new { Transaction = transaction, ElectProject = electProject }
                )
                .Where(joinResult => joinResult.ElectProject.IsDelete == false)
                .Where(j=> fileNumber ==0 || j.ElectProject.FileNumber == fileNumber)
                .Where(join=>transactionStatusEnum == TransactionStatusEnum.None ||  join.Transaction.TransactionStatus == transactionStatusEnum)
                .Where(join=>julianCreated == null ||  join.Transaction.JulianCreated.Date == julianCreated)
                .Where(joinResult => EF.Property<Guid>(joinResult.Transaction, "ClientId") == clientId)
                .Where(joinResult => idCity == 0 || joinResult.ElectProject.IdCity == idCity)
                .OrderByDescending(c => c.Transaction.JulianCreated)
              
                .Select(joinResult => new TransactionViewModel()
                {
                    Amount = joinResult.Transaction.Amount,
                    BankTransactionId = joinResult.Transaction.BankTransactionId,
                    Des = joinResult.Transaction.Des,
                    FileNumber = joinResult.ElectProject.FileNumber.ToString(),
                    GatewayType = joinResult.Transaction.GatewayType,
                    Id = joinResult.Transaction.Id,
                    ProjectId = joinResult.Transaction.ProjectId,
                    JulianCreated = joinResult.Transaction.JulianCreated,
                    SolarCreated = joinResult.Transaction.SolarCreated,
                    TransactionStatus = joinResult.Transaction.TransactionStatus,
                    TransactionStatusName = joinResult.Transaction.TransactionStatus.GetDisplayName(),
                    TransactionTypeName = joinResult.Transaction.TransactionType.GetDisplayName(),
                    TransactionType = joinResult.Transaction.TransactionType,
                    UserId = joinResult.Transaction.UserId,
                    IdCity = joinResult.ElectProject.IdCity,
                    IdSection = joinResult.ElectProject.IdSection,
                    GatewayTypeName = joinResult.Transaction.GatewayType.GetDisplayName()

                });

            var endResult = await result1.ToListAsync();
            var endResultWithPagination = endResult.Skip(pageNumber * rowCount)
                .Take(rowCount);

            mainModel.AggregateModel = endResultWithPagination;
            mainModel.TotalItem = endResult.Count;
            return mainModel;
        }

        public async Task<IEnumerable<Invoice>> GetClientInvoices(Guid clientId, int pageNumber, int rowCount)
        {
            //return await context.Transactions.Include(p => p.Invoice).Include(p => p.Invoice.GasProject).Where(t => EF.Property<Guid>(t, "ClientId") == clientId)
            //    .Skip(pageNumber * rowCount).Take(rowCount).OrderByDescending(c => c.JulianCreated).ToListAsync();
            return await context.Invoices.Include(i => i.ElectProjectProcess)
                .Include(g=>g.ElectProject)
                .Include(iii=>iii.ElectProjectProcess.Engineer).Include(ii => ii.Transaction)
                .Include(ins=>ins.ElectProjectProcess.QuarterTariff)
                .Where(t => EF.Property<Guid>(t, "ClientId") == clientId).Skip(pageNumber * rowCount).Take(rowCount)
                .OrderByDescending(c => c.Transaction.JulianCreated).ToListAsync();
            //var dd = await (from t in context.Transactions join executor in context.Executors on t.UserId equals executor.UserId 
            //        select t).ToListAsync();
            //return  dd;
        }

        public async Task<IEnumerable<Invoice>> GetClientInvoices(Guid clientId, Guid engId, int pageNumber, int rowCount)
        {
            return await context.Invoices.Include(i => i.ElectProjectProcess)
                .Include(g => g.ElectProject)
                .Include(iii => iii.ElectProjectProcess.Engineer).Include(ii => ii.Transaction)
                .Include(ins => ins.ElectProjectProcess.QuarterTariff)
                .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == engId)
                .Skip(pageNumber * rowCount).Take(rowCount)
                .OrderByDescending(c => c.ElectProjectProcess.JulianDateDeliverOffice).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetClientEngInvoices(Guid clientId, DateTime startDate, DateTime endDate)
        {
            return await context.Invoices.Include(i => i.ElectProjectProcess)
                .Include(g => g.ElectProject)
                .Include(iii => iii.ElectProjectProcess.Engineer).Include(ii => ii.Transaction)
                .Include(ins => ins.ElectProjectProcess.QuarterTariff)
                .Where(t => EF.Property<Guid>(t, "ClientId") == clientId &&
                            t.ElectProjectProcess.JulianDateDeliverOffice.Value.Date >= startDate.Date &&
                            t.ElectProjectProcess.JulianDateDeliverOffice.Value.Date <= endDate.Date
                )
                .OrderBy(c => c.ElectProjectProcess.Engineer.SortIndex).ToListAsync();
        }

        public async Task<IEnumerable<EngPaymentViewModel>> GetClientEngPaymentGroup(Guid clientId, DateTime startDate, DateTime endDate)
        {
            var query = await context.Invoices
                .Include(i => i.ElectProjectProcess)
                .Include(iii => iii.ElectProjectProcess.Engineer)
                .Where(t => EF.Property<Guid>(t, "ClientId") == clientId &&
                            t.ElectProjectProcess.JulianDateAccepted.Value.Date >= startDate.Date &&
                            t.ElectProjectProcess.JulianDateAccepted.Value.Date <= endDate.Date
                )
                .GroupBy(c => new
                {
                    c.ElectProjectProcess.Engineer.Id
                }).Select(c => new EngPaymentViewModel()
                {
                    EngineerId = c.Key.Id,
                    Amount = c.Sum(ss => ss.Amount)
                }).ToListAsync();

            return query;
        }
        public async Task<IEnumerable<Invoice>> GetClientEngPaymentByEng(Guid clientId, DateTime startDate, DateTime endDate)
        {
            var query = await context.Invoices
                .Include(i => i.ElectProjectProcess)
                .Include(i=>i.Transaction)
                .Include(i=>i.ElectProject)
                .Include(iii => iii.ElectProjectProcess.Engineer)
                .Where(t => EF.Property<Guid>(t, "ClientId") == clientId &&
                            t.ElectProjectProcess.JulianDateAccepted.Value.Date >= startDate.Date &&
                            t.ElectProjectProcess.JulianDateAccepted.Value.Date <= endDate.Date
                ).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<Transaction>> GetClientTransactions(Guid clientId, TransactionStatusEnum transactionStatusEnum)
        {
            var query = await context.Transactions
                .Where(w=>w.TransactionStatus == transactionStatusEnum)
                .ToListAsync();

            return query;
        }

        public async Task<long> GetClientEngInvoiceSum(Guid clientId, Guid engId, Guid idQuarterTariff, string month)
        {
            var totalAmountInvoices = await context.Invoices.Include(i => i.ElectProjectProcess)
                .Include(i=>i.ElectProjectProcess.QuarterTariff)
                .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == engId && t.ElectProjectProcess.QuarterTariff.Id == idQuarterTariff)
                .Where(w => w.Transaction.SolarCreated.Substring(5,2).Contains(month))
                .SumAsync(c => c.Amount);
            return totalAmountInvoices;
        }

        public async Task<long> GetClientEngInvoiceSum(Guid clientId, Guid engId)
        {
            var totalAmountInvoices = await context.Invoices.Include(i => i.ElectProjectProcess)
                .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == engId)
                .SumAsync(c => c.Amount);
            return totalAmountInvoices;
        }

        public async Task<IEnumerable<Invoice>> GetClientEngInvoices(Guid clientId, Guid engId, DateTime startDate, DateTime endDate)
        {
            return await context.Invoices.Include(i => i.ElectProjectProcess)
                .Include(g => g.ElectProject)
                .Include(iii => iii.ElectProjectProcess.Engineer).Include(ii => ii.Transaction)
                .Include(ins => ins.ElectProjectProcess.QuarterTariff)
                .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == engId && t.ElectProjectProcess.JulianDateDeliverOffice.Value.Date >= startDate.Date && t.ElectProjectProcess.JulianDateDeliverOffice.Value.Date <= endDate.Date)
                .OrderBy(c => c.ElectProjectProcess.JulianDateDeliverOffice).ToListAsync();
        }

        public async Task<Transaction> GetByBankTransactionId(string btId)
        {
            return await context.Transactions.SingleOrDefaultAsync(c => c.BankTransactionId == btId.ToUpper());
        }

        public async Task<Transaction> GetByElectProjectId(string projectId, TransactionStatusEnum transactionStatusEnum)
        {
            return await context.Transactions.FirstOrDefaultAsync(s => s.ProjectId == projectId && s.TransactionStatus == transactionStatusEnum);
        }


        public async Task<bool> HasTransactionInForChild(List<string> childIds, TransactionStatusEnum transactionStatusEnum)
        {
            return await context.Transactions.AnyAsync(a => childIds.Contains(a.ProjectId) && a.TransactionStatus == transactionStatusEnum);
        }

        public async Task<long> GetProjectBalance(Guid electProjectId)
        {
            var query = context.Transactions.Where(t => t.ProjectId == electProjectId.ToString());
            return (await query.Where(t => t.TransactionStatus == TransactionStatusEnum.In).SumAsync(t => t.Amount))
                   - (await query.Where(t => t.TransactionStatus == TransactionStatusEnum.Out).SumAsync(t => t.Amount));


        }

		public async Task<long> GetBigProjectBalance(List<string> childIds)
		{
			var query = context.Transactions.Where(t => childIds.Contains(t.ProjectId));
			return (await query.Where(t => t.TransactionStatus == TransactionStatusEnum.In).SumAsync(t => t.Amount))
				   - (await query.Where(t => t.TransactionStatus == TransactionStatusEnum.Out).SumAsync(t => t.Amount));


		}

		public async Task<long> GetClientEngPaymentSum(Guid clientId)
        {
            var transactions = await context.Transactions.Where(w => w.Client.Id == clientId && w.TransactionStatus == TransactionStatusEnum.In).ToListAsync();
            long amount = 0;
            var engineers = await context.Engineer.ToListAsync();
            foreach(var trans in  transactions)
            {
                if(engineers.Any(a=>a.UserId == trans.UserId)) amount += trans.Amount;
            }
            return amount;
        }

        public async Task DeleteByIds(IEnumerable<Guid> ids)
        {
            var idList = ids.ToList();
            if (idList.Count == 0) return;

            var entities = await context.Transactions
                .Where(t => idList.Contains(t.Id))
                .ToListAsync();

            context.Transactions.RemoveRange(entities);
            await context.SaveChangesAsync();
        }
    }
}
