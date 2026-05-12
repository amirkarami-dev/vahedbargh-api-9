using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Mappings;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Features.Clients.Queries.GetClientUsers
{
    public  class ClientUserDto : IMapFrom<UserData>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool Active { get; set; }
        public int IsPublic { get; set; }
        public string About { get; set; }
        public string Role { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserData, ClientUserDto>();
        }
    }
}
