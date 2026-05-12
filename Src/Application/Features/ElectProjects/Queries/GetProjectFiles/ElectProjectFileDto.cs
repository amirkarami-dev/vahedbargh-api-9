using System;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetProjectFiles
{
    public class ElectProjectFileDto : IMapFrom<ElectProjectFile>
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Des { get; set; }
        public FileTypeEnum FileTypeEnum { get; set; }
        public string FileTypeName { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string ToUserId { get; set; }
        public ElectProject ElectProject { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ElectProjectFile, ElectProjectFileDto>()
                .ForMember(destination => destination.FileTypeName,
                    otp => otp.MapFrom(source => Enum.GetName(source.FileTypeEnum)));
        }

    }


}
