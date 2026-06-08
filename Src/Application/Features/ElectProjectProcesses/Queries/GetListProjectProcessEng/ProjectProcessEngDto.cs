using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using CheckListEdc = Coreapi.Common.ViewModels.CheckListEdc;

namespace Coreapi.Application.Features.ElectProjectProcesses.Queries.GetListProjectProcessEng
{
    public class ProjectProcessEngDto:IMapFrom<ElectProjectProcess>
    {
        public string EnName { get; set; }
        public long Fee { get; set; }
        public string ProjectLevelName { get; set; }
        public ProjectLevelEnum ProjectLevel { get; set; }
        public long FileNumber { get; set; }
        public Guid Id { get; set; }
        public Guid ElectProjectId { get; set; }
        public Client Client { get; set; }
        public string UserId { get; set; }
        public string LandLordName { get; set; }
        public string LandlordNaCode { get; set; }
        public string LandlordPhoneNumber { get; set; }
        public DateTime JulianRegisterDate { get; set; }
        public string SolarRegisterDate { get; set; }
        public DateTime JulianDateDeliverEngineer { get; set; }
        public string SolarDateDeliverEngineer { get; set; }
        public DateTime? JulianDateDeliverOffice { get; set; }
        public string SolarDateDeliverOffice { get; set; }

        public int IdSection { get; set; }

        public InspectionStatusEnum InspectionStatus { get; set; }
        public string InspectionStatusName { get; set; }
        public string EngineerCellPhone { get; set; }
        public string Description { get; set; }

        public int NumberOfFloor { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int Area { get; set; }

        public string ElectProjectDescription { get; set; }
        public bool Accepted { get; set; }
        public DateTime? JulianDateAccepted { get; set; }
        public string SolarDateAccepted { get; set; }

        // توضیحات نقص شرکت توزیع
        public string DefectDes { get; set; }

        // نقص در مراحل تخصص کارشناسی
        public bool IsDefectEng { get; set; }
        public bool SolvedDefectEng { get; set; }
        public DateTime? JulianDateDefectEng { get; set; }
        public string SolarDateDefectEng { get; set; }
        public DateTime? JulianDateSolvedDefectEng { get; set; }
        public string SolarDateSolvedDefectEng { get; set; }
        public string DefectEngDes { get; set; }
        public string DefectAdminDes { get; set; }


        public long ProjectBalance { get; set; }

        public string ProjectBalanceLinkForPay { get; set; }
		public bool IsMain { get; set; }



		public IEnumerable<CommentEngForm> CommentEngForm { get; set; }
        public IEnumerable<CheckListForm> CheckListForms { get; set; }
        public IEnumerable<CheckListEdc> CheckListEdcs { get; set; }
        public ElectProjectErtForm ElectProjectErtForm { get; set; }
        public ElectProject ElectProject { get; set; }
        public Engineer Engineer { get; set; }
        public QuarterTariff QuarterTariff { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ElectProjectProcess, ProjectProcessEngDto>()
                .ForMember(destination => destination.ProjectLevelName,
                    otp => otp.MapFrom(source => source.ProjectLevelEnum.GetDisplayName()))

                .ForMember(destination => destination.InspectionStatusName,
                    otp => otp.MapFrom(source => source.InspectionStatusEnum.GetDisplayName()))

                .ForMember(destination => destination.FileNumber,
                    otp => otp.MapFrom(source=>source.ElectProject.FileNumber))
                
                .ForMember(destination => destination.ElectProjectId,
                    otp => otp.MapFrom(source => source.ElectProject.Id))
                
                .ForMember(destination => destination.ProjectLevel,
                    otp => otp.MapFrom(source => source.ProjectLevelEnum))

                .ForMember(destination => destination.InspectionStatus,
                    otp => otp.MapFrom(source => source.InspectionStatusEnum))

                .ForMember(destination => destination.EnName,
                    otp => otp.MapFrom(source => source.Engineer.FullName))

                .ForMember(destination => destination.LandLordName,
                    otp => otp.MapFrom(source => source.ElectProject.LandlordName))

                .ForMember(destination => destination.LandlordNaCode,
                    otp => otp.MapFrom(source => source.ElectProject.LandlordNaCode))

                .ForMember(destination => destination.LandlordPhoneNumber,
                    otp => otp.MapFrom(source => source.ElectProject.LandlordPhoneNumber))

                .ForMember(destination => destination.IdSection,
                    otp => otp.MapFrom(source => source.ElectProject.IdSection))

                .ForMember(destination => destination.EngineerCellPhone,
                    otp => otp.MapFrom(source => source.Engineer.CellPhone))

                .ForMember(destination => destination.NumberOfFloor,
                    otp => otp.MapFrom(source => source.ElectProject.NumberOfFloor))

                .ForMember(destination => destination.Address,
                    otp => otp.MapFrom(source => source.ElectProject.Address))
                
                .ForMember(destination => destination.PostalCode,
                    otp => otp.MapFrom(source => source.ElectProject.PostalCode))

                .ForMember(destination => destination.Area,
                    otp => otp.MapFrom(source => source.ElectProject.Area))
     
                .ForMember(destination => destination.ElectProjectDescription,
                    otp => otp.MapFrom(source => source.ElectProject.Description))

                // defect

                .ForMember(destination => destination.DefectDes,
                    otp => otp.MapFrom(source => source.ElectProject.DefectDes))

                .ForMember(destination => destination.IsDefectEng,
                    otp => otp.MapFrom(source => source.ElectProject.IsDefectEng))

                .ForMember(destination => destination.SolvedDefectEng,
                    otp => otp.MapFrom(source => source.ElectProject.SolvedDefectEng))

                .ForMember(destination => destination.JulianDateDefectEng,
                    otp => otp.MapFrom(source => source.ElectProject.JulianDateDefectEng))

                .ForMember(destination => destination.SolarDateDefectEng,
                    otp => otp.MapFrom(source => source.ElectProject.SolarDateDefectEng))
                
                .ForMember(destination => destination.JulianDateSolvedDefectEng,
                    otp => otp.MapFrom(source => source.ElectProject.JulianDateSolvedDefectEng))
                
                .ForMember(destination => destination.SolarDateSolvedDefectEng,
                    otp => otp.MapFrom(source => source.ElectProject.SolarDateSolvedDefectEng))

                .ForMember(destination => destination.DefectEngDes,
                    otp => otp.MapFrom(source => source.ElectProject.DefectEngDes))

                .ForMember(destination => destination.DefectAdminDes,
                    otp => otp.MapFrom(source => source.ElectProject.DefectAdminDes))
                ;
        }

    }
}
