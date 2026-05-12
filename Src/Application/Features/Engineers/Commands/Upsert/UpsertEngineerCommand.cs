using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using MediatR;

namespace Coreapi.Application.Features.Engineers.Commands.Upsert
{
    public class UpsertEngineerCommand:IRequest<int>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string NaCode { get; set; }
        public string CellPhone { get; set; }
        public string SolarBirthDate { get; set; } = "1365/01/01";
        public string DadName { get; set; } = string.Empty;
        public string Tell { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int IdSection { get; set; }
        public string SolarMembershipDate { get; set; }
        public FieldTypeEnum FieldTypeEnum { get; set; } = FieldTypeEnum.None;
        public EducationTypeEnum EducationTypeEnum { get; set; } = EducationTypeEnum.MasterDegree;
        public MaritalStatusTypeEnum MaritalStatusTypeEnum { get; set; } = MaritalStatusTypeEnum.Married;
        public RelatedTypeEnum RelatedTypeEnum { get; set; }
        public long DefaultQuota { get; set; } 
        public bool Inactive { get; set; }
        public string BankAccountNumber { get; set; } = string.Empty;
        public int SortIndex { get; set; } = 10000;
        public bool BankAccountBlocked { get; set; } = false;
        public bool CertOfTest { get; set; } = false;
        public bool CertOfEarth { get; set; } = false;
        public bool CertOfFiber { get; set; } = false;
        public bool CertOfInspection { get; set; }
        public bool Has1Percent { get; set; }
        public bool HasQuarterIncrease { get; set; } = false;
    }
}
