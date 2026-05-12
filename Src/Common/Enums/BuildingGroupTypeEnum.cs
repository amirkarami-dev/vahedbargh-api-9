using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum BuildingGroupTypeEnum
{

    [Display(Name = "نامشخص")]
    G0,
    [Display(Name = "گروه الف")]
    G1,
    [Display(Name = "گروه ب")]
    G2,
    [Display(Name = "گروه ج")]
    G3,
    [Display(Name = "گروه د")]
    G4,
    [Display(Name = "پرونده های بزرگ")]
    G5,

}