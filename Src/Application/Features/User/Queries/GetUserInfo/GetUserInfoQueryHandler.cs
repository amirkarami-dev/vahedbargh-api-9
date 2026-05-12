using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Application.Common.Exceptions;
using System;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using Microsoft.Extensions.DependencyInjection;

namespace Coreapi.Application.Features.User.Queries.GetUserInfo
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoDto>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;
        private readonly IMapper mapper;
        private readonly IExecutorRepository executorRepository;
        private readonly IEngineerRepository engineerRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IClientRepository clientRepository;
        private readonly ISupportRepository supportRepository;

        public GetUserInfoQueryHandler(IUserManager userManager, ICurrentUser currentUser, IMapper mapper, IExecutorRepository executorRepository, IEngineerRepository engineerRepository, ITransactionRepository transactionRepository, IClientRepository clientRepository,
            ISupportRepository supportRepository)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
            this.mapper = mapper;
            this.executorRepository = executorRepository;
            this.engineerRepository = engineerRepository;
            this.transactionRepository = transactionRepository;
            this.clientRepository = clientRepository;
            this.supportRepository = supportRepository;
        }

        public async Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var user = await userManager.GetUserAsync(currentUser.UserId);
            if (user is null)
                return null;
            var userInfo = mapper.Map<UserInfoDto>(user);
            //result.SumAmountEngPayment = await transactionRepository.GetClientEngPaymentSum(client.Id);
            userInfo.SumAmountEngPayment = 0;
            //var executor = await executorRepository.GetExecutorByUserId(currentUser.SearchUserId);
            //if(executor is not null) result.IdSection = executor.IdSection;

            var countNewMessage = await supportRepository.GetCountUnreadMessage(user.Id);

            userInfo.CountUnreadMessage = countNewMessage;

            var engineer = await engineerRepository.getByUserId(currentUser.UserId);
            if (engineer is null) return userInfo;
            userInfo.SumAmountEngInvoice = await transactionRepository.GetClientEngInvoiceSum(client.Id ,engineer.Id);
            userInfo.IdSection = engineer.IdSection;


            return userInfo;
        }
    }
}
