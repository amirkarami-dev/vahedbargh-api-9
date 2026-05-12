using System.Collections.Generic;
using System;

namespace Coreapi.Common.Utility;

public class SmsHelper
{
    private static readonly Dictionary<int, string> TemplateDictionary = new Dictionary<int, string>
    {
        { 14646, "سامانه برق نظام مهندسی\nشماره پرونده:[param1]\nشماره تقاضا:[param2]\nآدرس جهت بارگذاری مدارک:\nwww.kurdnezambargh.ir/ep?[param3]" },
        { 14016, "برق نظام مهندسی\nپرونده:[param1]\nوضعیت:[param2]\nتاریخ:[param3]\nتعیین وضعیت و اعلام نظر بفرمایید" },
        { 14015, "کد تایید شما: [param1]\nواحد برق نظام مهندسی" },
        { 13812, "کد تایید شما: [param1]\nواحد گاز" },
        { 13767, "واحد گاز\nشماره پرونده:[param1]\nتاریخ:[param2]\nمبلغ:[param3]\nwww.kurdvahedgas.ir/tp?[param4]" },
        { 12590, "اطلاعیه واحد گاز\n[param1]\n[param2]\n[param3]\nتاریخ:[param4]" },
        { 12589, "اطلاعیه واحد گاز\n[param1]\n[param2]\n[param3]\n[param4]\nتاریخ:[param5]" },
        { 12053, "واحدگاز\nواریزی شما تاییدشد\nشماره پرونده:[param1]\nتاریخ:[param2]\nمبلغ:[param3]\nکدرهگیری:[param4]" },
        { 10558, "برق نظام مهندسی\nبازرسی ملکی شما ثبت شد\nتاریخ:[param1]\nشماره پرونده:[param2]\nشماره تقاضا:[param3]" },
        { 10443, "برق نظام مهندسی\nواریزی شما تاییدشد\nشماره پرونده:[param1]\nتاریخ:[param2]\nمبلغ:[param3]\nکدرهگیری:[param4]" },
        { 9857, "برق نظام مهندسی\nپرونده شما تایید و به شرکت برق ارسال شد\nتاریخ:[param1]\nشماره:[param2]" },
        { 9808, "برق نظام مهندسی\nپرونده:[param1]\nتوسط کارشناس:[param2]\nپذیرفته شد\nتاریخ:[param3]" },
        { 9593, "پرداخت به برق نظام مهندسی\nپرونده:[param1]\nتاریخ:[param2]\nمبلغ:[param3]\nwww.kurdnezambargh.ir/tp?[param4]" },
        { 9385, "برق نظام مهندسی\nدرخواست تست و تحویل ملک شما ثبت شد\nناظر:[param1]\nتاریخ:[param2]\nشماره پرونده:[param3]" },
        { 9382, "برق نظام مهندسی\nپرونده:[param1]\nمالک:[param2]\nمجری:[param3]\nموبایل:[param4]\nمجری تست وتحویل تعیین گردید\nتاریخ:[param5]" },
        { 9312, "برق نظام مهندسی\nشماره:[param1]\nمرحله:[param2]\nکارشناس:[param3]\nموبایل:[param4]\nتاریخ:[param5]" },
        { 9311, "تخصیص برق نظام مهندسی\nمالک:[param1]\nموبایل:[param2]\nپرونده:[param3]\nتاریخ:[param4]" },
        { 8868, "سامانه برق نظام مهندسی\nبرای پرونده:[param1]\nمبلغ [param2] ریال برداشت گردید\n[param3]" },
        { 8867, "سامانه برق نظام مهندسی\nبرای پرونده:[param1]\nمبلغ [param2] ریال واریز گردید\n[param3]" },
        { 8700, "برق نظام مهندسی\nیک پروژه چک لیست تابلو برای شما ارسال گردید\nتاریخ:[param1]\nشماره پرونده:[param2]" },
        { 8375, "برق نظام مهندسی\nبازرسی ملکی شما ثبت شد\nتاریخ:[param1]\nشماره پرونده:[param2]" },
        { 7396, "اطلاعیه برق نظام مهندسی\nموضوع:[param1]\n[param2]\n[param3]\nتاریخ:[param4]" },
        { 7395, "سامانه برق نظام مهندسی\nآدرس:www.kurdnezambargh.ir\nنام کاربری:[param1]\nرمزعبور:[param2]\nمهروامضا+جواز خودرادرقسمت فایلهای من آپلودکنید" },
        { 5519, "سامانه برق نظام مهندسی\nنام کاربری:[param1]\nرمزعبور:[param2]\nآدرس ورود:\nwww.kurdnezambargh.ir/login?[param3]" },
        { 5131, "اطلاعیه واحدگاز\n[param1]\n[param2]" },
        { 4662, "واریزی واحد گاز به حساب شما\nمبلغ(ریال):[param1]\nتاریخ:[param2]" },
        { 3906, "واحد گاز\nنقشه گازکشی به شرکت گاز ارسال شد\nتاریخ:[param1]\nشماره:[param2]\nمالک:[param3]" },
        { 3573, "واحد گاز\nنقشه گازکشی در واحد گاز تایید شد\nتاریخ:[param1]\nشماره:[param2]\nمالک:[param3]" },
        { 3572, "واحد گاز\nنقشه گازکشی شما تایید و به شرکت گاز ارسال شد\nتاریخ:[param1]\nشماره:[param2]" },
        { 3464, "از کیف پول شما مبلغ [param1] ریال برداشت شد\n[param2]\nwww.kurdvahedgas.ir" },
        { 3461, "اطلاعیه واحد گاز\n[param1]\n[param2]\nتاریخ:[param3]" },
        { 3427, "واحد گاز\nشماره:[param1]\nمرحله:[param2]\nمجری:[param3]\nتاریخ:[param4]" },
        { 3289, "واحد گاز\nپرونده ملک:[param1] ثبت شد.\nتاریخ:[param2]\nشماره:[param3]\nمجری:[param4]" },
        { 3168, "واحد گاز\nپرداخت به مجری:[param1]\nتاریخ:[param2]\nمبلغ:[param3]\nwww.kurdvahedgas.ir/tp?[param4]" },
        { 3046, "واحد گاز\n[param1]\nشماره پرونده:[param2]" },
        { 2853, "واحد گاز\nسریال پکیج را در آدرس زیر تاییدکنید\nwww.kurdvahedgas.ir/sp?[param1]" },
        { 2851, "واحد گاز\nنام کاربری:[param1]\nرمزعبور:[param2]\nآدرس ورود:\nwww.kurdvahedgas.ir/login?[param3]" },
        { 2846, "واحد گاز\nشماره:[param1]\nمرحله:[param2]\nکارشناس:[param3]\nموبایل:[param4]\nتاریخ:[param5]" },
        { 2819, "واحدگازکردستان\nآدرس:www.kurdvahedgas.ir\nنام کاربری:[param1]\nرمزعبور:[param2]\nمهروامضا+پروانه خودرادرقسمت فایلهای من آپلودکنید" },
        { 2790, "واحد گاز\nپرونده توسط نصاب تایید شد\nشماره:[param1]\nنصاب:[param2]\nمالک:[param3]" },
        { 2738, "واحدگازکردستان\nآدرس:www.kurdvahedgas.ir\nنام کاربری:[param1]\nرمزعبور:[param2]\nمهروامضا+جواز خودرادرقسمت فایلهای من آپلودکنید" },
        { 2661, "واحد گاز\nشماره:[param1]\nمرحله:[param2]\nکارشناس:[param3]\nتاریخ:[param4]" },
        { 2660, "تخصیص واحد گاز\nمالک:[param1]\nشماره:[param2]\nتاریخ:[param3]" },
        { 899, "کیف پول شما به مبلغ [param1] ریال شارژ شد\n[param2]\nwww.kurdvahedgas.ir" },
        { 15453, "واحد گاز\r\nجهت قبول/رد پرونده کارتابل خودراچک کنید\r\nتاریخ:[param1]" },
        { 17049, "واحد گاز\r\nبه تیکت پشتیبانی شما پاسخ داده شد\r\nشماره:[param1]" },
        { 18782, "واحدنظارت گازرسانی\nمالک محترم\nهزینه های دریافتی از شما بابت\nپرونده:[param2]\n مطابق تعرفه مصوب به مبلغ:[param1]\nتاریخ:[param3] " },
		{ 19096, "واحد گاز\r\nجهت ثبت سریال به کارتابل خودمراجعه کنید\r\nشماره:[param1]" },
		{ 19097, "واحد گاز\r\nجهت دریافت نام کاربری و رمز عبور به واحد گاز(درسایت اداری)مراجعه فرمایید\r\n" }
	};

    public static string BuildMessage(int templateId, params string[] parameters)
    {
        if (!TemplateDictionary.TryGetValue(templateId, out var template))
        {
            return "0";
        }

        var message = template;
        for (var i = 0; i < parameters.Length; i++)
        {
            message = message.Replace($"[param{i + 1}]", parameters[i]);
        }

        return message;
    }
}