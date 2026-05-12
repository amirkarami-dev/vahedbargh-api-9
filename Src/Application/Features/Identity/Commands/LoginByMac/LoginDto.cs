using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Features.Identity.Commands.LoginByMac
{
    public class LoginDto : IMapFrom<LoginOutput>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Checksum { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoginOutput, LoginDto>();
        }
    }
}
