using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum BuildingTypeEnum
{


    [Display(Name = "نامشخص")]
    None = 0,
    [Display(Name = "مسکونی")]
    Residential = 1,
    [Display(Name = "تجاری")]
    Commercial = 2,
    [Display(Name = "اداری")]
    Official = 3,
    [Display(Name = "صنعتی")]
    Industrial = 4,
    [Display(Name = "عمومی")]
    General = 5
}
