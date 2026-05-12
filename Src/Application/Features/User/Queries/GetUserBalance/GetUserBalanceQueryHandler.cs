using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.User.Queries.GetUserBalance
{
    public class GetUserBalanceQueryHandler:IRequestHandler<GetUserBalanceQuery,long>
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ICurrentUser currentUser;

        public GetUserBalanceQueryHandler(ITransactionRepository transactionRepository, ICurrentUser currentUser)
        {
            this.transactionRepository = transactionRepository;
            this.currentUser = currentUser;
        }
        public async Task<long> Handle(GetUserBalanceQuery request, CancellationToken cancellationToken)
        {

            return await transactionRepository.GetBalance(currentUser.UserId);
        }
    }
}
