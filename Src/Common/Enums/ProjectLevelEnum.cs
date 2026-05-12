using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Coreapi.Common.Enums;

public enum ProjectLevelEnum
{
    [Display(Name = "قبل تخصیص")]
    NullStage = 0,
    [Display(Name = "کارشناسی")]
    ExpertStage = 1,
    [Display(Name = "مجری ارت")]
    ErtStage = 2,
    [Display(Name = "تابلو برق")]
    ElectPanelStage = 3,
    [Display(Name = "تایید شده-نظام")]
    ApproveStage = 4,
    [Display(Name = "مجری ارت-تایید")]
    ApproveErtStage = 5,
    [Display(Name = "تست و تحویل")]
    TestDeliveryStage = 6,
    [Display(Name = "تست وتحویل-تایید")]
    ApproveTestDeliveryStage = 7,
    [Display(Name = "شرکت توزیع")]
    EdcStage = 8,
    [Display(Name = "نقص")]
    Defect = 9

}