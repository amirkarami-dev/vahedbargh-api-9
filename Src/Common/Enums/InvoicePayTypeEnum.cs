using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum InvoicePayTypeEnum
    {
        [Display(Name = "مرحله ایجاد پرونده")]
        CreateProjectStage = 0,
        [Display(Name = "جهت بازرسی")]
        ExpertStage = 1,
        [Display(Name = "جهت مجری ارت")]
        ErtStage = 2,
        [Display(Name = "جهت تست و تحویل")]
        TestAndDelivery = 3,
        [Display(Name = "درصد نظام مهندسی")]
        NezamStage = 4,
        [Display(Name = "جهت تایید ارت")]
        ErtApprovedStage = 5,
        [Display(Name = "هزینه خدمات")]
        ProjectServices = 6,

    }
}
