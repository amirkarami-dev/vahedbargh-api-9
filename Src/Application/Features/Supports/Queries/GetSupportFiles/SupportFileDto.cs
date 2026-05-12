using Coreapi.Common.Enums;
using System;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Domain.AggregatesModel.SupportAgg;

namespace Coreapi.Application.Features.Supports.Queries.GetSupportFiles;

public class SupportFileDto:IMapFrom<SupportFile>
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
    public SupportFile SupportFile { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SupportFile, SupportFileDto>()
            .ForMember(destination => destination.FileTypeName,
                otp => otp.MapFrom(source => Enum.GetName(source.FileTypeEnum)));
    }
}