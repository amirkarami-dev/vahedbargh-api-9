using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<long> GetBalance(string userId);
        Task<IEnumerable<UserBalanceViewModel>> GetBalances();
        Task<long> GetCountUserTransaction(string userId);
        Task<BaseModelWithTotalRow<TransactionViewModel>> GetClientUserTransactions(Guid clientId,string userId,int pageNumber,int rowCount);
        Task<BaseModelWithTotalRow<TransactionViewModel>> GetClientTransactions(Guid clientId,int pageNumber,int rowCount, long fileNumber, int idCity, TransactionStatusEnum transactionStatusEnum
        ,DateTime? julianCreated);
        Task<IEnumerable<Invoice>> GetClientInvoices(Guid clientId, int pageNumber, int rowCount);
        Task<IEnumerable<Invoice>> GetClientInvoices(Guid clientId,Guid engId, int pageNumber, int rowCount);
        Task<IEnumerable<Invoice>> GetClientEngInvoices(Guid clientId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<EngPaymentViewModel>> GetClientEngPaymentGroup(Guid clientId, DateTime startDate, DateTime endDate);
        Task<long> GetClientEngInvoiceSum(Guid clientId, Guid engId, Guid idQuarterTariff, string month);
        Task<long> GetClientEngInvoiceSum(Guid clientId,Guid engId);
        Task<long> GetClientEngPaymentSum(Guid clientId);
        Task<Transaction> GetByBankTransactionId(string btId);
        Task<Transaction> GetByElectProjectId(string projectId, TransactionStatusEnum transactionStatusEnum);
        Task<bool> HasTransactionInForChild(List<string>  childIds, TransactionStatusEnum transactionStatusEnum);
        Task<long> GetProjectBalance(Guid electProjectId); 
        Task<long> GetBigProjectBalance(List<string> childIds); 
		Task<IEnumerable<Invoice>> GetClientEngPaymentByEng(Guid clientId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transaction>> GetClientTransactions(Guid clientId, TransactionStatusEnum transactionStatusEnum);
        Task DeleteByIds(IEnumerable<Guid> ids);


    }
}
