using Coreapi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Models;

namespace Coreapi.Common.ViewModels
{
    public class ClientEngineersViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid ClientId { get; set; }
        public string FullName { get; set; }
        public string FullDescription { get; set; }
        public string NaCode { get; set; }
        public string CellPhone { get; set; }
        public DateTime JulianBirthDate { get; set; }
        public string SolarBirthDate { get; set; }
        public string DadName { get; set; }
        public string Tell { get; set; }
        public string Address { get; set; }
        public int IdSection { get; set; }
        public DateTime JulianMembershipDate { get; set; }
        public string SolarMembershipDate { get; set; }
        public FieldTypeEnum FieldTypeEnum { get; set; }
        public string FieldTypeName { get; set; }
        public EducationTypeEnum EducationTypeEnum { get; set; }
        public string EducationTypeName { get; set; }
        public MaritalStatusTypeEnum MaritalStatusTypeEnum { get; set; }
        public string MaritalStatusTypeName { get; set; }
        public RelatedTypeEnum RelatedTypeEnum { get; set; }
        public string RelatedTypeName { get; set; }
        public string Additional { get; set; }
        public IEnumerable<EngineerHistoryViewModel> EngineerHistoryViewModel { get; set; }
        public long RemainingQuota { get; set; }
        public long SumAmountEngQuota { get; set; }
        public long CountErtQuota { get; set; }
        public long RemainingCurrentQuarterQuota { get; set; }
        public long SumAmountEngProcessFee { get; set; }
        public long TotalQuotaBalance { get; set; }
        public double TotalQuotaBalancePercent { get; set; }
        public double CountErtBalancePercent { get; set; }
        public long SumAmountEngProcessCancelFee { get; set; }
        public long  SumEngBalance { get; set; }
        public UserDataModel UserData { get; set; }
        public long DefaultQuota { get; set; }
        public bool ExpiredDateWork { get; set; }
        public bool Inactive { get; set; }
        public string BankAccountNumber { get; set; }
        public int SortIndex { get; set; }
        public int CountCancel { get; set; }
        public int CountExpertCancel { get; set; }
        public int CountErtCancel { get; set; }
        public int SumAreaTestAndDelivery { get; set; }
        public string EngineerGradeTypeName { get; set; }
        public EngineerGradeTypeEnum EngineerGradeTypeEnum { get; set; }
        public string CanDoGroupBuilding { get; set; }
        public string SolarValidityDate { get; set; }
        public string SolarIssueDate { get; set; }
        public bool BankAccountBlocked { get; set; }
        public  string UserName { get; set; }
        public bool CertOfTest { get; set; }
        public bool CertOfEarth { get; set; }
        public bool CertOfFiber { get; set; }
        public bool CertOfInspection { get; set; }
        public IEnumerable<EngFiles> EngFiles { get; set; }
        public int CountErtProcess { get; set; }
        public bool Has1Percent { get; set; }
        public double Factor { get; set; }
        public bool HasQuarterIncrease { get; set; }
        public long ExpertIncrease { get; set; }
        public int ErtIncrease { get; set; }

    }

    public class EngineerHistoryViewModel
    {
        public Guid Id { get; set; }
        public EngineerGradeTypeEnum EngineerGradeTypeEnum { get; set; }
        public string EngineerGradeTypeName { get; set; }
        public long WorkPermitNum { get; set; }
        public DateTime JulianIssueDate { get; set; }
        public string SolarIssueDate { get; set; }
        public DateTime JulianValidityDate { get; set; }
        public string SolarValidityDate { get; set; }
        public bool WorkPermission { get; set; }
        public WorkPermitTypeEnum WorkPermitTypeEnum { get; set; }
        public string WorkPermitTypeName { get; set; }
    }

    public class EngFiles
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Des { get; set; }
        public UserFileTypeEnum UserFileTypeEnum { get; set; }
        public string UserFileTypeName { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
    }
}
