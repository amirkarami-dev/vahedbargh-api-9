using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum ProjectCreatedTypeEnum
{
    [Display(Name = "نظام مهندسی")]
    Nezam,
    [Display(Name = "شرکت توزیع")]
    Elect,
    [Display(Name = " وب سرویس شرکت توزیع")]
    ElectWebService,


}