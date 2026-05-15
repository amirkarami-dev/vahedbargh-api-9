using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using StackExchange.Redis;
using static Coreapi.Common.Enums.EngineerGradeTypeEnum;
using Coreapi.Common.ViewModels;
using Coreapi.Common.Models.IranKishModels;

namespace Coreapi.Common.Utility
{
    public static class Helper
    {

        public static DataTable LinqToDataTable<T>(IEnumerable<T> varList)
        {
            var dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varList == null) return dtReturn;

            foreach (var rec in varList)
            {
                // Use reflection to get property names, to create table, Only first time, others 

                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                                                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static DataTable LinqToDataTable<T>(T rec)
        {
            var dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (rec == null) return dtReturn;

                // Use reflection to get property names, to create table, Only first time, others 

                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (var pi in oProps)
                    {
                        var colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                                                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                var dr = dtReturn.NewRow();

                foreach (var pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                }

                dtReturn.Rows.Add(dr);
            
            return dtReturn;
        }


        public static string GenerateApiKey()
        {
            byte[] randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            byte[] hashedBytes = SHA256.HashData(randomBytes);
            string randomString = string.Empty;
            foreach (byte b in hashedBytes)
            {
                randomString += string.Format("{0:x2}", b);
            }
            return randomString;
        }
        public static string ATexto<T>(this T enumerable) where T : struct, Enum
        {
            var tipo = enumerable.GetType();
            return tipo.GetMember(enumerable.ToString())
                .Where(x => x.MemberType == MemberTypes.Field && ((FieldInfo)x).FieldType == tipo).First()
                .GetCustomAttribute<DisplayAttribute>()?.Name ?? enumerable.ToString();
        }
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()?
                .GetMember(enumValue.ToString())?
                .First()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name;
        }
        public static bool TryParsem<TEnum>(string myValue, out TEnum result) where TEnum : struct =>
            TryParsem<TEnum>(myValue, out result);
        public static bool TryParseMan<T>(string myValue, out T? result,string aa) where T : struct, IConvertible
        {
            Dictionary<string, T> displayNameMapping = new();
            if (!typeof(T).IsEnum) throw new ArgumentException("Invalid Enum");
            var displayAttributeType = typeof(DisplayAttribute);
            Enum.GetNames(typeof(T)).ToList().ForEach(name => {
                var member = typeof(T).GetMember(name).First();
                var displayAttrib = (DisplayAttribute)member.GetCustomAttributes(displayAttributeType, false).First();
                displayNameMapping.Add(displayAttrib.Name, (T)Enum.Parse(typeof(T), name));
            
            });
            
            result = displayNameMapping[myValue];
           // result = Enum.TryParse(value, out T tempResult) ? tempResult : default(T?);

            return (result == null) ? false : true;
        }
        public static bool GetDisplayNameEnum<T>(this T enumerable,string enumValue, out T? result) where T : struct, IConvertible
        {
            Dictionary<string, T> displayNameMapping = new();
            if (!typeof(T).IsEnum) throw new ArgumentException("Invalid Enum");
            var displayAttributeType = typeof(DisplayAttribute);
            Enum.GetNames(typeof(T)).ToList().ForEach(name => {
                var member = typeof(T).GetMember(name).First();
                var displayAttrib = (DisplayAttribute)member.GetCustomAttributes(displayAttributeType, false).First();
                if(displayAttrib.Name == enumValue)
                    displayNameMapping.Add(displayAttrib.Name, (T)Enum.Parse(typeof(T), name));

            });

            result = displayNameMapping[enumValue];
            // result = Enum.TryParse(value, out T tempResult) ? tempResult : default(T?);

            return (result == null) ? false : true;
        }

        public static string MiladiToShamsi(DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            var monthSolar = persianCalendar.GetMonth(dateTime).ToString();
            var daySolar = persianCalendar.GetDayOfMonth(dateTime).ToString();
            monthSolar = monthSolar.Length == 1 ? "0" + monthSolar : monthSolar;
            daySolar = daySolar.Length == 1 ? "0" + daySolar : daySolar;
            return persianCalendar.GetYear(dateTime) + "/" + monthSolar + "/" + daySolar;
        }

        public static string MiladiToShamsiForName(DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            var monthSolar = persianCalendar.GetMonth(dateTime).ToString();
            var daySolar = persianCalendar.GetDayOfMonth(dateTime).ToString();
            var second = persianCalendar.GetSecond(dateTime).ToString();
            monthSolar = monthSolar.Length == 1 ? "0" + monthSolar : monthSolar;
            daySolar = daySolar.Length == 1 ? "0" + daySolar : daySolar;
            return persianCalendar.GetYear(dateTime) + monthSolar + daySolar + "-" + second;
        }

		public static string MiladiToShamsiFileName(DateTime dateTime)
		{
			var persianCalendar = new PersianCalendar();
			var monthSolar = persianCalendar.GetMonth(dateTime).ToString();
			var daySolar = persianCalendar.GetDayOfMonth(dateTime).ToString();
			monthSolar = monthSolar.Length == 1 ? "0" + monthSolar : monthSolar;
			daySolar = daySolar.Length == 1 ? "0" + daySolar : daySolar;
			return persianCalendar.GetYear(dateTime) + monthSolar + daySolar;
		}
		public static string MiladiToShamsiForSms(DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            //DateTime dateT = p.ToDateTime(DateTime.Year, DateTime.Month, DateTime.Day, 0, 0, 0, 0);
            // return dateT.ToShortDateString();
            return persianCalendar.GetYear(dateTime) + "/" + persianCalendar.GetMonth(dateTime) + "/" +
                   persianCalendar.GetDayOfMonth(dateTime) ;
        }
        public static int GetYMDShamsiFromMiladi(DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            //DateTime dateT = p.ToDateTime(DateTime.Year, DateTime.Month, DateTime.Day, 0, 0, 0, 0);
            // return dateT.ToShortDateString();
            var daySolar = persianCalendar.GetDayOfMonth(dateTime);
            var monthSolar = persianCalendar.GetMonth(dateTime);
            var yearSolar = persianCalendar.GetYear(dateTime);
            var result = new int[3];
            result[0] = yearSolar;
            result[1] = monthSolar;
            result[2] = daySolar;
            return monthSolar;
        }
        public static bool MatchMiladiShamsiMonth(DateTime dateTime, int month)
        {
            var persianCalendar = new PersianCalendar();
            var daySolar = persianCalendar.GetDayOfMonth(dateTime);
            var monthSolar = persianCalendar.GetMonth(dateTime);
            var yearSolar = persianCalendar.GetYear(dateTime);
            var result = new int[3];
            result[0] = yearSolar;
            result[1] = monthSolar;
            result[2] = daySolar;
            return monthSolar == month;
        }
        public static string MiladiToShamsiFull(DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            var monthSolar = persianCalendar.GetMonth(dateTime).ToString();
            var daySolar = persianCalendar.GetDayOfMonth(dateTime).ToString();
            monthSolar = monthSolar.Length == 1 ? "0" + monthSolar : monthSolar;
            daySolar = daySolar.Length == 1 ? "0" + daySolar : daySolar;
            return persianCalendar.GetYear(dateTime) + "/" + monthSolar + "/" + daySolar + "_"
                  + persianCalendar.GetHour(dateTime) + ":" + persianCalendar.GetMinute(dateTime) + ":" + persianCalendar.GetSecond(dateTime);

        }

        public static string MiladiToShamsiForSupport(DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            var monthSolar = persianCalendar.GetMonth(dateTime).ToString();
            var daySolar = persianCalendar.GetDayOfMonth(dateTime).ToString();
            monthSolar = monthSolar.Length == 1 ? "0" + monthSolar : monthSolar;
            daySolar = daySolar.Length == 1 ? "0" + daySolar : daySolar;
            return persianCalendar.GetYear(dateTime) + "/" + monthSolar + "/" + daySolar + "_"
                   + persianCalendar.GetHour(dateTime) + ":" + persianCalendar.GetMinute(dateTime);

        }

        public static DateTime ShamsiToMiladi(string date)
        {
            var dateArray = date.Split('/');
            var year = Convert.ToInt32(dateArray[0]) ;
            var month = Convert.ToInt32(dateArray[1]);
            var day = Convert.ToInt32(dateArray[2]);
            var persianCalendar = new PersianCalendar();
            var julianDateTime = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            return julianDateTime;
        }



        public static int ShamsiToInt(string persianDate)
        {
            var separator = "/";
            if (!persianDate.Contains(separator))
            {
                separator = "-";
            }

            var dateParts = persianDate.Split(separator);
            if (dateParts.Length == 3 &&
                int.TryParse(dateParts[0], out var year) &&
                int.TryParse(dateParts[1], out var month) &&
                int.TryParse(dateParts[2], out var day))
            {
                var intDate = year * 10000 + month * 100 + day;
                return intDate;
            }
            else
            {
                return -1; 
            }
        }

        public static async Task<T> CallApi<T>(string apiUrl, object value, string Referer) where T : new()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Referer", Referer);
            var json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var w = client.PostAsync(apiUrl, content);
            w.Wait();

            var response = w.Result;
            if (!response.IsSuccessStatusCode) return await Task.FromResult(new T());
            var result = response.Content.ReadAsStringAsync();
            result.Wait();
            return await Task.FromResult(JsonConvert.DeserializeObject<T>(result.Result));

        }

