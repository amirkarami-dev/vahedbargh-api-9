using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum OwnershipTypeEnum
{
    [Display(Name="حقیقی")]
    Real = 1,
    [Display(Name ="حقوقی" )]
    Legal = 2,
}