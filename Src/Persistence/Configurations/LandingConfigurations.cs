using System;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations
{
    // Stable GUIDs so the seed is idempotent across migrations.
    internal static class LandingSeedIds
    {
        public static Guid Ann(int n) => new($"a0000000-0000-0000-0000-0000000000{n:00}");
        public static Guid Mtg(int n) => new($"b0000000-0000-0000-0000-0000000000{n:00}");
        public static Guid Res(int m, int n) => new($"c0000000-0000-0000-0000-000000{m:00}00{n:00}");
        public static Guid Doc(int n) => new($"d0000000-0000-0000-0000-0000000000{n:00}");
        public static Guid Stat(int n) => new($"e0000000-0000-0000-0000-0000000000{n:00}");
        public static Guid Flow(int n) => new($"f0000000-0000-0000-0000-0000000000{n:00}");
        public static Guid Step(int f, int n) => new($"f{f}000000-0000-0000-0000-0000000000{n:00}");
        // /about — a singleton row plus its four child collections.
        public static Guid About => new("1a000000-0000-0000-0000-000000000001");
        public static Guid Mission(int n) => new($"1b000000-0000-0000-0000-0000000000{n:00}");
        public static Guid OrgNode(int n) => new($"1c000000-0000-0000-0000-0000000000{n:00}");
        public static Guid Board(int n) => new($"1d000000-0000-0000-0000-0000000000{n:00}");
        public static Guid Duty(int n) => new($"1e000000-0000-0000-0000-0000000000{n:00}");
    }

    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Slug).IsRequired();
            b.HasIndex(e => e.Slug).IsUnique();
            b.Property(e => e.Title).IsRequired();
            b.Property(e => e.Priority).IsRequired();

            b.HasData(
                new Announcement { Id = LandingSeedIds.Ann(1), Slug = "tarife-electrode-zamin-1405", Title = "ابلاغ تعرفه جدید اجرای الکترود زمین — اردیبهشت ۱۴۰۵", Excerpt = "تعرفه‌های پیشنهادی اجرای ارت برای سال ۱۴۰۵ ابلاغ شد.", Content = "تعرفه جدید اجرای الکترود زمین برای سال ۱۴۰۵ توسط هیئت رئیسه دفتر اجرایی نظارت برق استان کردستان تصویب و ابلاغ گردید.", Category = "تعرفه و مالی", Priority = "urgent", JalaliDate = "1405/02/01", PublishedAt = new DateTime(2026, 4, 21), Featured = true },
                new Announcement { Id = LandingSeedIds.Ann(2), Slug = "nazamnameh-daftar-ejrayi", Title = "انتشار نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی", Excerpt = "نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق مصوب هیئت مدیره سازمان در سایت منتشر شد.", Content = "نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق در بخش آرشیو اسناد این سامانه قابل دسترسی می‌باشد.", Category = "اسناد سازمانی", Priority = "info", JalaliDate = "1405/01/20", PublishedAt = new DateTime(2026, 4, 9), Featured = false },
                new Announcement { Id = LandingSeedIds.Ann(3), Slug = "bazresi-parvane-barq-1405", Title = "فراخوان ارزیابی عملکرد کارشناسان برق — بهار ۱۴۰۵", Excerpt = "ارزیابی دوره‌ای عملکرد کلیه کارشناسان فعال در بهار ۱۴۰۵ انجام می‌شود.", Content = "کارشناسانی که درخواست تمدید صلاحیت دارند باید مدارک لازم را تا پایان اردیبهشت ماه ارائه دهند.", Category = "کارشناسان", Priority = "important", JalaliDate = "1405/01/15", PublishedAt = new DateTime(2026, 4, 4), Featured = false },
                new Announcement { Id = LandingSeedIds.Ann(4), Slug = "dowload-form-namazh-paziresh", Title = "انتشار فرم‌های جدید نظارت و پذیرش پروژه‌های برق", Excerpt = "فرم‌های استاندارد نظارت و پذیرش پروژه‌های برق برای سال ۱۴۰۵ به‌روزرسانی شد.", Content = "فرم‌های جدید شامل چک‌لیست بازرسی، فرم تأییدیه اجرا و گزارش پایان کار در بخش آرشیو اسناد قابل دانلود می‌باشد.", Category = "فرم‌ها و مدارک", Priority = "info", JalaliDate = "1405/01/10", PublishedAt = new DateTime(2026, 3, 30), Featured = false }
            );
        }
    }

    public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Subject).IsRequired();
            b.HasMany(e => e.Resolutions).WithOne().HasForeignKey(r => r.MeetingId).OnDelete(DeleteBehavior.Cascade);

            b.HasData(
                new Meeting { Id = LandingSeedIds.Mtg(12), SessionNumber = 12, Subject = "بررسی تعرفه اجرای الکترود زمین و تصویب نرخ‌های جدید برای سال ۱۴۰۵", JalaliDate = "1405/02/15", Date = new DateTime(2026, 5, 5), Status = "مصوبه صادر شد", Type = "هیئت رئیسه", PdfUrl = "/documents/meeting-12.pdf", Attendees = "مدیر اجرایی,نماینده هیئت مدیره,کارشناس فنی ارشد" },
                new Meeting { Id = LandingSeedIds.Mtg(11), SessionNumber = 11, Subject = "ارزیابی عملکرد کارشناسان و بررسی تخلفات گزارش‌شده", JalaliDate = "1405/01/28", Date = new DateTime(2026, 4, 17), Status = "در حال پیگیری", Type = "هیئت رئیسه" },
                new Meeting { Id = LandingSeedIds.Mtg(10), SessionNumber = 10, Subject = "تصویب برنامه آموزشی کارشناسان برق — نیمه اول ۱۴۰۵", JalaliDate = "1405/01/14", Date = new DateTime(2026, 4, 3), Status = "مصوبه صادر شد", Type = "هیئت رئیسه", PdfUrl = "/documents/meeting-10.pdf" }
            );
        }
    }

    public class MeetingResolutionConfiguration : IEntityTypeConfiguration<MeetingResolution>
    {
        public void Configure(EntityTypeBuilder<MeetingResolution> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Text).IsRequired();

            b.HasData(
                new MeetingResolution { Id = LandingSeedIds.Res(12, 1), MeetingId = LandingSeedIds.Mtg(12), Text = "تصویب تعرفه‌های اجرای الکترود زمین برای اردیبهشت ۱۴۰۵", Status = "اجرا شده" },
                new MeetingResolution { Id = LandingSeedIds.Res(12, 2), MeetingId = LandingSeedIds.Mtg(12), Text = "الزام کارشناسان به استفاده از تعرفه جدید از ابتدای خرداد ۱۴۰۵", Status = "در دست اقدام" },
                new MeetingResolution { Id = LandingSeedIds.Res(11, 1), MeetingId = LandingSeedIds.Mtg(11), Text = "تشکیل کمیته ارزیابی سه‌نفره برای بررسی پرونده‌های تخلف", Status = "در دست اقدام" },
                new MeetingResolution { Id = LandingSeedIds.Res(10, 1), MeetingId = LandingSeedIds.Mtg(10), Text = "برگزاری ۴ دوره آموزشی در نیمه اول سال", Status = "در دست اقدام" }
            );
        }
    }

    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Title).IsRequired();
            b.Property(e => e.Category).IsRequired();

            b.HasData(
                new Document { Id = LandingSeedIds.Doc(1), Title = "نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق", Category = "نظام‌نامه", Date = new DateTime(2014, 7, 24), JalaliDate = "1393/05/02", Version = "1.0", Description = "مصوب جلسه مورخ ۱۳۹۳/۰۵/۰۲ هیئت مدیره سازمان نظام مهندسی ساختمان استان کردستان", FileSize = "2.4 MB", DownloadCount = 847, Tags = "هیئت رئیسه,شرح وظایف,مدیر اجرایی", Featured = true },
                new Document { Id = LandingSeedIds.Doc(2), Title = "قیمت‌های پیشنهادی اجرای ارت (اردیبهشت ماه ۱۴۰۵)", Category = "تعرفه", Date = new DateTime(2026, 4, 21), JalaliDate = "1405/02/01", Version = "3.0", Description = "تعرفه اجرای الکترود زمین — شامل الکترود ساده، اساسی، افقی و فونداسیون", FileSize = "1.1 MB", DownloadCount = 1243, Tags = "الکترود زمین,تعرفه,اجرا", Featured = true },
                new Document { Id = LandingSeedIds.Doc(3), Title = "چک‌لیست بازرسی تأسیسات برقی ساختمان", Category = "چک‌لیست", Date = new DateTime(2025, 9, 1), JalaliDate = "1404/06/10", Version = "2.1", Description = "چک‌لیست جامع بازرسی تأسیسات برقی ساختمان‌های مسکونی و تجاری", FileSize = "0.8 MB", DownloadCount = 632, Tags = "بازرسی,تأسیسات برقی,ساختمان", Featured = false },
                new Document { Id = LandingSeedIds.Doc(4), Title = "فرم گزارش پایان کار و پذیرش نظارت برق", Category = "فرم اجرایی", Date = new DateTime(2026, 1, 1), JalaliDate = "1404/10/11", Version = "2.0", Description = "فرم استاندارد گزارش پایان کار پروژه‌های برقی و تأییدیه نظارت", FileSize = "0.5 MB", DownloadCount = 415, Tags = "فرم,پذیرش,پایان کار", Featured = false }
            );
        }
    }

    public class StatItemConfiguration : IEntityTypeConfiguration<StatItem>
    {
        public void Configure(EntityTypeBuilder<StatItem> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Label).IsRequired();

            b.HasData(
                new StatItem { Id = LandingSeedIds.Stat(1), Label = "پروانه صادرشده", Value = 12480, Suffix = "+", IconName = "FileCheck", SortOrder = 1 },
                new StatItem { Id = LandingSeedIds.Stat(2), Label = "کارشناس ثبت‌شده", Value = 342, Suffix = "", IconName = "Users", SortOrder = 2 },
                new StatItem { Id = LandingSeedIds.Stat(3), Label = "پروژه فعال", Value = 894, Suffix = "", IconName = "Activity", SortOrder = 3 },
                new StatItem { Id = LandingSeedIds.Stat(4), Label = "بازرسی انجام‌شده", Value = 2156, Suffix = "+", IconName = "ShieldCheck", SortOrder = 4 }
            );
        }
    }

    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Name).IsRequired();
            b.Property(e => e.Message).IsRequired();
        }
    }

    public class ProcessFlowConfiguration : IEntityTypeConfiguration<ProcessFlow>
    {
        public void Configure(EntityTypeBuilder<ProcessFlow> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Key).IsRequired();
            b.HasIndex(e => e.Key).IsUnique();
            b.Property(e => e.Title).IsRequired();
            b.HasMany(e => e.Steps).WithOne().HasForeignKey(s => s.ProcessFlowId).OnDelete(DeleteBehavior.Cascade);

            b.HasData(
                new ProcessFlow { Id = LandingSeedIds.Flow(1), Key = "inspection", Title = "بازرسی برق", Subtitle = "راهنمای فرآیند بازرسی تأسیسات الکتریکی", Description = "فرآیند رسمی بازرسی تأسیسات الکتریکی ساختمان مطابق با مبحث ۱۳ مقررات ملی ساختمان، استاندارد IEC 60364-6 و BS 7671", Color = "from-blue-500 to-indigo-600", GlowColor = "rgba(99,102,241,0.25)", Icon = "🔍", SortOrder = 1 },
                new ProcessFlow { Id = LandingSeedIds.Flow(2), Key = "earth", Title = "سیستم ارت", Subtitle = "راهنمای اجرا و مستندسازی سیستم زمین", Description = "فرآیند طراحی، اجرا، اندازه‌گیری و مستندسازی سیستم الکترود زمین ساختمان مطابق با مقررات ملی ساختمان", Color = "from-emerald-500 to-teal-600", GlowColor = "rgba(16,185,129,0.25)", Icon = "⚡", SortOrder = 2 },
                new ProcessFlow { Id = LandingSeedIds.Flow(3), Key = "test-delivery", Title = "تست و تحویل", Subtitle = "فرآیند تست، بازرسی و تحویل تأسیسات الکتریکی", Description = "فرآیند رسمی تست بدو تحویل تأسیسات برق ساختمان مطابق ماده ۱۳-۳-۵ مبحث ۱۳ مقررات ملی ساختمان و استاندارد IEC 60364-6", Color = "from-amber-500 to-orange-600", GlowColor = "rgba(245,158,11,0.25)", Icon = "🧪", SortOrder = 3 }
            );
        }
    }

    public class ProcessStepConfiguration : IEntityTypeConfiguration<ProcessStep>
    {
        private static string J(params string[] items) => string.Join("\n", items);

        public void Configure(EntityTypeBuilder<ProcessStep> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Title).IsRequired();

            b.HasData(
                // --- Flow 1: inspection ---
                new ProcessStep { Id = LandingSeedIds.Step(1, 0), ProcessFlowId = LandingSeedIds.Flow(1), Number = 1, SortOrder = 1, Title = "مراجعهٔ مالک و تشکیل پرونده", Description = "برای بازرسی برق یا سیستم ارت، ابتدا مالک به شرکت توزیع مراجعه می‌کند. شرکت توزیع برای درخواست مالک یک پرونده در کارتابل دفتر اجرایی ایجاد می‌کند و ادامهٔ فرآیند در دفتر اجرایی انجام می‌شود.", Details = J("مراجعهٔ مالک به شرکت توزیع برق", "ثبت درخواست و تشکیل پرونده در کارتابل دفتر اجرایی", "ارجاع پرونده به دفتر اجرایی برای ادامهٔ فرآیند") },
                new ProcessStep { Id = LandingSeedIds.Step(1, 1), ProcessFlowId = LandingSeedIds.Flow(1), Number = 2, SortOrder = 2, Title = "دریافت درخواست بازرسی", Description = "ناظر تأسیسات الکتریکی درخواست بازرسی را تکمیل و به دفتر اجرایی ارائه می‌دهد.", RequiredDocs = J("فرم درخواست بازرسی", "کپی پروانه ساخت", "نقشه‌های تأسیسات الکتریکی") },
                new ProcessStep { Id = LandingSeedIds.Step(1, 2), ProcessFlowId = LandingSeedIds.Flow(1), Number = 3, SortOrder = 3, Title = "بررسی مدارک و صلاحیت", Description = "مدارک ناظر و مجری بررسی می‌شود. گواهینامه فنی بازرسی و پروانه اشتغال به کار معتبر باشد.", RequiredDocs = J("گواهینامه فنی بازرس", "پروانه اشتغال ناظر", "مستندات مجری برق"), Details = J("اعتبار گواهینامه فنی بازرسی تأیید شود", "صلاحیت بازرس در سامانه نظام مهندسی احراز شود") },
                new ProcessStep { Id = LandingSeedIds.Step(1, 3), ProcessFlowId = LandingSeedIds.Flow(1), Number = 4, SortOrder = 4, Title = "هماهنگی و زمان‌بندی بازدید", Description = "تاریخ و زمان بازدید میدانی با هماهنگی ناظر، مجری و مالک تعیین می‌شود.", Details = J("ارسال اطلاعیه زمان بازدید به ناظر هماهنگ‌کننده", "اطمینان از حضور مجری اجرا در محل") },
                new ProcessStep { Id = LandingSeedIds.Step(1, 4), ProcessFlowId = LandingSeedIds.Flow(1), Number = 5, SortOrder = 5, Title = "بازدید میدانی از محل", Description = "بازرس به محل ساختمان مراجعه و وضعیت اجرایی تأسیسات را از نظر انطباق با مقررات ملی ارزیابی می‌کند.", Details = J("بررسی کابل‌کشی و لوله‌گذاری", "بررسی تابلوهای توزیع و کنتور", "بررسی سیستم زمین و همبندی", "بررسی تجهیزات حفاظتی"), Tools = J("متر و ابزار اندازه‌گیری", "چراغ‌قوه بازرسی", "دوربین ثبت مستندات") },
                new ProcessStep { Id = LandingSeedIds.Step(1, 5), ProcessFlowId = LandingSeedIds.Flow(1), Number = 6, SortOrder = 6, Title = "تکمیل چک‌لیست بازرسی", Description = "چک‌لیست استاندارد بازرسی ویرایش ۱۴۰۴ مطابق IEC 60364-6، BS 7671 و استاندارد ملی ایران ۶-۱۹۳۷ تکمیل می‌شود.", RequiredDocs = J("چک‌لیست بازرسی ویرایش ۱۴۰۴", "فرم مشخصات منبع تغذیه", "جداول آرایش سیستم زمین"), Details = J("آرایش سیستم زمین: TN-C-S / TT / IT", "مشخصات انشعاب (جریان نامی، ماکزیمم دیماند)", "مشخصات کلید قطع‌کننده اصلی", "پیوستگی هادی‌های حفاظتی و همبندی") },
                new ProcessStep { Id = LandingSeedIds.Step(1, 6), ProcessFlowId = LandingSeedIds.Flow(1), Number = 7, SortOrder = 7, Title = "رسم کروکی بازرسی", Description = "کروکی محل به صورت دستی یا با استفاده از قالب DWG تهیه و ضمیمه گزارش می‌شود.", RequiredDocs = J("قالب کروکی بازرسی (.dwg)", "نمونه کروکی مرجع"), Details = J("موقعیت تابلوها، کنتورها و کلیدها مشخص شود", "مسیر کابل‌های اصلی ترسیم شود") },
                new ProcessStep { Id = LandingSeedIds.Step(1, 7), ProcessFlowId = LandingSeedIds.Flow(1), Number = 8, SortOrder = 8, Title = "تنظیم و صدور گزارش بازرسی", Description = "گزارش کامل بازرسی شامل نتایج بازدید، موارد عدم انطباق و پیشنهادات تنظیم می‌شود.", Details = J("موارد متضاد با مبحث ۱۳ مقررات ملی مستند شود", "پیشنهاد دوره بازرسی بعدی (حداکثر ... سال)", "شماره گواهینامه فنی بازرس درج شود") },
                new ProcessStep { Id = LandingSeedIds.Step(1, 8), ProcessFlowId = LandingSeedIds.Flow(1), Number = 9, SortOrder = 9, Title = "تأیید ناظر و ارسال به برق", Description = "پس از رفع نقص‌ها، ناظر تأسیسات فرم تأیید را مهر و امضاء کرده و مدارک به شرکت توزیع برق ارسال می‌شود.", RequiredDocs = J("فرم تأیید ناظر (مهر و امضاء)", "گزارش بازرسی نهایی", "کروکی تأیید شده"), Note = "صدور گواهی پایان کار منوط به تأیید این مرحله است." },

                // --- Flow 2: earth ---
                new ProcessStep { Id = LandingSeedIds.Step(2, 1), ProcessFlowId = LandingSeedIds.Flow(2), Number = 1, SortOrder = 1, Title = "بررسی نقشه‌های سیستم ارت", Description = "نقشه‌های اجرایی سیستم زمین بررسی می‌شود. نوع آرایش سیستم زمین (TN-C-S یا TT) مشخص می‌گردد.", RequiredDocs = J("نقشه سیستم زمین (.dwg)", "مدل‌های TNCS و TT"), Details = J("تعیین نوع سیستم: TN-C-S یا TT", "بررسی محل نصب الکترود زمین", "محاسبه مقاومت زمین مورد نیاز") },
                new ProcessStep { Id = LandingSeedIds.Step(2, 2), ProcessFlowId = LandingSeedIds.Flow(2), Number = 2, SortOrder = 2, Title = "تعیین روش اجرای الکترود", Description = "بر اساس نوع خاک، عمق زمین آب‌دار و مساحت، روش اجرای الکترود زمین انتخاب می‌شود.", Details = J("الکترود میله‌ای (Rod): خاک‌های عمیق", "الکترود تسمه‌ای (Strap): خاک‌های سطحی", "الکترود صفحه‌ای (Plate): فضای محدود", "الکترود پایه ساختمان (Foundation): بتون‌آرمه"), Tools = J("آزمون مقاومت ویژه خاک (Wenner Method)") },
                new ProcessStep { Id = LandingSeedIds.Step(2, 3), ProcessFlowId = LandingSeedIds.Flow(2), Number = 3, SortOrder = 3, Title = "اجرای الکترود زمین", Description = "الکترود زمین با مشخصات فنی مندرج در مبحث ۱۳ مقررات ملی ساختمان نصب می‌شود.", Details = J("عمق حداقل اجرا رعایت شود", "اتصالات با کلمپ مستقیم یا جوش احتراقی (Exothermic)", "هادی اتصال زمین: مس یا آلومینیوم با سطح مقطع استاندارد", "حفاظت از خوردگی الکترود"), Tools = J("گیره کلمپ ارت", "جوش احتراقی (Cadweld)", "الکترود مسی استاندارد") },
                new ProcessStep { Id = LandingSeedIds.Step(2, 4), ProcessFlowId = LandingSeedIds.Flow(2), Number = 4, SortOrder = 4, Title = "اندازه‌گیری مقاومت زمین", Description = "مقاومت الکترود زمین با دستگاه اندازه‌گیر استاندارد (Earth Tester) سنجیده می‌شود.", Tools = J("KYORITSU 4105A", "FLUKE 1653B", "میخ‌های کمکی اندازه‌گیری"), Details = J("روش سه‌نقطه (Three Point Method)", "فاصله‌گذاری استاندارد میخ‌ها"), IsDecision = true, DecisionYes = "مقاومت ≤ ۱۰ اهم → قبول", DecisionNo = "مقاومت > ۱۰ اهم → اصلاح" },
                new ProcessStep { Id = LandingSeedIds.Step(2, 5), ProcessFlowId = LandingSeedIds.Flow(2), Number = 5, SortOrder = 5, Title = "اصلاح در صورت عدم قبولی", Description = "در صورتی که مقاومت زمین بیش از ۱۰ اهم باشد، اقدامات اصلاحی انجام می‌شود.", Details = J("افزودن الکترود موازی", "استفاده از مواد بهبوددهنده (Backfill Compound)", "تعویض الکترود با سطح مقطع بزرگتر", "تکرار اندازه‌گیری پس از اصلاح"), Note = "تا رسیدن به مقاومت ≤ ۱۰Ω مرحله ۴ تکرار می‌شود." },
                new ProcessStep { Id = LandingSeedIds.Step(2, 6), ProcessFlowId = LandingSeedIds.Flow(2), Number = 6, SortOrder = 6, Title = "تکمیل شناسنامه ارت", Description = "شناسنامه ارت با کلیه مشخصات الکترود، هادی‌ها، مقاومت اندازه‌گیری شده و امضای مجری تکمیل می‌شود.", RequiredDocs = J("شناسنامه ارت (فرم ۱ و ۲)", "نتایج اندازه‌گیری مقاومت"), Details = J("نوع الکترود و مشخصات ثبت شود", "مقدار مقاومت زمین (Ω) درج شود", "تاریخ اندازه‌گیری ثبت شود") },
                new ProcessStep { Id = LandingSeedIds.Step(2, 7), ProcessFlowId = LandingSeedIds.Flow(2), Number = 7, SortOrder = 7, Title = "صدور مستندات مجری ارت", Description = "مستندات کامل مجری ارت شامل شناسنامه‌های تأیید شده، گزارش اندازه‌گیری و نقشه اجرایی تحویل می‌شود.", RequiredDocs = J("مستندات مجری ارت (تأیید شده)", "شناسنامه ارت ممهور", "نقشه as-built سیستم زمین"), Note = "این مستندات برای مراحل تست و تحویل الزامی است." },

                // --- Flow 3: test-delivery ---
                new ProcessStep { Id = LandingSeedIds.Step(3, 1), ProcessFlowId = LandingSeedIds.Flow(3), Number = 1, SortOrder = 1, Title = "معرفی مجری ذیصلاح تست", Description = "ناظر تأسیسات الکتریکی فرم معرفی مجری ذیصلاح تست و تحویل را به دفتر اجرایی ارائه می‌دهد.", RequiredDocs = J("فرم درخواست معرفی مجری تست و تحویل"), Details = J("مطابق ماده ۱۳-۳-۵ مبحث ۱۳ مقررات ملی ساختمان", "هماهنگی با ناظر هماهنگ‌کننده پروژه", "اطلاع‌رسانی به مالک") },
                new ProcessStep { Id = LandingSeedIds.Step(3, 2), ProcessFlowId = LandingSeedIds.Flow(3), Number = 2, SortOrder = 2, Title = "دریافت و بررسی مدارک پروژه", Description = "مدارک کامل طراح، مجری و ناظر تأسیسات الکتریکی دریافت و بررسی می‌شود.", RequiredDocs = J("اعلامیه طراح تأسیسات (مهر و امضاء)", "اعلامیه مجری تأسیسات (مهر و امضاء)", "مستندات بازرس برق", "شناسنامه ارت تأیید شده") },
                new ProcessStep { Id = LandingSeedIds.Step(3, 3), ProcessFlowId = LandingSeedIds.Flow(3), Number = 3, SortOrder = 3, Title = "آماده‌سازی تجهیزات تست", Description = "تجهیزات تست کالیبره شده آماده و مشخصات تابلوهای توزیع بررسی می‌شود.", Tools = J("FLUKE 1653B (مولتی‌فانکشن تست)", "KYORITSU 4105A (ارت‌تستر)", "کلمپ آمپرمتر", "ولت‌متر دیجیتال"), Details = J("بررسی تاریخ کالیبراسیون دستگاه‌ها", "مطالعه نقشه‌های تابلوی توزیع", "تهیه تست شیت خالی") },
                new ProcessStep { Id = LandingSeedIds.Step(3, 4), ProcessFlowId = LandingSeedIds.Flow(3), Number = 4, SortOrder = 4, Title = "تست توالی فاز و قطبیت", Description = "صحت توالی فازها و درستی قطبیت اتصالات در تمامی مدارها بررسی می‌شود.", Details = J("توالی فاز R-S-T: راستگرد (مثبت)", "قطبیت هادی‌های L و N در تمامی پریزها", "قطبیت کلیدها (فاز باید قطع شود)"), Tools = J("تستر توالی فاز", "FLUKE 1653B") },
                new ProcessStep { Id = LandingSeedIds.Step(3, 5), ProcessFlowId = LandingSeedIds.Flow(3), Number = 5, SortOrder = 5, Title = "تست مقاومت عایقی", Description = "مقاومت عایق هادی‌های مدارها با ولتاژ آزمون DC اندازه‌گیری می‌شود.", Details = J("ولتاژ آزمون: ۵۰۰V DC برای مدارهای ۲۳۰/۴۰۰V", "حداقل مقاومت: ≥ ۱ مگا‌اهم", "تست در وضعیت بدون بار", "جداسازی تجهیزات حساس (SPD، UPS)"), Tools = J("FLUKE 1653B (Insulation Mode)"), RequiredDocs = J("جدول نتایج مقاومت عایقی"), IsDecision = true, DecisionYes = "≥ ۱ MΩ → قبول", DecisionNo = "< ۱ MΩ → اصلاح عایق‌بندی" },
                new ProcessStep { Id = LandingSeedIds.Step(3, 6), ProcessFlowId = LandingSeedIds.Flow(3), Number = 6, SortOrder = 6, Title = "تست امپدانس حلقه اتصال کوتاه", Description = "امپدانس حلقه (Zs) برای تمامی مدارها اندازه‌گیری و با مقادیر مجاز مقایسه می‌شود.", Details = J("اندازه‌گیری Zs در انتهای هر مدار", "بررسی کفایت جریان خطا برای عملکرد MCB", "محاسبه IPF (Prospective Fault Current)"), Tools = J("FLUKE 1653B (Loop Impedance Mode)"), RequiredDocs = J("جدول مشخصات مدارهای تابلو توزیع") },
                new ProcessStep { Id = LandingSeedIds.Step(3, 7), ProcessFlowId = LandingSeedIds.Flow(3), Number = 7, SortOrder = 7, Title = "تست کلید جریان باقیمانده (RCD)", Description = "عملکرد و زمان قطع تمامی کلیدهای جریان باقیمانده (RCCB/RCBO) آزمون می‌شود.", Details = J("جریان آزمون: ۱×IΔn (باید < ۳۰۰ms قطع شود)", "جریان آزمون: ۵×IΔn (باید < ۴۰ms قطع شود)", "تست در هر دو نیم‌سیکل مثبت و منفی", "تست دکمه Test روی هر RCD"), Tools = J("FLUKE 1653B (RCD Test Mode)"), RequiredDocs = J("جدول نتایج تست RCD") },
                new ProcessStep { Id = LandingSeedIds.Step(3, 8), ProcessFlowId = LandingSeedIds.Flow(3), Number = 8, SortOrder = 8, Title = "تست سیستم ارت و همبندی", Description = "مقاومت الکترود زمین اندازه‌گیری و صحت پیوستگی هادی‌های حفاظتی بررسی می‌شود.", Details = J("مقاومت الکترود زمین: ≤ ۱۰ اهم", "پیوستگی هادی‌های PE در تمامی مدارها", "اتصال همبندی اصلی (MEB)", "اتصال همبندی لوله‌های فلزی"), Tools = J("KYORITSU 4105A", "FLUKE 1653B (Continuity Mode)") },
                new ProcessStep { Id = LandingSeedIds.Step(3, 9), ProcessFlowId = LandingSeedIds.Flow(3), Number = 9, SortOrder = 9, Title = "تکمیل تست شیت و گزارش", Description = "تست شیت کامل با کلیه نتایج اندازه‌گیری، امضای طراح، مجری، ناظر و بازرس تکمیل می‌شود.", RequiredDocs = J("تست شیت تأسیسات برقی (مهر و امضاء طراح)", "تست شیت (مهر و امضاء مجری)", "تست شیت (مهر و امضاء ناظر)", "گزارش بازرسی تست و تحویل"), Details = J("کلیه نتایج تست‌ها ثبت شود", "موارد عدم انطباق مستند گردد", "پیشنهاد دوره بازرسی بعدی درج شود") },
                new ProcessStep { Id = LandingSeedIds.Step(3, 10), ProcessFlowId = LandingSeedIds.Flow(3), Number = 10, SortOrder = 10, Title = "ارائه به کمیسیون و صدور گواهی", Description = "مدارک کامل به کمیسیون تأسیسات دفتر اجرایی ارائه شده و گواهی پایان کار صادر می‌شود.", RequiredDocs = J("پکیج کامل مستندات تست و تحویل", "روال اجرایی تحویل تأسیسات برقی", "فرم تأیید ناظر نهایی"), Note = "صدور گواهی پایان کار برقی منوط به تکمیل موفق کلیه مراحل تست و تحویل است." }
            );
        }
    }

    // --- /about ---------------------------------------------------------------------------
    // Seeded from the content that used to be hard-coded in web AboutPage.tsx, so the page
    // looks identical the moment the migration runs and every word becomes editable.

    public class AboutContentConfiguration : IEntityTypeConfiguration<AboutContent>
    {
        public void Configure(EntityTypeBuilder<AboutContent> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.PageTitle).IsRequired();
            b.HasMany(e => e.Missions).WithOne().HasForeignKey(m => m.AboutContentId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(e => e.OrgNodes).WithOne().HasForeignKey(n => n.AboutContentId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(e => e.BoardMembers).WithOne().HasForeignKey(m => m.AboutContentId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(e => e.Duties).WithOne().HasForeignKey(d => d.AboutContentId).OnDelete(DeleteBehavior.Cascade);

            b.HasData(new AboutContent
            {
                Id = LandingSeedIds.About,
                PageTitle = "درباره دفتر اجرایی",
                Intro = "دفتر اجرایی نظارت برق، یکی از واحدهای تخصصی سازمان نظام مهندسی ساختمان استان کردستان است که با هدف نظارت بر حسن اجرای پروژه‌های برق ساختمانی و ارتقاء ایمنی تأسیسات الکتریکی در استان فعالیت می‌نماید. این دفتر بر اساس نظام‌نامه مصوب مورخ ۱۳۹۳/۰۵/۰۲ هیئت مدیره سازمان تشکیل شده و فعالیت‌های خود را در چارچوب قوانین و مقررات ملی ساختمان انجام می‌دهد.",
                MissionsTitle = "اهداف و مأموریت",
                OrgChartTitle = "ساختار سازمانی",
                BoardTitle = "هیئت رئیسه",
                DutiesTitle = "شرح وظایف",
            });
        }
    }

    public class AboutMissionConfiguration : IEntityTypeConfiguration<AboutMission>
    {
        public void Configure(EntityTypeBuilder<AboutMission> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Title).IsRequired();

            b.HasData(
                new AboutMission { Id = LandingSeedIds.Mission(1), AboutContentId = LandingSeedIds.About, IconName = "Shield", Title = "نظارت فنی", Description = "نظارت بر حسن اجرای پروژه‌های برق ساختمانی مطابق با مقررات ملی و استانداردهای فنی", SortOrder = 1 },
                new AboutMission { Id = LandingSeedIds.Mission(2), AboutContentId = LandingSeedIds.About, IconName = "Target", Title = "ارتقاء کیفیت", Description = "ارتقاء کیفیت اجرای کارهای برق و ایمنی تأسیسات الکتریکی در استان کردستان", SortOrder = 2 },
                new AboutMission { Id = LandingSeedIds.Mission(3), AboutContentId = LandingSeedIds.About, IconName = "BookOpen", Title = "آموزش و توسعه", Description = "آموزش و توانمندسازی کارشناسان و ناظران حوزه برق ساختمان", SortOrder = 3 },
                new AboutMission { Id = LandingSeedIds.Mission(4), AboutContentId = LandingSeedIds.About, IconName = "Users", Title = "هماهنگی سازمانی", Description = "هماهنگی بین سازمان‌های مرتبط و یکپارچه‌سازی فرآیندهای اجرایی", SortOrder = 4 }
            );
        }
    }

    public class AboutOrgNodeConfiguration : IEntityTypeConfiguration<AboutOrgNode>
    {
        public void Configure(EntityTypeBuilder<AboutOrgNode> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Title).IsRequired();

            b.HasData(
                new AboutOrgNode { Id = LandingSeedIds.OrgNode(1), AboutContentId = LandingSeedIds.About, Level = 0, SortOrder = 1, Title = "هیئت مدیره سازمان نظام مهندسی" },
                new AboutOrgNode { Id = LandingSeedIds.OrgNode(2), AboutContentId = LandingSeedIds.About, Level = 1, SortOrder = 1, Title = "هیئت رئیسه دفتر اجرایی (۳ نفر)" },
                new AboutOrgNode { Id = LandingSeedIds.OrgNode(3), AboutContentId = LandingSeedIds.About, Level = 2, SortOrder = 1, Title = "مدیر اجرایی" },
                new AboutOrgNode { Id = LandingSeedIds.OrgNode(4), AboutContentId = LandingSeedIds.About, Level = 2, SortOrder = 2, Title = "نمایندگان شهرستان‌ها" }
            );
        }
    }

    public class AboutBoardMemberConfiguration : IEntityTypeConfiguration<AboutBoardMember>
    {
        public void Configure(EntityTypeBuilder<AboutBoardMember> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Name).IsRequired();

            b.HasData(
                new AboutBoardMember { Id = LandingSeedIds.Board(1), AboutContentId = LandingSeedIds.About, Name = "مدیر اجرایی دفتر", Role = "رئیس هیئت رئیسه", Description = "منتخب هیئت مدیره سازمان", SortOrder = 1 },
                new AboutBoardMember { Id = LandingSeedIds.Board(2), AboutContentId = LandingSeedIds.About, Name = "نماینده هیئت مدیره", Role = "عضو هیئت رئیسه", Description = "نماینده انتصابی هیئت مدیره", SortOrder = 2 },
                new AboutBoardMember { Id = LandingSeedIds.Board(3), AboutContentId = LandingSeedIds.About, Name = "نماینده مجمع عمومی", Role = "عضو هیئت رئیسه", Description = "منتخب مجمع عمومی", SortOrder = 3 }
            );
        }
    }

    public class AboutDutyConfiguration : IEntityTypeConfiguration<AboutDuty>
    {
        public void Configure(EntityTypeBuilder<AboutDuty> b)
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.Text).IsRequired();

            b.HasData(
                new AboutDuty { Id = LandingSeedIds.Duty(1), AboutContentId = LandingSeedIds.About, SortOrder = 1, Text = "نظارت بر اجرای صحیح ضوابط و مقررات فنی مربوط به تأسیسات برقی" },
                new AboutDuty { Id = LandingSeedIds.Duty(2), AboutContentId = LandingSeedIds.About, SortOrder = 2, Text = "بررسی و تأیید صلاحیت کارشناسان برق" },
                new AboutDuty { Id = LandingSeedIds.Duty(3), AboutContentId = LandingSeedIds.About, SortOrder = 3, Text = "صدور پروانه اشتغال و کنترل کیفیت" },
                new AboutDuty { Id = LandingSeedIds.Duty(4), AboutContentId = LandingSeedIds.About, SortOrder = 4, Text = "رسیدگی به شکایات و تخلفات" },
                new AboutDuty { Id = LandingSeedIds.Duty(5), AboutContentId = LandingSeedIds.About, SortOrder = 5, Text = "همکاری با ادارات و سازمان‌های دولتی ذیربط" },
                new AboutDuty { Id = LandingSeedIds.Duty(6), AboutContentId = LandingSeedIds.About, SortOrder = 6, Text = "برگزاری دوره‌های آموزشی و مهارت‌افزایی" },
                new AboutDuty { Id = LandingSeedIds.Duty(7), AboutContentId = LandingSeedIds.About, SortOrder = 7, Text = "تهیه و به‌روزرسانی دستورالعمل‌های فنی" },
                new AboutDuty { Id = LandingSeedIds.Duty(8), AboutContentId = LandingSeedIds.About, SortOrder = 8, Text = "تدوین تعرفه‌های خدمات مهندسی برق" }
            );
        }
    }
}
