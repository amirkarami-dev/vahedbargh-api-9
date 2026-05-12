using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;

namespace Coreapi.Domain.AggregatesModel.EngineerAgg
{
    public class EngineerHistory
    {
        private EngineerHistory(){}
        public EngineerHistory(Engineer engineer, EngineerGradeTypeEnum engineerGradeTypeEnum, long workPermitNum, DateTime julianIssueDate, string solarIssueDate, DateTime julianValidityDate, string solarValidityDate, bool workPermission, WorkPermitTypeEnum workPermitTypeEnum)
        {
            Id = Guid.NewGuid();
            Engineer = engineer;
            EngineerGradeTypeEnum = engineerGradeTypeEnum;
            WorkPermitNum = workPermitNum;
            JulianIssueDate = julianIssueDate;
            SolarIssueDate = solarIssueDate;
            SolarIssueDateInt = Helper.ShamsiToInt(SolarIssueDate);
            JulianValidityDate = julianValidityDate;
            SolarValidityDate = solarValidityDate;
            SolarValidityDateInt = Helper.ShamsiToInt(solarValidityDate);
            WorkPermission = workPermission;
            IsDefault = false;
            WorkPermitTypeEnum = workPermitTypeEnum;
        }
        public Guid Id { get; init; }
        public Engineer Engineer { get; init; }
  
        public EngineerGradeTypeEnum EngineerGradeTypeEnum { get; private set; }
        public long WorkPermitNum { get; private set; }
        public DateTime JulianIssueDate { get; private set; }
        public string SolarIssueDate { get; private set; }
        public int SolarIssueDateInt { get; private set; }
        public DateTime JulianValidityDate { get; private set; }
        public string SolarValidityDate { get; private set; }
        public int SolarValidityDateInt { get; private set; }
        public bool WorkPermission { get; private set; }
        public bool IsDefault { get; set; }
        public WorkPermitTypeEnum WorkPermitTypeEnum { get; set; }

        public void Update( EngineerGradeTypeEnum engineerGradeTypeEnum, long workPermitNum, DateTime julianIssueDate, string solarIssueDate, DateTime julianCreditDate, string solarValidityDate, bool workPermission,WorkPermitTypeEnum workPermitTypeEnum)
        {
            EngineerGradeTypeEnum = engineerGradeTypeEnum;
            WorkPermitNum = workPermitNum;
            JulianIssueDate = julianIssueDate;
            SolarIssueDate = solarIssueDate;
            SolarIssueDateInt = Helper.ShamsiToInt(SolarIssueDate);
            JulianValidityDate = julianCreditDate;
            SolarValidityDate = solarValidityDate;
            SolarValidityDateInt = Helper.ShamsiToInt(solarValidityDate);
            WorkPermission = workPermission;
            WorkPermitTypeEnum = workPermitTypeEnum;
        }
    }
}