        public static string EncodeGuid(Guid guid)
        {
            var bytes = guid.ToByteArray();

            // Standard URL-safe Base64 without padding (22 chars for a 16-byte GUID).
            // '+' → '-', '/' → '_', strip trailing "==".
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Substring(0, 22);
        }

        public static Guid DecodeGuid(string value)
        {
            // Reverse URL-safe substitutions, re-add the two stripped padding chars.
            value = value
                .Replace('-', '+')
                .Replace('_', '/')
                + "==";

            var bytes = Convert.FromBase64String(value);
            return new Guid(bytes);
        }

        public static string Tranlate(string keyValue)
        {
            switch (keyValue) 
            {
                case "amir":
                    return "sss";
                break;
                default:
                    return "نامشخص";
                break;
            }
        }

        public static int GetDiffMonth(DateTime firstTime, DateTime lastTime)
        {
            var diffMonth = ((firstTime.Year - lastTime.Year) * 12) + firstTime.Month - lastTime.Month;
            return diffMonth > 0 ? diffMonth : -diffMonth;
        }

        public static int GetDiffDay(DateTime? firstTime, DateTime? lastTime)
        {
            if (lastTime is null || firstTime is null) return 0;
            // Calculate the difference between the two DateTime objects
            var difference = lastTime.Value - firstTime.Value;

            // Return the total number of days as an integer
            return (int)difference.TotalDays;
        }
        public static double GetEngGradeFactor(IEnumerable<EngineerHistoryViewModel> historyViewModels, DateTime allotmentDate, int addDifDay)
        {
            
            var allotmentDateQuarter = allotmentDate.AddDays(addDifDay);

            var engHistory = historyViewModels
                .Where(w => w.JulianIssueDate <= allotmentDateQuarter)
                .Where(w => w.JulianValidityDate >= allotmentDate)
                .Where(w=>w.WorkPermitTypeEnum is WorkPermitTypeEnum.None or WorkPermitTypeEnum.Inspection or WorkPermitTypeEnum.InspectionAndEarth)
                .MaxBy(a => a.JulianIssueDate);


            if (engHistory == null) return 0;

            var getGradeFactor = engHistory.EngineerGradeTypeEnum switch
            {
                FirstBase => 2,
                SecondBase => 1.5,
                ThirdBase => 1,
                SeniorDegree => 2.5,
                _ => 0
            };
            return getGradeFactor;



        }

