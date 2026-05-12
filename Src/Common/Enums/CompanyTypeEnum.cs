using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum CompanyTypeEnum
    {
        [Display(Name = "Public Company")]
        PublicCompany = 1,
        [Display(Name = "Private Company")]
        PrivateCompany = 2,
        [Display(Name = "Organization")]
        Organization = 3
    }
}
