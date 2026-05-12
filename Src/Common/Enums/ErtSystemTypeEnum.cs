using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum ErtSystemTypeEnum
{
    [Display(Name = "ندارد")]
    None,
    [Display(Name = "الکترود زمین ساده1")]
    E1,
    [Display(Name = "الکترود زمین افقی")]
    E2,
    [Display(Name = "الکترود زمین ساده2")]
    E3,
    [Display(Name = "دو راد")]
    E4,
    [Display(Name = "الکتروداساسی-5حلقه")]
    E5,
    [Display(Name = "الکترود فونداسیون")]
    E6

}