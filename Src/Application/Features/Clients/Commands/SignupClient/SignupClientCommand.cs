using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Features.Clients.Commands.SignupClient
{
    public class SignupClientCommand : IRequest<SignupOutput>
    {
        public Guid PackageId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string ABN { get; set; }
        public CompanyTypeEnum CompanyType { get; set; }
        public StaffRangeEnum StaffRange { get; set; }
        public int LicenseCount { get; set; } = 1;
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpireDate { get; set; }
        public string CCV { get; set; }
        public CardTypeEnum Type { get; set; }
    }
}
