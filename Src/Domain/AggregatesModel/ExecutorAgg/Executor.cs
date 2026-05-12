using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Domain.AggregatesModel.ExecutorAgg
{
    public class Executor
    {
        private Executor(){}

        public Executor( string userId, OwnershipTypeEnum ownershipTypeEnum, ExecutorTypeEnum executorTypeEnum, ExecutorGradTypeEnum executorGradTypeEnum, string companyName, string fullName, string tel, string naCode, string cellPhone, bool license,string licenseNumber, int idSection, string address, string moreInfo,Client client,
            Guid clientId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Client = client;
            ClientId= clientId;
            OwnershipTypeEnum = ownershipTypeEnum;
            ExecutorTypeEnum = executorTypeEnum;
            ExecutorGradTypeEnum = executorGradTypeEnum;
            CompanyName = companyName;
            FullName = fullName;
            Tel = tel;
            NaCode = naCode;
            CellPhone = cellPhone;
            License = license;
            LicenseNumber = licenseNumber;
            IdSection = idSection;
            Address = address;
            MoreInfo = moreInfo;
            Inactive= false;

        }

        public Guid Id { get; init; }
        public string UserId  { get; set; }
        public Client Client { get; init; }
        public Guid ClientId { get; private set; }
        public OwnershipTypeEnum OwnershipTypeEnum { get; private set; }
        public ExecutorTypeEnum ExecutorTypeEnum { get; private set; }
        public ExecutorGradTypeEnum ExecutorGradTypeEnum { get; private set; }
        public string CompanyName { get; private set; }
        public string FullName { get; private set; }
        public string Tel { get; private set; }
        public string NaCode { get; private set; }
        public string CellPhone { get; private set; }
        public bool License { get; private set; }
        public string LicenseNumber { get; private set; }
        public int IdSection { get; private set; }
        public string Address { get; private set; }
        public string MoreInfo { get; private set; }
        public string SignatureFileName { get; private set; }
        public string LicenseFileName { get; private set; }
        public DateTime? JulianLicenseExpire { get; private set; }
        public string SolarLicenseExpire { get; private set; }
        public bool Inactive { get; set; }


        public void UpdateSignatureFileName(string signatureFileName)
        {
            SignatureFileName = signatureFileName;
        }
        public void UpdateLicenseFileName(string licenseFileName)
        {
            LicenseFileName = licenseFileName;
        }
        public void UpdateUserId(string userId)
        {
            UserId = userId;
        }
        public void UpdateMoreInfo(string moreInfo)
        {
            MoreInfo = moreInfo;
        }

        public void UpdateNaCode(string naCode)
        {
            NaCode = naCode;
        }

        public void UpdateInactive(bool inactive)
        {
            Inactive = inactive;
        }

        public void UpdateExecutor( OwnershipTypeEnum ownershipTypeEnum, ExecutorTypeEnum executorTypeEnum, ExecutorGradTypeEnum executorGradTypeEnum, string companyName, string fullName, string tel, string naCode, string cellPhone, bool license, string licenseNumber, int idSection, string address, string moreInfo, bool inactive)
        {
            OwnershipTypeEnum = ownershipTypeEnum;
            ExecutorTypeEnum = executorTypeEnum;
            ExecutorGradTypeEnum = executorGradTypeEnum;
            CompanyName = companyName;
            FullName = fullName;
            Tel = tel;
            NaCode = naCode;
            CellPhone = cellPhone;
            License = license;
            LicenseNumber = licenseNumber;
            IdSection = idSection;
            Address = address;
            MoreInfo = moreInfo;
            Inactive = inactive;

        }

    }
}
