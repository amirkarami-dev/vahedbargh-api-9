using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum FileTypeEnum
    {
        [Display(Name = "نامشخص")]
        None = 0,
        [Display(Name = "کارت ملی")]
        IdCard=1,
        [Display(Name = "نقشه برق")]
        ElectPlan=2,
        [Display(Name = "جواز مرتبط")]
        RelatedPermit = 3,
        [Display(Name = "شناسنامه ارت")]
        ErtMap = 4,
        [Display(Name = "چک لیست تابلو برق")]
        CheckListBoard = 5,
        [Display(Name = "کروکی")]
        Crooky = 6,
        [Display(Name = "کروکی احداث شبکه")]
        ElectNetwork = 7,
        [Display(Name = "فرم تست و تحویل")]
        TestAndDelivery = 8,
        [Display(Name = "فرم تایید شمار3")]
        ApprovedComment = 9,
        [Display(Name = "فرم تایید چک لیست")]
        ApprovedCheckList = 10,
        [Display(Name = "فرم تایید مجری ارت")]
        ApprovedErtExecutor = 11,
        [Display(Name = "فرم تایید نظام مهندسی")]
        ApprovedNezam = 12,
        [Display(Name = "فرم تایید ارسال به شرکت توزیع")]
        ApprovedSentToElect = 13,
        [Display(Name = "کروکی چیدمان الکترودها")]
        CrookyOfElectrode = 14,
        [Display(Name = "فرم تایید ناظر")]
        SupervisorApproveForm = 15,
        [Display(Name = "مستندات بازرس برق")]
        ExpertDocument = 16,
        [Display(Name = "مستندات مجری ارت")]
        ErtDocument = 17,
        [Display(Name = "مستندات مجری تست و تحویل")]
        TestAndDeliveryDocument = 18,
        [Display(Name = "مستندات نظارت")]
        SupervisionDocument = 19,
        [Display(Name = "مربوط به اطلاعیه")]
        ForAnnouncement = 20,
        [Display(Name = "سایر")]
        Other = 21,
        [Display(Name = "اخطاریه")]
        Notice = 22,
        [Display(Name = "نقشه ازبیلت")]
        AzbuiltMap = 23,
		[Display(Name = "افزایش ظرفیت")]
		ForUpperCapacity = 24

	}
}
