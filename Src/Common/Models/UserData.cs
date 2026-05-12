using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Models
{
    public class UserDataModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Created { get; set; }
        public double Score { get; set; }
        public bool Active { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public string RoleId { get; set; }
        public string ClientId { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string NickName { get; set; }
        public string ExpiryDate { get; set; }


    }
}
