using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Features.User.Queries.GetUserInfo
{
    public class UserInfoDto : IMapFrom<UserData>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Skype { get; set; }
        public string Instagram { get; set; }
        public int IdSection { get; set; }
        public bool Active { get; set; }
        public string PhoneNumber { get; set; }
        public long SumAmountEngInvoice { get; set; }
        public long SumAmountEngPayment { get; set; }
        public int CountUnreadMessage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserData, UserInfoDto>();
        }
    }
}
