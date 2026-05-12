using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    [System.Flags]
    public enum EngineerGradeTypeEnum
    {
        [Display(Name = "ارشد")]
        SeniorDegree= 0,
        [Display(Name = "پایه 1")]
        FirstBase =1,
        [Display(Name = "پایه 2")]
        SecondBase =2,
        [Display(Name = "پایه 3")]
        ThirdBase =3,
        [Display(Name = "none")]
        None = 4,

    }
}
