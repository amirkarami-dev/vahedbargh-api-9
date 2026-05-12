using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class EngineerRepository:BaseRepository<Engineer>,IEngineerRepository
    {
        public EngineerRepository(CoreapiDbContext context) : base(context)
        {
        }

        public async Task<Engineer> getByUserId(string userId)
        {
            return await context.Engineer.Where(c => c.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Engineer> getByNaCode(string naCode)
        {
            
            return await context.Engineer.Where(c => c.NaCode == naCode).FirstOrDefaultAsync();
        }

        public long GetSumAmountQuota(long defaultQuota, IEnumerable<EngineerHistoryViewModel> historyViewModels, IEnumerable<QuarterTariff> quarterTariffs)
        {
           
            var amountQuota = (from quarterTariff in quarterTariffs 
                let engGradFactor = Helper.GetEngGradeFactor(historyViewModels, quarterTariff.JulianAllotmentDate,quarterTariff.AddDifDays) 
                select quarterTariff.Fee * engGradFactor).Sum();

            return (long)amountQuota + defaultQuota;

        }
        public long GetSumAmountQuotaSingle(long defaultQuota, EngineerGradeTypeEnum engGradeTypeEnum, DateTime julianWorkPermit, QuarterTariff quarterTariff, WorkPermitTypeEnum workPermitTypeEnum)
        {
            if (workPermitTypeEnum is not (WorkPermitTypeEnum.None or WorkPermitTypeEnum.Inspection
                or WorkPermitTypeEnum.InspectionAndEarth)) return 0;

            var engGradFactor = Helper.GetEngGradeFactorSingle(engGradeTypeEnum, julianWorkPermit, quarterTariff.JulianAllotmentDate);
            var amountQuota = quarterTariff.Fee * engGradFactor;

            return (long)amountQuota;

        }
        public long GetCountErtQuota(long defaultQuota, IEnumerable<EngineerHistoryViewModel> historyViewModels, IEnumerable<QuarterTariff> quarterTariffs)
        {

            var amountQuota = (from quarterTariff in quarterTariffs
                let engGradFactor = Helper.GetEngGradeFactorForErt(historyViewModels, quarterTariff.JulianAllotmentDate, quarterTariff.AddDifDays)
                select quarterTariff.CountErt * engGradFactor).Sum();

            return (long)amountQuota;

        }

        public long GetCountErtQuotaSingle(long defaultQuota, EngineerGradeTypeEnum engGradeTypeEnum, DateTime julianWorkPermit, QuarterTariff quarterTariff)
        {
            var engGradFactor = Helper.GetEngGradeFactorSingle(engGradeTypeEnum, julianWorkPermit, quarterTariff.JulianAllotmentDate);
            var amountQuota = quarterTariff.Fee * engGradFactor;

            return (long)amountQuota;

        }

        public async Task<IEnumerable<ClientEngineersViewModel>> GetByClientIdViewModel(Guid clientId, Guid? engId, FilterCertEnum filterCertEnum)
        {


            var engineer = await context.Engineer
                .Include(c => c.Client)
                .Where(t => t.Client.Id == clientId && !t.IsDelete)
                .Where(w=> filterCertEnum == FilterCertEnum.Fc0 
                           || (filterCertEnum == FilterCertEnum.Fc1 && w.CertOfInspection
                           || (filterCertEnum == FilterCertEnum.Fc2 && w.CertOfEarth)
                           || (filterCertEnum == FilterCertEnum.Fc3 && w.CertOfTest)
                           ))
                .Where(w=> engId == null || w.Id == engId)
                .Select(p => new ClientEngineersViewModel
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    ClientId = p.Client.Id,
                    CellPhone = p.CellPhone,
                    Additional = p.Additional,
                    Address = p.Address,
                    DadName = p.DadName,
                    EducationTypeEnum = p.EducationTypeEnum,
                    EducationTypeName = Enum.GetName(p.EducationTypeEnum),
                    FieldTypeEnum = p.FieldTypeEnum,
                    FieldTypeName = Enum.GetName(p.FieldTypeEnum),
                    FullName = p.FullName,
                    IdSection = p.IdSection,
                    JulianMembershipDate = p.JulianMembershipDate,
                    SolarMembershipDate = p.SolarMembershipDate,
                    JulianBirthDate = p.JulianBirthDate,
                    SolarBirthDate = p.SolarBirthDate,
                    NaCode = p.NaCode,
                    MaritalStatusTypeEnum = p.MaritalStatusTypeEnum,
                    MaritalStatusTypeName = Enum.GetName(p.MaritalStatusTypeEnum),
                    RelatedTypeEnum = p.RelatedTypeEnum,
                    RelatedTypeName = Enum.GetName(p.RelatedTypeEnum),
                    Tell = p.Tell,
                    DefaultQuota = p.DefaultQuota,
                    BankAccountNumber = p.BankAccountNumber,
                    SortIndex = p.SortIndex,
                    Inactive = p.Inactive,
                    BankAccountBlocked = p.BankAccountBlocked,
                    CertOfEarth = p.CertOfEarth,
                    CertOfFiber = p.CertOfFiber,
                    CertOfTest = p.CertOfTest,
                    CertOfInspection = p.CertOfInspection,
                    Has1Percent = p.Has1Percent,
                    HasQuarterIncrease = p.HasQuarterIncrease,
                    EngFiles = context.UserFiles.Where(f => f.UserId == p.UserId).Select(s=>new EngFiles
                    {
                        Id = s.Id,
                        Des = s.Des,
                        FileName = s.FileName,
                        FolderName = s.FolderName,
                        Name = s.Name,
                        Title = s.Title,
                        UserFileTypeEnum = s.UserFileTypeEnum,
                        UserFileTypeName = s.UserFileTypeEnum.GetDisplayName()
                    }).ToList(),
                    EngineerHistoryViewModel = p.EngineerHistories.Select(s=>new EngineerHistoryViewModel
                    {
                        Id = s.Id,
                        WorkPermitNum = s.WorkPermitNum,
                        WorkPermission = s.WorkPermission,
                        EngineerGradeTypeEnum = s.EngineerGradeTypeEnum,
                        EngineerGradeTypeName = s.EngineerGradeTypeEnum.GetDisplayName(),
                        JulianValidityDate = s.JulianValidityDate,
                        SolarValidityDate = s.SolarValidityDate,
                        JulianIssueDate = s.JulianIssueDate,
                        SolarIssueDate = s.SolarIssueDate,
                        WorkPermitTypeEnum = s.WorkPermitTypeEnum,
                        WorkPermitTypeName = s.WorkPermitTypeEnum.GetDisplayName()
                    })
                }).ToListAsync();

            return engineer;
        }

        public async Task<IEnumerable<ClientEngWorkViewModel>> GetEngWorkViewModel(Guid clientId, Guid? engId, Guid idQuarterTariff,int period, int month1,int month2, int month3)
        {
            var engineer = await context.Engineer.Include(c => c.Client)
                .Where(t => t.Client.Id == clientId && !t.IsDelete)
                .Where(w => engId == null || w.Id == engId)
                .Select(p=> new ClientEngWorkViewModel
                {
                    FullName = p.FullName,
                    Id = p.Id,
                    UserId = p.UserId,
                    SumAmountEngRealWorkQuarterFirstMonth = context.Invoices
                        .Include(i => i.ElectProjectProcess.Engineer)
                        .Include(i => i.ElectProjectProcess.QuarterTariff)
                        .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == p.Id && t.ElectProjectProcess.QuarterTariff.Id == idQuarterTariff)
                        .Where(w => w.Transaction.SolarCreated.Substring(5,2).Equals($"0{month1}") || w.Transaction.SolarCreated.Substring(5,2).Equals($"{month1}/"))
                        .Sum(c => c.Amount),

                    SumAmountEngRealWorkQuarterSecondMonth = context.Invoices
                        .Include(i => i.ElectProjectProcess.Engineer)
                        .Include(i => i.ElectProjectProcess.QuarterTariff)
                        .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == p.Id && t.ElectProjectProcess.QuarterTariff.Id == idQuarterTariff)
                        .Where(w => w.Transaction.SolarCreated.Substring(5,2).Equals($"0{month2}") || w.Transaction.SolarCreated.Substring(5,2).Equals($"{month2}/"))
                        .Sum(c => c.Amount),

                    SumAmountEngRealWorkQuarterThirdMonth = context.Invoices
                        .Include(i => i.ElectProjectProcess.Engineer)
                        .Include(i => i.ElectProjectProcess.QuarterTariff)
                        .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == p.Id && t.ElectProjectProcess.QuarterTariff.Id == idQuarterTariff)
                        .Where(w => w.Transaction.SolarCreated.Substring(5,2).Equals($"0{month3}") || w.Transaction.SolarCreated.Substring(5,2).Equals($"{month3}/"))
                        .Sum(c => c.Amount),
                    SumAmountEngRealWordBefore =  context.Invoices
                    .Include(i => i.ElectProjectProcess.Engineer)
                    .Include(i => i.ElectProjectProcess.QuarterTariff)
                    .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == p.Id && t.ElectProjectProcess.QuarterTariff.Period < period)
                    .Sum(c => c.Amount),
                    SumAmountEngRealWordThisQuarter = context.Invoices
                        .Include(i => i.ElectProjectProcess.Engineer)
                        .Include(i => i.ElectProjectProcess.QuarterTariff)
                        .Where(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectProjectProcess.Engineer.Id == p.Id && t.ElectProjectProcess.QuarterTariff.Period == period)
                        .Sum(c => c.Amount),

                    EngineerHistoryViewModel = p.EngineerHistories.Select(s => new EngineerHistoryViewModel
                    {
                        Id = s.Id,
                        WorkPermitNum = s.WorkPermitNum,
                        WorkPermission = s.WorkPermission,
                        EngineerGradeTypeEnum = s.EngineerGradeTypeEnum,
                        EngineerGradeTypeName = s.EngineerGradeTypeEnum.GetDisplayName(),
                        JulianValidityDate = s.JulianValidityDate,
                        SolarValidityDate = s.SolarValidityDate,
                        JulianIssueDate = s.JulianIssueDate,
                        SolarIssueDate = s.SolarIssueDate,
                        WorkPermitTypeEnum = s.WorkPermitTypeEnum,
                        WorkPermitTypeName = s.WorkPermitTypeEnum.GetDisplayName()
                    }),
                    DefaultQuota = p.DefaultQuota


                }).ToListAsync();
            return engineer;
        }

        public async Task<long> GetLastWorkPermitNum(Guid engId)
        {
            var engHistory = await context.EngineerHistories
                .Include(i=>i.Engineer)
                .Where(w=>w.Engineer.Id == engId)
                .OrderByDescending(g => g.JulianIssueDate)
                .FirstOrDefaultAsync();

            return engHistory?.WorkPermitNum ?? 0;
        }

        public async Task<bool> CanDoThisProcess(Guid engId, Guid electProjectId)
        {
            var lastHistory = await context.EngineerHistories.Include(i => i.Engineer).OrderByDescending(o=>o.JulianIssueDate)
                .FirstOrDefaultAsync(f => f.Engineer.Id == engId);
            if (lastHistory == null) return false;

            var electProject = await context.ElectProjects.Include(electProject => electProject.BuildingTariff).FirstOrDefaultAsync(f => f.Id == electProjectId);

            if(electProject?.BuildingTariff is null) return false;

            var result = Helper.CanDoProcess(lastHistory.EngineerGradeTypeEnum, electProject.BuildingTariff.BuildingGroupTypeEnum);
            return result;
        }

        public double GetEngFactor(EngineerHistoryViewModel engHistory)
        {
            if(engHistory is null) return 0;
            var getGradeFactor = engHistory.EngineerGradeTypeEnum switch
            {
                EngineerGradeTypeEnum.FirstBase => 2,
                EngineerGradeTypeEnum.SecondBase => 1.5,
                EngineerGradeTypeEnum.ThirdBase => 1,
                EngineerGradeTypeEnum.SeniorDegree => 2.5,
                _ => 0
            };

            return getGradeFactor;

        }

        public long GetCountAreaTestAndDelivery(long defaultQuota, IEnumerable<EngineerHistoryViewModel> historyViewModels, IEnumerable<QuarterTariff> quarterTariffs, int addDifDay)
        {

            var amountQuota = (from quarterTariff in quarterTariffs
                let engGradFactor = Helper.GetEngGradeFactorForErt(historyViewModels, quarterTariff.JulianAllotmentDate,addDifDay)
                select quarterTariff.CountErt * engGradFactor).Sum();

            return (long)amountQuota;

        }
    }
}
