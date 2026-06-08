using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;


namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg
{
    public class ElectProjectProcess
    {
        private ElectProjectProcess()
        {
        }

        public ElectProjectProcess(
            Client client, 
            string userId, 
            ElectProject electProject, 
            Engineer engineer,
            ProjectLevelEnum projectLevelEnum,
            BuildingTariff buildingTariff, 
            InspectionStatusEnum inspectionStatusEnum,
            bool defect, long fee, 
            QuarterTariff quarterTariff, 
            DateTime julianRegisterDate, 
            string solarRegisterDate,
            DateTime julianDateDeliverEngineer, 
            string solarDateDeliverEngineer, 
            DateTime? julianDateDeliverOffice,
            string solarDateDeliverOffice, 
            string description)
        {
            Id = Guid.NewGuid();
            Client = client;
            UserId = userId;
            ElectProject = electProject;
            Engineer = engineer;
            ProjectLevelEnum = projectLevelEnum;
            BuildingTariff = buildingTariff;
            InspectionStatusEnum = inspectionStatusEnum;
            Defect = defect;
            Fee = fee;
            QuarterTariff = quarterTariff;
            JulianRegisterDate = julianRegisterDate;
            SolarRegisterDate = solarRegisterDate;
            JulianDateDeliverEngineer = julianDateDeliverEngineer;
            SolarDateDeliverEngineer = solarDateDeliverEngineer;
            JulianDateDeliverOffice = julianDateDeliverOffice;
            SolarDateDeliverOffice = solarDateDeliverOffice;
            Accepted = false;
            Description = description;
            IsDelete = false;
            IsMain = false;
        }

        public Guid Id { get; init; }
        public Client Client { get; init; }
        public string UserId { get; init; }
        public ElectProject ElectProject { get; init; }
        public Engineer Engineer { get; set; }
        public ProjectLevelEnum ProjectLevelEnum { get; set; }
        public BuildingTariff BuildingTariff { get; set; }
        public InspectionStatusEnum InspectionStatusEnum { get; set; }
        public bool Defect { get; set; }
        public long Fee { get; set; }
        public QuarterTariff QuarterTariff { get; init; }
        public DateTime JulianRegisterDate { get; set; }
        public string SolarRegisterDate { get; set; }
        public DateTime JulianDateDeliverEngineer { get; set; }
        public string SolarDateDeliverEngineer { get; set; }
        public DateTime? JulianDateDeliverOffice { get; set; }
        public string SolarDateDeliverOffice { get; set; }
        public bool Accepted { get; set; }
        public DateTime? JulianDateAccepted { get; set; }
        public string SolarDateAccepted { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public bool IsMain { get; set; }



        public void UpdateDisapproval(DateTime? julianDateDeliverOffice, string solarDateDeliverOffice,
            string description)
        {
            InspectionStatusEnum = InspectionStatusEnum.Disapproval;
            JulianDateDeliverOffice = julianDateDeliverOffice;
            SolarDateDeliverOffice = solarDateDeliverOffice;
            ProjectLevelEnum = ProjectLevelEnum.ExpertStage;
            Description = description;
        }

        public void UpdateDoneExpertStage(DateTime? julianDateDeliverOffice, string solarDateDeliverOffice,
            string description)
        {
            InspectionStatusEnum = InspectionStatusEnum.Done;
            JulianDateDeliverOffice = julianDateDeliverOffice;
            SolarDateDeliverOffice = solarDateDeliverOffice;
            Description = description;
            ProjectLevelEnum = ProjectLevelEnum.ExpertStage;

        }
        public void UpdateDoneErtStage(DateTime? julianDateDeliverOffice, string solarDateDeliverOffice,
            string description)
        {
            InspectionStatusEnum = InspectionStatusEnum.Done;
            JulianDateDeliverOffice = julianDateDeliverOffice;
            SolarDateDeliverOffice = solarDateDeliverOffice;
            Description = description;
            ProjectLevelEnum = ProjectLevelEnum.ErtStage;

        }

        public void UpdateDoneTestAndDeliveryStage(DateTime? julianDateDeliverOffice, string solarDateDeliverOffice,
            string description)
        {
            InspectionStatusEnum = InspectionStatusEnum.Done;
            JulianDateDeliverOffice = julianDateDeliverOffice;
            SolarDateDeliverOffice = solarDateDeliverOffice;
            Description = description;
            ProjectLevelEnum = ProjectLevelEnum.TestDeliveryStage;

        }

        public void UpdateCancelExpertStage(DateTime? julianDateDeliverOffice, string solarDateDeliverOffice,
            string description, ProjectLevelEnum projectLevelEnum)
        {
            InspectionStatusEnum = InspectionStatusEnum.Canceled;
            JulianDateDeliverOffice = julianDateDeliverOffice;
            SolarDateDeliverOffice = solarDateDeliverOffice;
            Description = description;
            ProjectLevelEnum = projectLevelEnum;

        }




        // کنسل کردن در زمانیکه تخصیص نقص به کارشناسی بدیم و کنسل کنه
        public void UpdateCancelDefectStage(DateTime? julianDateDeliverOffice, string solarDateDeliverOffice,
            string description)
        {
            InspectionStatusEnum = InspectionStatusEnum.Canceled;
            JulianDateDeliverOffice = julianDateDeliverOffice;
            SolarDateDeliverOffice = solarDateDeliverOffice;
            Description = description;
            ProjectLevelEnum = ProjectLevelEnum.ApproveStage;
        }

        public void UpdateAccepted()
        {
            Accepted = true;
            JulianDateAccepted = DateTime.Now.Date;
            SolarDateAccepted = Helper.MiladiToShamsi(DateTime.Now.Date);

        }

		public void ChangeEngineer(Engineer engineer) => Engineer = engineer;

		public void UpdateIsMain(bool isMain) => IsMain = isMain;

		public void SoftDelete()
        {
            IsDelete = true;
        }
    }
}
