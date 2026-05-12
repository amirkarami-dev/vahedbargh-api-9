using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Common.Enums;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;

namespace Coreapi.Application.Features.ElectProjectProcesses.Queries.GetProjectProcess
{
    public class GetProjectProcessDto : IMapFrom<ElectProjectProcess>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string SolarRegisterDate { get; set; }
        public DateTime JulianRegisterDate { get; set; }
        public DateTime JulianDateDeliverEngineer { get; set; }
        public DateTime? JulianDateDeliverOffice { get; set; }
        public string ProjectLevelName { get; set; }
        public string InspectionStatusName { get; set; }
        public bool Defect { get; set; }
        public string CellPhone { get; set; }
        public string Description { get; set; }
        public ElectProject ElectProject { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ElectProjectProcess, GetProjectProcessDto>()

                .ForMember(destination => destination.InspectionStatusName,
                    opt => opt.MapFrom(source => source.InspectionStatusEnum.GetDisplayName()))

                .ForMember(destination => destination.ProjectLevelName,
                    opt => opt.MapFrom(source => source.ProjectLevelEnum.GetDisplayName()))
                
                .ForMember(destination=>destination.FullName,otp=>otp.MapFrom(source=>source.Engineer.FullName))
                
                .ForMember(destination=>destination.CellPhone,otp=>otp.MapFrom(source=>source.Engineer.CellPhone))
                ;
        }
    }
}
