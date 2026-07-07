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
}