        public static double GetEngGradeFactorSingle( EngineerGradeTypeEnum engGradeType, DateTime workPermitDate, DateTime allotmentDate)
        {
            

            var getGradeFactor = engGradeType switch
            {
                FirstBase => 2,
                SecondBase => 1.5,
                ThirdBase => 1,
                SeniorDegree => 2.5,
                _ => 0
            };

            return getGradeFactor;

        }
        public static double GetEngGradeFactorForErt(IEnumerable<EngineerHistoryViewModel> historyViewModels, DateTime allotmentDate , int addDifDay)
        {
            var allotmentDateQuarter = allotmentDate.AddDays(addDifDay);

            var engHistory = historyViewModels
                .Where(w => w.JulianIssueDate <= allotmentDateQuarter)
                .Where(w=>w.JulianValidityDate >= allotmentDate)
                .Where(w=>w.WorkPermitTypeEnum is WorkPermitTypeEnum.None or WorkPermitTypeEnum.Earth or WorkPermitTypeEnum.InspectionAndEarth)
                .MaxBy(a => a.JulianIssueDate);


            if (engHistory == null) return 0;
            var getGradeFactor = engHistory.EngineerGradeTypeEnum switch
            {
                FirstBase => 2,
                SecondBase => 1.5,
                ThirdBase => 1,
                SeniorDegree => 2.5,
                _ => 0
            };

            return getGradeFactor;
        }

