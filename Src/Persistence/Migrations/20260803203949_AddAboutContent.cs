using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coreapi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The generated AlterColumn on Clients.ApiKey was removed: that column's default is
            // regenerated on every build, so EF emits the same spurious drift into every
            // migration. It is never a real change.

            migrationBuilder.CreateTable(
                name: "AboutContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Intro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MissionsTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgChartTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DutiesTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutBoardMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutBoardMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutBoardMembers_AboutContents_AboutContentId",
                        column: x => x.AboutContentId,
                        principalTable: "AboutContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AboutDuties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutDuties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutDuties_AboutContents_AboutContentId",
                        column: x => x.AboutContentId,
                        principalTable: "AboutContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AboutMissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IconName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutMissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutMissions_AboutContents_AboutContentId",
                        column: x => x.AboutContentId,
                        principalTable: "AboutContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AboutOrgNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutOrgNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutOrgNodes_AboutContents_AboutContentId",
                        column: x => x.AboutContentId,
                        principalTable: "AboutContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AboutContents",
                columns: new[] { "Id", "BoardTitle", "DutiesTitle", "Intro", "MissionsTitle", "OrgChartTitle", "PageTitle" },
                values: new object[] { new Guid("1a000000-0000-0000-0000-000000000001"), "هیئت رئیسه", "شرح وظایف", "دفتر اجرایی نظارت برق، یکی از واحدهای تخصصی سازمان نظام مهندسی ساختمان استان کردستان است که با هدف نظارت بر حسن اجرای پروژه‌های برق ساختمانی و ارتقاء ایمنی تأسیسات الکتریکی در استان فعالیت می‌نماید. این دفتر بر اساس نظام‌نامه مصوب مورخ ۱۳۹۳/۰۵/۰۲ هیئت مدیره سازمان تشکیل شده و فعالیت‌های خود را در چارچوب قوانین و مقررات ملی ساختمان انجام می‌دهد.", "اهداف و مأموریت", "ساختار سازمانی", "درباره دفتر اجرایی" });

            migrationBuilder.InsertData(
                table: "AboutBoardMembers",
                columns: new[] { "Id", "AboutContentId", "Description", "Name", "Role", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("1d000000-0000-0000-0000-000000000001"), new Guid("1a000000-0000-0000-0000-000000000001"), "منتخب هیئت مدیره سازمان", "مدیر اجرایی دفتر", "رئیس هیئت رئیسه", 1 },
                    { new Guid("1d000000-0000-0000-0000-000000000002"), new Guid("1a000000-0000-0000-0000-000000000001"), "نماینده انتصابی هیئت مدیره", "نماینده هیئت مدیره", "عضو هیئت رئیسه", 2 },
                    { new Guid("1d000000-0000-0000-0000-000000000003"), new Guid("1a000000-0000-0000-0000-000000000001"), "منتخب مجمع عمومی", "نماینده مجمع عمومی", "عضو هیئت رئیسه", 3 }
                });

            migrationBuilder.InsertData(
                table: "AboutDuties",
                columns: new[] { "Id", "AboutContentId", "SortOrder", "Text" },
                values: new object[,]
                {
                    { new Guid("1e000000-0000-0000-0000-000000000001"), new Guid("1a000000-0000-0000-0000-000000000001"), 1, "نظارت بر اجرای صحیح ضوابط و مقررات فنی مربوط به تأسیسات برقی" },
                    { new Guid("1e000000-0000-0000-0000-000000000002"), new Guid("1a000000-0000-0000-0000-000000000001"), 2, "بررسی و تأیید صلاحیت کارشناسان برق" },
                    { new Guid("1e000000-0000-0000-0000-000000000003"), new Guid("1a000000-0000-0000-0000-000000000001"), 3, "صدور پروانه اشتغال و کنترل کیفیت" },
                    { new Guid("1e000000-0000-0000-0000-000000000004"), new Guid("1a000000-0000-0000-0000-000000000001"), 4, "رسیدگی به شکایات و تخلفات" },
                    { new Guid("1e000000-0000-0000-0000-000000000005"), new Guid("1a000000-0000-0000-0000-000000000001"), 5, "همکاری با ادارات و سازمان‌های دولتی ذیربط" },
                    { new Guid("1e000000-0000-0000-0000-000000000006"), new Guid("1a000000-0000-0000-0000-000000000001"), 6, "برگزاری دوره‌های آموزشی و مهارت‌افزایی" },
                    { new Guid("1e000000-0000-0000-0000-000000000007"), new Guid("1a000000-0000-0000-0000-000000000001"), 7, "تهیه و به‌روزرسانی دستورالعمل‌های فنی" },
                    { new Guid("1e000000-0000-0000-0000-000000000008"), new Guid("1a000000-0000-0000-0000-000000000001"), 8, "تدوین تعرفه‌های خدمات مهندسی برق" }
                });

            migrationBuilder.InsertData(
                table: "AboutMissions",
                columns: new[] { "Id", "AboutContentId", "Description", "IconName", "SortOrder", "Title" },
                values: new object[,]
                {
                    { new Guid("1b000000-0000-0000-0000-000000000001"), new Guid("1a000000-0000-0000-0000-000000000001"), "نظارت بر حسن اجرای پروژه‌های برق ساختمانی مطابق با مقررات ملی و استانداردهای فنی", "Shield", 1, "نظارت فنی" },
                    { new Guid("1b000000-0000-0000-0000-000000000002"), new Guid("1a000000-0000-0000-0000-000000000001"), "ارتقاء کیفیت اجرای کارهای برق و ایمنی تأسیسات الکتریکی در استان کردستان", "Target", 2, "ارتقاء کیفیت" },
                    { new Guid("1b000000-0000-0000-0000-000000000003"), new Guid("1a000000-0000-0000-0000-000000000001"), "آموزش و توانمندسازی کارشناسان و ناظران حوزه برق ساختمان", "BookOpen", 3, "آموزش و توسعه" },
                    { new Guid("1b000000-0000-0000-0000-000000000004"), new Guid("1a000000-0000-0000-0000-000000000001"), "هماهنگی بین سازمان‌های مرتبط و یکپارچه‌سازی فرآیندهای اجرایی", "Users", 4, "هماهنگی سازمانی" }
                });

            migrationBuilder.InsertData(
                table: "AboutOrgNodes",
                columns: new[] { "Id", "AboutContentId", "Level", "SortOrder", "Title" },
                values: new object[,]
                {
                    { new Guid("1c000000-0000-0000-0000-000000000001"), new Guid("1a000000-0000-0000-0000-000000000001"), 0, 1, "هیئت مدیره سازمان نظام مهندسی" },
                    { new Guid("1c000000-0000-0000-0000-000000000002"), new Guid("1a000000-0000-0000-0000-000000000001"), 1, 1, "هیئت رئیسه دفتر اجرایی (۳ نفر)" },
                    { new Guid("1c000000-0000-0000-0000-000000000003"), new Guid("1a000000-0000-0000-0000-000000000001"), 2, 1, "مدیر اجرایی" },
                    { new Guid("1c000000-0000-0000-0000-000000000004"), new Guid("1a000000-0000-0000-0000-000000000001"), 2, 2, "نمایندگان شهرستان‌ها" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutBoardMembers_AboutContentId",
                table: "AboutBoardMembers",
                column: "AboutContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutDuties_AboutContentId",
                table: "AboutDuties",
                column: "AboutContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutMissions_AboutContentId",
                table: "AboutMissions",
                column: "AboutContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutOrgNodes_AboutContentId",
                table: "AboutOrgNodes",
                column: "AboutContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutBoardMembers");

            migrationBuilder.DropTable(
                name: "AboutDuties");

            migrationBuilder.DropTable(
                name: "AboutMissions");

            migrationBuilder.DropTable(
                name: "AboutOrgNodes");

            migrationBuilder.DropTable(
                name: "AboutContents");

            // The matching Clients.ApiKey AlterColumn was removed here too — see Up().
        }
    }
}
