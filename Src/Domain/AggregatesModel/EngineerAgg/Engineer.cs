using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Domain.AggregatesModel.EngineerAgg
{
    
    public class Engineer
    {
        private Engineer(){}
        public Engineer( string userId,Client client, string fullName, string naCode, string cellPhone, DateTime julianBirthDate, string solarBirthDate, string dadName, string tell, string address, int idSection, DateTime julianMembershipDate, string solarMembershipDate, FieldTypeEnum fieldTypeEnum, EducationTypeEnum educationTypeEnum, MaritalStatusTypeEnum maritalStatusTypeEnum, RelatedTypeEnum relatedTypeEnum, string bankAccountNumber, bool inactive, long defaultQuota, int sortIndex, bool bankAccountBlocked, bool certOfEarth, bool certOfTest, bool certOfFiber,bool certOfInspection)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Client = client;
            FullName = fullName;
            NaCode = naCode;
            CellPhone = cellPhone;
            JulianBirthDate = julianBirthDate;
            SolarBirthDate = solarBirthDate;
            DadName = dadName;
            Tell = tell;
            Address = address;
            IdSection = idSection;
            JulianMembershipDate = julianMembershipDate;
            SolarMembershipDate = solarMembershipDate;
            FieldTypeEnum = fieldTypeEnum;
            EducationTypeEnum = educationTypeEnum;
            MaritalStatusTypeEnum = maritalStatusTypeEnum;
            RelatedTypeEnum = relatedTypeEnum;
            BankAccountNumber = bankAccountNumber;
            Inactive = inactive;
            IsDelete = false;
            SortIndex = sortIndex;
            Inactive = inactive;
            DefaultQuota = defaultQuota;
            BankAccountBlocked = bankAccountBlocked;
            CertOfEarth = certOfEarth;
            CertOfTest = certOfTest;
            CertOfFiber = certOfFiber;
            CertOfInspection = certOfInspection;
            Has1Percent = true;
            HasQuarterIncrease = false;
        }

        public Guid Id { get; init; }

        public Client Client { get; init; }
        public string UserId { get; private set; }
        public string FullName { get; private set; }
        public string NaCode { get; private set; }
        public string CellPhone { get; private set; }
        public string Email { get; set; }
        public DateTime JulianBirthDate { get; private set; }
        public string SolarBirthDate { get; private set; }
        public string DadName { get; private set; }
        public string Tell { get; private set; }
        public string Address { get; private set; }
        public int IdSection { get; set; }
        public DateTime JulianMembershipDate { get; set; }
        public string SolarMembershipDate { get; private set; }
        public FieldTypeEnum FieldTypeEnum { get; private set; }
        public EducationTypeEnum EducationTypeEnum { get; private set; }
        public MaritalStatusTypeEnum MaritalStatusTypeEnum { get; private set; }
        public RelatedTypeEnum RelatedTypeEnum { get; private set; }
        public string Additional { get; set; }
        public long DefaultQuota { get; set; }
        public bool CertOfTest { get; set; }
        public bool CertOfEarth { get; set; }
        public bool CertOfFiber { get; set; }
        public bool CertOfInspection { get; set; }
        public bool Inactive { get; set; }
        public bool IsDelete { get; set; }
        public string BankAccountNumber { get; set; }
        public int SortIndex { get; set; }
        public ICollection<EngineerHistory> EngineerHistories { get; set; }
        public bool BankAccountBlocked { get; set; }
        public bool Has1Percent { get; set; }
        public bool HasQuarterIncrease { get; set; }

        public void UpdateUserId(string userId)
        {
            UserId = userId;
        }

        public void UpdateEngineer( 
            string fullName, 
            string naCode, 
            string cellPhone, 
            DateTime julianBirthDate, 
            string solarBirthDate, 
            string dadName, 
            string tell, 
            string address, 
            int idSection, 
            DateTime julianMembershipDate, 
            string solarMembershipDate, 
            FieldTypeEnum fieldTypeEnum, 
            EducationTypeEnum educationTypeEnum, 
            MaritalStatusTypeEnum maritalStatusTypeEnum, 
            RelatedTypeEnum relatedTypeEnum, 
            string bankAccountNumber, 
            bool inactive, 
            long defaultQuota, 
            int sortIndex, 
            bool bankAccountBlocked, 
            bool certOfEarth, 
            bool certOfTest, 
            bool certOfFiber, 
            bool certOfInspection, 
            bool has1Percent,
            bool hasQuarterIncrease)
        {
            FullName = fullName;
            NaCode = naCode;
            CellPhone = cellPhone;
            JulianBirthDate = julianBirthDate;
            SolarBirthDate = solarBirthDate;
            DadName = dadName;
            Tell = tell;
            Address = address;
            IdSection = idSection;
            JulianMembershipDate = julianMembershipDate;
            SolarMembershipDate = solarMembershipDate;
            FieldTypeEnum = fieldTypeEnum;
            EducationTypeEnum = educationTypeEnum;
            MaritalStatusTypeEnum = maritalStatusTypeEnum;
            RelatedTypeEnum = relatedTypeEnum;
            BankAccountNumber = bankAccountNumber;
            Inactive = inactive;
            DefaultQuota = defaultQuota;
            SortIndex = sortIndex;
            BankAccountBlocked = bankAccountBlocked;
            CertOfEarth = certOfEarth;
            CertOfTest = certOfTest;
            CertOfFiber = certOfFiber;
            CertOfInspection = certOfInspection;
            Has1Percent = has1Percent;
            HasQuarterIncrease = hasQuarterIncrease;
        }

    }
}
