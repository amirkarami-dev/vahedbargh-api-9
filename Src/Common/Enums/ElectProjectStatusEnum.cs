using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum ElectProjectStatusEnum
{
    [Display(Name = "نامشخص")]
    None = 0,
    [Display(Name = "در جریان")]
    InProgress = 1,
    [Display(Name = "در انتظار ارسال به شرکت توزیع")]
    WaitingForSendEdc = 2,
    [Display(Name = "در انتظار ارسال به مخابرات")]
    WaitingToSendTci = 3,
    [Display(Name = "در انتظار بررسی شرکت توزیع")]
    DeliveredToEdc = 4,
    [Display(Name = "در انتظار بررسی مخابرات")]
    DeliveredToTci = 5,
    [Display(Name = "دارای نقص در شرکت توزیع")]
    DefectOnEdc = 6,
    [Display(Name = "دارای نقص در مخابرات")]
    DefectOnTci = 7,
    [Display(Name = "تایید شده در شرکت توزیع")]
    SubmitOnEdc = 8,
    [Display(Name = "تایید شده در مخابرات")]
    SubmitOnTci = 9,
    [Display(Name = "بایگانی-شرکت توزیع")]
    ArchiveEdcStage = 10,
    [Display(Name = "بایگانی-مخابرات")]
    ArchiveTciStage = 11,
    [Display(Name = "نقص در نظام مهندسی")]
    DefectOnNezam = 12,
    [Display(Name = "تایید در نظام مهندسی")]
    SubmitOnNezam = 13,
}