        public static double GetEngGradeFactorForErtSingle(EngineerGradeTypeEnum engGradeType, DateTime workPermitDate, DateTime allotmentDate)
        {

            var getGradeFactor = engGradeType switch
            {
                FirstBase => 2,
                SecondBase => 1.5,
                ThirdBase => 1,
                SeniorDegree => 2.5,
                _ => 0
            };
            return getGradeFactor;
        }

        public static string GetInspectionDesEnum(int keyValue)
        {
            return keyValue switch
            {
                0 => "کنترل حریم شبکه های برق در حالت دایم(ساختمان تکمیل شده) و در حالت موقت(پیش آمدگی که عمدتا در نصب داربست برای نماکاری رخ میدهد)مطابق مقررات شرکت توزیع برق و پیوست 7 از مبحث 13 مقررات ملی ساختمان",
                1 => "کنترل محل و مشخصات پست زمینی در صورت وجود مطابق مقررات شرکت توزیع برق و فصل 5 از مبحث 13 مقررات ملی ساختمان ",
                2 => "کنترل محل تابلوی کنتور و تابلو خانه و ابعاد آن در صورت وجود مطابق مقررات وزارت نیرو و مبحث 13 مقررات ملی ساختمان",
                3 => "طرح تابلوی کنتور مطابق با استاندارد و مشخصات شرکت توزیع برق",
                4 => "کنترل فاصله استاندارد تابلوی کنتور با سایر تاسیسات گاز و تلفن و غیره مطابق مقررات وزارت نیرو و  مبحث 13  و 17 مقررات ملی ساختمان (فاصله حداقل 60 سانتیمتری از کنتور آب و 50 سانتیمتری از کنتور گاز و 30 سانتیمتری از سایر لوله های تاسیسات مکانیکی)",
                5 => "رنگ آمیزی تابلوی برق اصلی(mdp) از نوع الکترواستاتیک پاششی",
                6 => "کنترل بالاترین و پایین ترین وضعیت محل نمراتور (آکس تابلو حداقل در ارتفاع 170 سانتیمتری و حداکثر در ارتفاع 200 سانتیمتری باشد)",
                7 => "کنترل فضای آزاد حداقل 120 سانتی متر در مقابل تابلوی کنتور",
                8 => "کنترل IP متناسب با توجه به شرایط محیطی نصب تابلو (OUTDOOR/INDOOR)",
                9 => "اتصال بین شینه های نول و ارت در تابلوی اصلی برق با شینه مسی قابل جدا شدن مطابق بند (ج)13-6-1-2 از مبحث 13 مقررات ملی ساختمان",
                10 => "تجهیزات داخل تابلوی برق اعم از وسایل قطع و وصل و حفاظتی ، داکت های عبور سیم ، ترمینالها و وسایل اندازه گیری و شینه ها و سرسیم و شماره گذاری و از نوع استاندارد می باشد؟ ( بند 13-6-2 از مبحث 13 مقررات ملی ساختمان)",
                11 => "کنترل و اجرای سیستم اتصال زمین با مواد و مصالح استاندارد و روش اجرا مطابق با آمپراژ درخواستی ساختمان (بند 13-5-4 از مبحث 13 مقررات ملی ساختمان)",
                12 => "کنترل رعایت حداقل فاصله  چاه های ارت (بند 13-5-4 از مبحث 13 مقررات ملی ساختمان)",
                13 => "کنترل سطح مقطع هادی اتصال زمین با توجه به حداقل استاندارد و همچنین سطح مقطع سیم چهارم در سیستم های تغذیه سه فاز (هادی اتصال زمین با سطح مقطع حداقل 25 میلیمتر مربع و هادی همبندی روکشدار با سطح مقطع حداقل 6 میلیمتر مربع)(پیوست 1 بند پ10-1 از مبحث 13 مقررات ملی ساختمان)",
                14 => "در نظر گرفتن تمهیدات لازم برای جلوگیری از صدمه به کابل های برق در زمان کابل کشی \r\n(بند 13-7-2 از مبحث 13 مقررات ملی ساختمان)\r\n",
                15 => "کنترل کانال لوله و یا مسیر ورودی کابل های شرکت توزیع برابر بند 13-7-2 از مبحث 13",
                16=> "کنترل کردن چک لیست تابلوی برق و انطباق آن با مشخصات تابلو",//first
                17=> "کنترل کانال لوله گذاری و نصب نوار خطر زرد رنگ",
                18=> "کنترل مسیر کابل و عدم استفاده از زانو",
                19=> "کنترل عبور طناب از داخل لوله",
                20=> "تهیه یک شاخه لوله گالوانیزه نمره 6 فارسی بر شده برای انشعابات زمینی",
                21=> "کنترل و نصب ارت باکس در نزدیکی محل الکترود زمین",
                22=> "کنترل و  نصب شینه TNCS در انشعابات هوایی",
                23=> "آیا در موارد ضروری سکویی در زیر تابلو برای ایمنی بیشتر و همچنین جلوگیری از برخورد اتومبیل با تابلو ایجاد شده است؟",
                24=> "درج علامت خطر و شماره تماس اضطراری شرکت توزیع با رنگ زرد بر روی تابلو کنتور",
                25=> "سایز شینه ارت تابلوی کنتور متناسب با تعداد کنتورها و مصارف ساختمان",
                _ => "توضیحات ندارد"
            };
        }

