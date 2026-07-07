using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coreapi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLandingContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // NOTE: only the public landing tables are created here. The spurious
            // Clients.ApiKey default-value alter (a pre-existing random-default drift)
            // was intentionally removed to keep this migration surgical.
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Excerpt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JalaliDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Featured = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JalaliDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Featured = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JalaliDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attendees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingResolutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingResolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingResolutions_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Category", "Content", "Excerpt", "Featured", "ImageUrl", "JalaliDate", "Priority", "PublishedAt", "Slug", "Title" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "تعرفه و مالی", "تعرفه جدید اجرای الکترود زمین برای سال ۱۴۰۵ توسط هیئت رئیسه دفتر اجرایی نظارت برق استان کردستان تصویب و ابلاغ گردید.", "تعرفه‌های پیشنهادی اجرای ارت برای سال ۱۴۰۵ ابلاغ شد.", true, null, "1405/02/01", "urgent", new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "tarife-electrode-zamin-1405", "ابلاغ تعرفه جدید اجرای الکترود زمین — اردیبهشت ۱۴۰۵" },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "اسناد سازمانی", "نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق در بخش آرشیو اسناد این سامانه قابل دسترسی می‌باشد.", "نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق مصوب هیئت مدیره سازمان در سایت منتشر شد.", false, null, "1405/01/20", "info", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "nazamnameh-daftar-ejrayi", "انتشار نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی" },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), "کارشناسان", "کارشناسانی که درخواست تمدید صلاحیت دارند باید مدارک لازم را تا پایان اردیبهشت ماه ارائه دهند.", "ارزیابی دوره‌ای عملکرد کلیه کارشناسان فعال در بهار ۱۴۰۵ انجام می‌شود.", false, null, "1405/01/15", "important", new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "bazresi-parvane-barq-1405", "فراخوان ارزیابی عملکرد کارشناسان برق — بهار ۱۴۰۵" },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), "فرم‌ها و مدارک", "فرم‌های جدید شامل چک‌لیست بازرسی، فرم تأییدیه اجرا و گزارش پایان کار در بخش آرشیو اسناد قابل دانلود می‌باشد.", "فرم‌های استاندارد نظارت و پذیرش پروژه‌های برق برای سال ۱۴۰۵ به‌روزرسانی شد.", false, null, "1405/01/10", "info", new DateTime(2026, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "dowload-form-namazh-paziresh", "انتشار فرم‌های جدید نظارت و پذیرش پروژه‌های برق" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "Category", "Date", "Description", "DownloadCount", "Featured", "FileSize", "FileUrl", "JalaliDate", "Tags", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("d0000000-0000-0000-0000-000000000001"), "نظام‌نامه", new DateTime(2014, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "مصوب جلسه مورخ ۱۳۹۳/۰۵/۰۲ هیئت مدیره سازمان نظام مهندسی ساختمان استان کردستان", 847, true, "2.4 MB", null, "1393/05/02", "هیئت رئیسه,شرح وظایف,مدیر اجرایی", "نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق", "1.0" },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), "تعرفه", new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "تعرفه اجرای الکترود زمین — شامل الکترود ساده، اساسی، افقی و فونداسیون", 1243, true, "1.1 MB", null, "1405/02/01", "الکترود زمین,تعرفه,اجرا", "قیمت‌های پیشنهادی اجرای ارت (اردیبهشت ماه ۱۴۰۵)", "3.0" },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), "چک‌لیست", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "چک‌لیست جامع بازرسی تأسیسات برقی ساختمان‌های مسکونی و تجاری", 632, false, "0.8 MB", null, "1404/06/10", "بازرسی,تأسیسات برقی,ساختمان", "چک‌لیست بازرسی تأسیسات برقی ساختمان", "2.1" },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), "فرم اجرایی", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "فرم استاندارد گزارش پایان کار پروژه‌های برقی و تأییدیه نظارت", 415, false, "0.5 MB", null, "1404/10/11", "فرم,پذیرش,پایان کار", "فرم گزارش پایان کار و پذیرش نظارت برق", "2.0" }
                });

            migrationBuilder.InsertData(
                table: "Meetings",
                columns: new[] { "Id", "Attendees", "Date", "JalaliDate", "Notes", "PdfUrl", "SessionNumber", "Status", "Subject", "Type" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000010"), null, new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "1405/01/14", null, "/documents/meeting-10.pdf", 10, "مصوبه صادر شد", "تصویب برنامه آموزشی کارشناسان برق — نیمه اول ۱۴۰۵", "هیئت رئیسه" },
                    { new Guid("b0000000-0000-0000-0000-000000000011"), null, new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "1405/01/28", null, null, 11, "در حال پیگیری", "ارزیابی عملکرد کارشناسان و بررسی تخلفات گزارش‌شده", "هیئت رئیسه" },
                    { new Guid("b0000000-0000-0000-0000-000000000012"), "مدیر اجرایی,نماینده هیئت مدیره,کارشناس فنی ارشد", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "1405/02/15", null, "/documents/meeting-12.pdf", 12, "مصوبه صادر شد", "بررسی تعرفه اجرای الکترود زمین و تصویب نرخ‌های جدید برای سال ۱۴۰۵", "هیئت رئیسه" }
                });

            migrationBuilder.InsertData(
                table: "StatItems",
                columns: new[] { "Id", "IconName", "Label", "SortOrder", "Suffix", "Value" },
                values: new object[,]
                {
                    { new Guid("e0000000-0000-0000-0000-000000000001"), "FileCheck", "پروانه صادرشده", 1, "+", 12480 },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), "Users", "کارشناس ثبت‌شده", 2, "", 342 },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), "Activity", "پروژه فعال", 3, "", 894 },
                    { new Guid("e0000000-0000-0000-0000-000000000004"), "ShieldCheck", "بازرسی انجام‌شده", 4, "+", 2156 }
                });

            migrationBuilder.InsertData(
                table: "MeetingResolutions",
                columns: new[] { "Id", "MeetingId", "Status", "Text" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000100001"), new Guid("b0000000-0000-0000-0000-000000000010"), "در دست اقدام", "برگزاری ۴ دوره آموزشی در نیمه اول سال" },
                    { new Guid("c0000000-0000-0000-0000-000000110001"), new Guid("b0000000-0000-0000-0000-000000000011"), "در دست اقدام", "تشکیل کمیته ارزیابی سه‌نفره برای بررسی پرونده‌های تخلف" },
                    { new Guid("c0000000-0000-0000-0000-000000120001"), new Guid("b0000000-0000-0000-0000-000000000012"), "اجرا شده", "تصویب تعرفه‌های اجرای الکترود زمین برای اردیبهشت ۱۴۰۵" },
                    { new Guid("c0000000-0000-0000-0000-000000120002"), new Guid("b0000000-0000-0000-0000-000000000012"), "در دست اقدام", "الزام کارشناسان به استفاده از تعرفه جدید از ابتدای خرداد ۱۴۰۵" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_Slug",
                table: "Announcements",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetingResolutions_MeetingId",
                table: "MeetingResolutions",
                column: "MeetingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "MeetingResolutions");

            migrationBuilder.DropTable(
                name: "StatItems");

            migrationBuilder.DropTable(
                name: "Meetings");
        }
    }
}
