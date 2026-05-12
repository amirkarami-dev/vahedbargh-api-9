using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.Models;
using Coreapi.Common.ViewModels;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetClientUserTransactions
{
    public class GetClientUserTransactionQuery:IRequest<PaggingList<TransactionViewModel>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int IdCity { get; set; }
        public string SolarCreated { get; set; }
        public long FileNumber { get; set; }
        public TransactionStatusEnum TransactionStatusEnum { get; set; }
    }
}