        public static string GetCheckListEnum(int keyValue)
        {
            return keyValue switch
            {
                0 => "سایر",
                1 => "کنترل محل و مشخصات پست زمینی مطابق مقررات وزارت نیرو در صورت وجود",
                2 => "کنترل محل تابلوی کنتور و تابلو خانه و ابعاد آن مطابق مقررات وزارت نیرو در صورت وجود",
                3 => "طرح تابلوی کنتور مطابق با استاندارد و مشخصات شرکت توزیع برق",
                4 => "کنترل فاصله استاندارد تابلوی کنتور با سایر تاسیسات مانند گاز و تلفن و غیره",
                5 => "رنگ آمیزی تابلوی برق اصلی (MDP) از نوع الکترواستاتیک پاششی می باشد",
                6 => "کنترل بالاترین و پایین ترین وضعیت محل نمراتور ",
                _ => "توضیحات ندارد"
            };
        }

        public static bool CanDoProcess(EngineerGradeTypeEnum gradeTypeEnum, BuildingGroupTypeEnum buildingGroupTypeEnum)
        {
            return gradeTypeEnum switch
            {
                ThirdBase when buildingGroupTypeEnum is BuildingGroupTypeEnum.G3 or BuildingGroupTypeEnum.G4 => false,
                SecondBase when buildingGroupTypeEnum is BuildingGroupTypeEnum.G4 => false,
                _ => true
            };
        }

