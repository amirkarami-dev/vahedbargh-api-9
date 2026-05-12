using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Features.ElectProjectProcesses.Queries.GetListProjectProcessEng;
using Coreapi.Common.Models;
using Coreapi.Common.Utility;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetClientUserTransactions
{
    public class GetClientUserTransactionQueryHandler:IRequestHandler<GetClientUserTransactionQuery,PaggingList<TransactionViewModel>>
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;
        private readonly IUserManager userManager;

        public GetClientUserTransactionQueryHandler(ITransactionRepository transactionRepository, IMapper mapper, ICurrentUser currentUser, IClientRepository clientRepository, IUserManager userManager)
        {
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
            this.userManager = userManager;
        }
        public async Task<PaggingList<TransactionViewModel>> Handle(GetClientUserTransactionQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetWithSetting(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var getUser = await userManager.GetUserAsync(currentUser.UserId) ?? throw new NotAllowedException("کاربری مشکل دارد");

            var output = new PaggingList<TransactionViewModel>()
            {
                Data = new List<TransactionViewModel>(),
                TotalItems = 0
            };



            if (currentUser.Role is "Accountant" or "Administrator" or "Section")
            {
                var transactions = await transactionRepository
                    .GetClientTransactions(client.Id,request.Page-1, request.PageSize,request.FileNumber,
                        request.IdCity,
                        request.TransactionStatusEnum,
                        string.IsNullOrEmpty(request.SolarCreated)?null: 
                            Helper.ShamsiToMiladi(request.SolarCreated).Date);
                //foreach (var dto in transactionList)
                //{
                //    dto.userData = getUsers.FirstOrDefault(f => f.Id == dto.SearchUserId);
                //}
                output.Data = transactions.AggregateModel.ToList();
                output.TotalItems = transactions.TotalItem;
                return output;
            }


            else
            {
                var transactions = await transactionRepository.GetClientUserTransactions(client.Id, currentUser.UserId, request.Page-1, request.PageSize);

                 output.Data = transactions.AggregateModel.ToList();
                 output.TotalItems = transactions.TotalItem;
                 return output;
            }
        
        }
    }
}
