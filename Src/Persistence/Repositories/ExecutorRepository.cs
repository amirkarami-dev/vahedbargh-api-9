using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class ExecutorRepository:BaseRepository<Executor>,IExecutorRepository
    {
        public ExecutorRepository(CoreapiDbContext context) : base(context)
        {
        }

        public async Task<Executor> GetExecutorByUserId(string userId)
        {
            return await context.Executors.Where(c=>c.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Executor>> GetByClientId(Guid clientId)
        {
            return await context.Executors.Where(c => c.ClientId == clientId).ToListAsync();
        }

        public async Task<IEnumerable<ClientExecutorsViewModel>> GetByClientIdViewModel(Guid clientId)
        {
            
            var executors = await context.Executors.Where(t => EF.Property<Guid>(t, "ClientId") == clientId)
                .Select(p => new ClientExecutorsViewModel
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    ClientId = p.ClientId,
                    OwnershipTypeEnum = p.OwnershipTypeEnum,
                    ExecutorTypeEnum = p.ExecutorTypeEnum,
                    ExecutorGradTypeEnum = p.ExecutorGradTypeEnum,
                    CompanyName = p.CompanyName,
                    FullName = p.FullName,
                    Tel = p.Tel,
                    NaCode = p.NaCode,
                    CellPhone = p.CellPhone,
                    License = p.License,
                    LicenseNumber = p.LicenseNumber,
                    IdSection = p.IdSection,
                    Address = p.Address,
                    MoreInfo = p.MoreInfo,
                    SignatureFileName = p.SignatureFileName,
                    LicenseFileName = p.LicenseFileName,
                    Inactive = p.Inactive,
                    Balance = context.Transactions.Where(c=>c.UserId== p.UserId && c.TransactionStatus == TransactionStatusEnum.In).Sum(s=>s.Amount) - context.Transactions.Where(c => c.UserId == p.UserId && c.TransactionStatus == TransactionStatusEnum.Out).Sum(s => s.Amount)
                }).ToListAsync();
            return executors;
        }

        public async Task<Executor> GetExecutorByCellPhone(string cellPhone)
        {
            return await context.Executors.Where(c => c.CellPhone == cellPhone).FirstOrDefaultAsync();
        }

        public async Task<Executor> GetExecutorByNaCode(string naCode)
        {
            return await context.Executors.Where(c => c.NaCode == naCode).FirstOrDefaultAsync();
        }

        public async Task<Executor> GetExecutorByMoreInfo(long moreInfo)
        {
            return await context.Executors.Where(c => c.MoreInfo!= null && c.MoreInfo == moreInfo.ToString()).FirstOrDefaultAsync();

        }
    }
}