        public static string GetDoGroupBuilding(EngineerGradeTypeEnum engineerGradeTypeEnum)
        {
            return engineerGradeTypeEnum switch
            {
                ThirdBase => "گروه:الف،ب",
                SecondBase => "گروه:الف،ب،ج",
                _ => "گروه:الف،ب،ج،د"
            };
        }

        public static long GetAmountErtSystemType(ErtSystemTypeEnum ertSystemTypeEnum, long tariff,float factor,int foundationElectrodeArea)
        {
            return ertSystemTypeEnum switch
            {
                //ErtSystemTypeEnum.E1 or 
                //    ErtSystemTypeEnum.E2 or 
                //    ErtSystemTypeEnum.E3 or 
                //    ErtSystemTypeEnum.E4 or 
                //    ErtSystemTypeEnum.E5 => Convert.ToInt64(tariff * factor),
                //ErtSystemTypeEnum.E6 => Convert.ToInt64(tariff * factor * foundationElectrodeArea),
                //_ => 0,
                ErtSystemTypeEnum.E1 => 7000000,
                ErtSystemTypeEnum.E2 => 7000000,
                ErtSystemTypeEnum.E3 => 7000000,
                ErtSystemTypeEnum.E4 => 9000000,
                ErtSystemTypeEnum.E5 => 9000000,
                ErtSystemTypeEnum.E6 => 9000000,
                _ => 0,
            };
        }

        public static long GetAmountTestAndDelivery(long tariff, int area, float factor)
        {
            //return Convert.ToInt64(tariff * area * factor);
            return 0;
        }

        public static long GetAmountInspection(long tariff, int area, float factor, long minTariff)
        {
            var amount = Convert.ToInt64(tariff * area * factor);
            return amount < minTariff ? minTariff : Convert.ToInt64(tariff * area * factor);
        }

        public static long GetAmountProjectServices(long amount)
        {
            var fee = Convert.ToInt64(amount * 0.02);
            return fee;
        }

        public static long GetAmountSupervision(long tariff, int area, float factor)
        {
            var amount = Convert.ToInt64(tariff * area * factor);
            return amount;
        }

        public static string ReturnIranKishError(string code)
        {
            return code switch
            {
                "901" => $" درخواست نا معتبر است  {code}",
                "902" => $" پارامترهای اضافی درخواست نامعتبر می باشد  {code}",
                "903" => $" شناسه پرداخت نامعتبر می باشد {code}",
                "904" => $" اطلاعات مرتبط با قبض نا معتبر می باشد {code}",
                "905" => $" شناسه درخواست نامعتبر می باشد {code}",
                "906" => $" درخواست تاریخ گذشته است {code}",
                "907" => $" آدرس بازگشت نتیجه پرداخت نامعتبر می باشد {code}",
                "909" => $" پذیرنده نامعتبر می باشد {code}",
                "910" => $" پارامترهای مورد انتشار پرداخت تسهیمی تامین نگردیده است {code}",
                "911" => $" پارامترهای مورد انتشار پرداخت تسهیمی نا معتبر یا دارای اشکال می باشد {code}",
                "912" => $" تراکنش درخواستی برای پذیرنده فعال نیست {code}",
                "913" => $" تراکنش تسهیم برای پذیرنده  فعال نیست {code}",
                "914" => $"  آدرس آی پی دریافتی درخواست نا معتبر می باشد {code}",
                "915" => $"   شماره پایانه نامعتبر می باشد {code}",
                "916" => $"   شماره پذیرنده نا معتبر می باشد {code}",
                "917" => $"   نوع تراکنش اعلام شده در خواست نا معتبر می باشد {code}",
                "918" => $"   پذیرنده فعال نیست {code}",
                "919" => $"   مبالغ تسهیمی ارائه شده با توجه به قوانین حاکم بر وضعیت تسهیم پذیرنده ، نا معتبر است {code}",
                "920" => $"   شناسه نشانه نامعتبر می باشد {code}",
                "921" => $"   شناسه نشانه نامعتبر و یا منقضی شده است {code}",
                "922" => $" نقض امنیت درخواست  {code}",
                "923" => $" ارسال شناسه پرداخت در تراکنش قبض مجاز نیست  {code}",
                "928" => $" مبلغ مبادله شده نا معتبر می باشد  {code}",
                "929" => $" شناسه پرداخت ارائه شده با توجه به الگوریتم متناظر نا معتبر می باشد  {code}",
                "930" => $" کد ملی ارائه شده نا معتبر می باشد  {code}",
                "96" => $" General failure  {code}",
                _ => $"result:{code} desc:خطای نامشخص"
            };
        }

