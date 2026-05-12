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
using Coreapi.Domain.AggregatesModel.UserFileAgg;

namespace Coreapi.Application.Features.User.Queries.GetClientUserFiles
{
    public class GetUserFilesDto:IMapFrom<UserFile>
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Des { get; set; }
        public UserFileTypeEnum FileTypeEnum { get; set; }
        public string FileTypeName { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string ToUserId { get; set; }
        public Client Client { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserFile, GetUserFilesDto>()
                .ForMember(destination => destination.FileTypeName,
                    otp => otp.MapFrom(source => source.UserFileTypeEnum.GetDisplayName()))
                .ForMember(destination => destination.FileTypeEnum,
                    otp => otp.MapFrom(source => source.UserFileTypeEnum));
            ;
        }
    }
}
