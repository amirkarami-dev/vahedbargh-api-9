using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Delete
{
    public class DeleteProjectsCommandHandler : IRequestHandler<DeleteProjectsCommand, string>
    {
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;
        private readonly IElectProjectRepository electProjectRepository;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IElectProjectProcessRepository processRepository;
        private readonly ISmsService msService;

        public DeleteProjectsCommandHandler(ICurrentUser currentUser, IClientRepository clientRepository, IElectProjectRepository electProjectRepository, IInvoiceRepository invoiceRepository, ITransactionRepository transactionRepository, IElectProjectProcessRepository processRepository, ISmsService msService)
        {
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
            this.electProjectRepository = electProjectRepository;
            this.invoiceRepository = invoiceRepository;
            this.transactionRepository = transactionRepository;
            this.processRepository = processRepository;
            this.msService = msService;
        }
        public async Task<string> Handle(DeleteProjectsCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var electProject = await electProjectRepository.GetById(request.Id);
            if (electProject is null || electProject.IsDelete) throw new NotFoundException("این پرونده وجود ندارد");

            if (electProject.IsBigProject) throw new NotFoundException("فعلا پرونده های بزرگ قابل حذف نیستند");
            
            if (electProject.ProjectLevelEnum != ProjectLevelEnum.NullStage) throw new NotFoundException("جهت حذف پرونده باید در مرحله قبل از تخصیص باشد");
            var epp = await processRepository.GetElectProjectProcessByEpId(electProject.Id);

            if (epp.Any())
            {
                throw new NotFoundException("این پونده دارای تخصیص می باشد و قابل حذف نیست");
            }

            electProject.SoftDelete();

            var findInvoiceCreateProject =
                await invoiceRepository.GetInvoiceByProjectId(electProject.Id, InvoicePayTypeEnum.CreateProjectStage);
            var findInvoiceNezamStage =
                await invoiceRepository.GetInvoiceByProjectId(electProject.Id, InvoicePayTypeEnum.NezamStage);
            if (findInvoiceCreateProject is not null)
            {
                if (findInvoiceCreateProject.InvoiceStatus == InvoiceStatusEnum.Cancelled) throw new NotFoundException("شماره فاکتور مربوط به این پرونده قبلا کنسل شده است");
                findInvoiceCreateProject.Cancel();

                var transaction = new Transaction(findInvoiceCreateProject.Amount, client, client.Id.ToString(), GatewayTypeEnum.BackPay,
                    TransactionTypeEnum.Client, TransactionStatusEnum.In, DateTime.Now,
                    Helper.MiladiToShamsiFull(DateTime.Now), electProject.FileNumber.ToString(), "برگشت از ایجاد پرونده" + ":" + electProject.FileNumber,electProject.Id.ToString());

                transactionRepository.Add(transaction);
            }
            if (findInvoiceNezamStage is not null)
            {
                if (findInvoiceNezamStage.InvoiceStatus == InvoiceStatusEnum.Cancelled) throw new NotFoundException("شماره فاکتور مربوط به این پرونده قبلا کنسل شده است");
                findInvoiceNezamStage.Cancel();

                var transaction = new Transaction(findInvoiceNezamStage.Amount, client, client.Id.ToString(), GatewayTypeEnum.BackPay,
                    TransactionTypeEnum.Client, TransactionStatusEnum.In, DateTime.Now,
                    Helper.MiladiToShamsiFull(DateTime.Now), electProject.FileNumber.ToString(), "برگشت از 7 درصد نظام-ارت" + ":" + electProject.FileNumber, electProject.Id.ToString());

                transactionRepository.Add(transaction);
            }


            await transactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return "success";


        }
    }
}