        public static string ReturnIranKishMessageAfterTransaction(int errorCode)
        {
            switch (errorCode)
            {
                case 5:
                    return "5 از انجام تراکنش صرف نظر شد";
                case 3:
                    return "3 پذیرنده فروشگاهی نا معتبر است";
                case 64:
                    return "64 مبلغ تراکنش نادرست است،جمع مبالغ تقسیم وجوه برابر مبلغ کل تراکنش نمی باشد";
                case 94:
                    return "94 تراکنش تکراری است";
                case 25:
                    return "25 تراکنش اصلی یافت نشد";
                case 77:
                    return "77 روز مالی تراکنش نا معتبر است";
                case 97:
                    return "97 کد تولید کد اعتبار سنجی نا معتبر است";
                case 30:
                    return "30 فرمت پیام نادرست است";
                case 86:
                    return "86 شتاب در حال Off Sign است";
                case 55:
                    return "55 رمز کارت نادرست است";
                case 40:
                    return "40 عمل درخواستی پشتیبانی نمی شود";
                case 57:
                    return "57 انجام تراکنش مورد درخواست توسط پایانه انجام دهنده مجاز نمی باشد";
                case 58:
                    return "58 انجام تراکنش مورد درخواست توسط پایانه انجام دهنده مجاز نمی باشد";
                case 63:
                    return "63 تمهیدات امنیتی نقض گردیده است";
                case 96:
                    return "96 قوانین سامانه نقض گردیده است ، خطای داخلی سامانه";
                case 2:
                    return "2 تراکنش قبال برگشت شده است";
                case 54:
                    return "54 تاریخ انقضا کارت سررسید شده است";
                case 62:
                    return "62 کارت محدود شده است";
                case 75:
                    return "75 تعداد دفعات ورود رمز اشتباه از حد مجاز فراتر رفته است";
                case 14:
                    return "14 اطالعات کارت صحیح نمی باشد";
                case 51:
                    return "51 موجودی حساب کافی نمی باشد";
                case 56:
                    return "56 اطالعات کارت یافت نشد";
                case 61:
                    return "61 مبلغ تراکنش بیش از حد مجاز است";
                case 65:
                    return "65 تعداد دفعات انجام تراکنش بیش از حد مجاز است";
                case 78:
                    return "78 کارت فعال نیست";
                case 79:
                    return "79 حساب متصل به کارت بسته یا دارای اشکال است";
                default:
                    return "Unknown error code";
            }
        }

		public static string RemoveDiacritics(string text)
		{
			var normalized = text.Normalize(NormalizationForm.FormD);
			var sb = new StringBuilder();

			foreach (var ch in normalized)
				if (!char.GetUnicodeCategory(ch)
					 .Equals(System.Globalization.UnicodeCategory.NonSpacingMark))
					sb.Append(ch);

			return sb.ToString();
		}

	}
}
