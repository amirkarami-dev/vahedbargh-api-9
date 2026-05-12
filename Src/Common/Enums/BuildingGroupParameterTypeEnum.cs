using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Coreapi.Common.Enums;

public enum BuildingGroupParameterTypeEnum
{
    [Display(Name = "نامشخص")]
    B0 = 0,
    [Display(Name = "از1تا2")]
    B1 = 1,
    [Display(Name = "از3تا5")]
    B2 = 2,
    [Display(Name = "7از6تا")]
    B3 = 3,
    [Display(Name = "10از8تا")]
    B4 = 4,
    [Display(Name = "12از11تا")]
    B5 = 5,
    [Display(Name = "15از13تا")]
    B6 = 6,
    [Display(Name = "از16به بالا")]
    B7 = 7

}