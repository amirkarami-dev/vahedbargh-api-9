using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coreapi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessFlows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Spurious Clients.ApiKey default-drift removed (kept surgical), same as
            // the earlier landing migrations.
            migrationBuilder.CreateTable(
                name: "ProcessFlows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GlowColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessFlows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredDocs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tools = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDecision = table.Column<bool>(type: "bit", nullable: false),
                    DecisionYes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessSteps_ProcessFlows_ProcessFlowId",
                        column: x => x.ProcessFlowId,
                        principalTable: "ProcessFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProcessFlows",
                columns: new[] { "Id", "Color", "Description", "GlowColor", "Icon", "Key", "SortOrder", "Subtitle", "Title" },
                values: new object[,]
                {
                    { new Guid("f0000000-0000-0000-0000-000000000001"), "from-blue-500 to-indigo-600", "فرآیند رسمی بازرسی تأسیسات الکتریکی ساختمان مطابق با مبحث ۱۳ مقررات ملی ساختمان، استاندارد IEC 60364-6 و BS 7671", "rgba(99,102,241,0.25)", "🔍", "inspection", 1, "راهنمای فرآیند بازرسی تأسیسات الکتریکی", "بازرسی برق" },
                    { new Guid("f0000000-0000-0000-0000-000000000002"), "from-emerald-500 to-teal-600", "فرآیند طراحی، اجرا، اندازه‌گیری و مستندسازی سیستم الکترود زمین ساختمان مطابق با مقررات ملی ساختمان", "rgba(16,185,129,0.25)", "⚡", "earth", 2, "راهنمای اجرا و مستندسازی سیستم زمین", "سیستم ارت" },
                    { new Guid("f0000000-0000-0000-0000-000000000003"), "from-amber-500 to-orange-600", "فرآیند رسمی تست بدو تحویل تأسیسات برق ساختمان مطابق ماده ۱۳-۳-۵ مبحث ۱۳ مقررات ملی ساختمان و استاندارد IEC 60364-6", "rgba(245,158,11,0.25)", "🧪", "test-delivery", 3, "فرآیند تست، بازرسی و تحویل تأسیسات الکتریکی", "تست و تحویل" }
                });

            migrationBuilder.InsertData(
                table: "ProcessSteps",
                columns: new[] { "Id", "DecisionNo", "DecisionYes", "Description", "Details", "IsDecision", "Note", "Number", "ProcessFlowId", "RequiredDocs", "SortOrder", "Title", "Tools" },
                values: new object[,]
                {
                    { new Guid("f1000000-0000-0000-0000-000000000001"), null, null, "ناظر تأسیسات الکتریکی درخواست بازرسی را تکمیل و به دفتر اجرایی ارائه می‌دهد.", null, false, null, 1, new Guid("f0000000-0000-0000-0000-000000000001"), "فرم درخواست بازرسی\nکپی پروانه ساخت\nنقشه‌های تأسیسات الکتریکی", 1, "دریافت درخواست بازرسی", null },
                    { new Guid("f1000000-0000-0000-0000-000000000002"), null, null, "مدارک ناظر و مجری بررسی می‌شود. گواهینامه فنی بازرسی و پروانه اشتغال به کار معتبر باشد.", "اعتبار گواهینامه فنی بازرسی تأیید شود\nصلاحیت بازرس در سامانه نظام مهندسی احراز شود", false, null, 2, new Guid("f0000000-0000-0000-0000-000000000001"), "گواهینامه فنی بازرس\nپروانه اشتغال ناظر\nمستندات مجری برق", 2, "بررسی مدارک و صلاحیت", null },
                    { new Guid("f1000000-0000-0000-0000-000000000003"), null, null, "تاریخ و زمان بازدید میدانی با هماهنگی ناظر، مجری و مالک تعیین می‌شود.", "ارسال اطلاعیه زمان بازدید به ناظر هماهنگ‌کننده\nاطمینان از حضور مجری اجرا در محل", false, null, 3, new Guid("f0000000-0000-0000-0000-000000000001"), null, 3, "هماهنگی و زمان‌بندی بازدید", null },
                    { new Guid("f1000000-0000-0000-0000-000000000004"), null, null, "بازرس به محل ساختمان مراجعه و وضعیت اجرایی تأسیسات را از نظر انطباق با مقررات ملی ارزیابی می‌کند.", "بررسی کابل‌کشی و لوله‌گذاری\nبررسی تابلوهای توزیع و کنتور\nبررسی سیستم زمین و همبندی\nبررسی تجهیزات حفاظتی", false, null, 4, new Guid("f0000000-0000-0000-0000-000000000001"), null, 4, "بازدید میدانی از محل", "متر و ابزار اندازه‌گیری\nچراغ‌قوه بازرسی\nدوربین ثبت مستندات" },
                    { new Guid("f1000000-0000-0000-0000-000000000005"), null, null, "چک‌لیست استاندارد بازرسی ویرایش ۱۴۰۴ مطابق IEC 60364-6، BS 7671 و استاندارد ملی ایران ۶-۱۹۳۷ تکمیل می‌شود.", "آرایش سیستم زمین: TN-C-S / TT / IT\nمشخصات انشعاب (جریان نامی، ماکزیمم دیماند)\nمشخصات کلید قطع‌کننده اصلی\nپیوستگی هادی‌های حفاظتی و همبندی", false, null, 5, new Guid("f0000000-0000-0000-0000-000000000001"), "چک‌لیست بازرسی ویرایش ۱۴۰۴\nفرم مشخصات منبع تغذیه\nجداول آرایش سیستم زمین", 5, "تکمیل چک‌لیست بازرسی", null },
                    { new Guid("f1000000-0000-0000-0000-000000000006"), null, null, "کروکی محل به صورت دستی یا با استفاده از قالب DWG تهیه و ضمیمه گزارش می‌شود.", "موقعیت تابلوها، کنتورها و کلیدها مشخص شود\nمسیر کابل‌های اصلی ترسیم شود", false, null, 6, new Guid("f0000000-0000-0000-0000-000000000001"), "قالب کروکی بازرسی (.dwg)\nنمونه کروکی مرجع", 6, "رسم کروکی بازرسی", null },
                    { new Guid("f1000000-0000-0000-0000-000000000007"), null, null, "گزارش کامل بازرسی شامل نتایج بازدید، موارد عدم انطباق و پیشنهادات تنظیم می‌شود.", "موارد متضاد با مبحث ۱۳ مقررات ملی مستند شود\nپیشنهاد دوره بازرسی بعدی (حداکثر ... سال)\nشماره گواهینامه فنی بازرس درج شود", false, null, 7, new Guid("f0000000-0000-0000-0000-000000000001"), null, 7, "تنظیم و صدور گزارش بازرسی", null },
                    { new Guid("f1000000-0000-0000-0000-000000000008"), null, null, "پس از رفع نقص‌ها، ناظر تأسیسات فرم تأیید را مهر و امضاء کرده و مدارک به شرکت توزیع برق ارسال می‌شود.", null, false, "صدور گواهی پایان کار منوط به تأیید این مرحله است.", 8, new Guid("f0000000-0000-0000-0000-000000000001"), "فرم تأیید ناظر (مهر و امضاء)\nگزارش بازرسی نهایی\nکروکی تأیید شده", 8, "تأیید ناظر و ارسال به برق", null },
                    { new Guid("f2000000-0000-0000-0000-000000000001"), null, null, "نقشه‌های اجرایی سیستم زمین بررسی می‌شود. نوع آرایش سیستم زمین (TN-C-S یا TT) مشخص می‌گردد.", "تعیین نوع سیستم: TN-C-S یا TT\nبررسی محل نصب الکترود زمین\nمحاسبه مقاومت زمین مورد نیاز", false, null, 1, new Guid("f0000000-0000-0000-0000-000000000002"), "نقشه سیستم زمین (.dwg)\nمدل‌های TNCS و TT", 1, "بررسی نقشه‌های سیستم ارت", null },
                    { new Guid("f2000000-0000-0000-0000-000000000002"), null, null, "بر اساس نوع خاک، عمق زمین آب‌دار و مساحت، روش اجرای الکترود زمین انتخاب می‌شود.", "الکترود میله‌ای (Rod): خاک‌های عمیق\nالکترود تسمه‌ای (Strap): خاک‌های سطحی\nالکترود صفحه‌ای (Plate): فضای محدود\nالکترود پایه ساختمان (Foundation): بتون‌آرمه", false, null, 2, new Guid("f0000000-0000-0000-0000-000000000002"), null, 2, "تعیین روش اجرای الکترود", "آزمون مقاومت ویژه خاک (Wenner Method)" },
                    { new Guid("f2000000-0000-0000-0000-000000000003"), null, null, "الکترود زمین با مشخصات فنی مندرج در مبحث ۱۳ مقررات ملی ساختمان نصب می‌شود.", "عمق حداقل اجرا رعایت شود\nاتصالات با کلمپ مستقیم یا جوش احتراقی (Exothermic)\nهادی اتصال زمین: مس یا آلومینیوم با سطح مقطع استاندارد\nحفاظت از خوردگی الکترود", false, null, 3, new Guid("f0000000-0000-0000-0000-000000000002"), null, 3, "اجرای الکترود زمین", "گیره کلمپ ارت\nجوش احتراقی (Cadweld)\nالکترود مسی استاندارد" },
                    { new Guid("f2000000-0000-0000-0000-000000000004"), "مقاومت > ۱۰ اهم → اصلاح", "مقاومت ≤ ۱۰ اهم → قبول", "مقاومت الکترود زمین با دستگاه اندازه‌گیر استاندارد (Earth Tester) سنجیده می‌شود.", "روش سه‌نقطه (Three Point Method)\nفاصله‌گذاری استاندارد میخ‌ها", true, null, 4, new Guid("f0000000-0000-0000-0000-000000000002"), null, 4, "اندازه‌گیری مقاومت زمین", "KYORITSU 4105A\nFLUKE 1653B\nمیخ‌های کمکی اندازه‌گیری" },
                    { new Guid("f2000000-0000-0000-0000-000000000005"), null, null, "در صورتی که مقاومت زمین بیش از ۱۰ اهم باشد، اقدامات اصلاحی انجام می‌شود.", "افزودن الکترود موازی\nاستفاده از مواد بهبوددهنده (Backfill Compound)\nتعویض الکترود با سطح مقطع بزرگتر\nتکرار اندازه‌گیری پس از اصلاح", false, "تا رسیدن به مقاومت ≤ ۱۰Ω مرحله ۴ تکرار می‌شود.", 5, new Guid("f0000000-0000-0000-0000-000000000002"), null, 5, "اصلاح در صورت عدم قبولی", null },
                    { new Guid("f2000000-0000-0000-0000-000000000006"), null, null, "شناسنامه ارت با کلیه مشخصات الکترود، هادی‌ها، مقاومت اندازه‌گیری شده و امضای مجری تکمیل می‌شود.", "نوع الکترود و مشخصات ثبت شود\nمقدار مقاومت زمین (Ω) درج شود\nتاریخ اندازه‌گیری ثبت شود", false, null, 6, new Guid("f0000000-0000-0000-0000-000000000002"), "شناسنامه ارت (فرم ۱ و ۲)\nنتایج اندازه‌گیری مقاومت", 6, "تکمیل شناسنامه ارت", null },
                    { new Guid("f2000000-0000-0000-0000-000000000007"), null, null, "مستندات کامل مجری ارت شامل شناسنامه‌های تأیید شده، گزارش اندازه‌گیری و نقشه اجرایی تحویل می‌شود.", null, false, "این مستندات برای مراحل تست و تحویل الزامی است.", 7, new Guid("f0000000-0000-0000-0000-000000000002"), "مستندات مجری ارت (تأیید شده)\nشناسنامه ارت ممهور\nنقشه as-built سیستم زمین", 7, "صدور مستندات مجری ارت", null },
                    { new Guid("f3000000-0000-0000-0000-000000000001"), null, null, "ناظر تأسیسات الکتریکی فرم معرفی مجری ذیصلاح تست و تحویل را به دفتر اجرایی ارائه می‌دهد.", "مطابق ماده ۱۳-۳-۵ مبحث ۱۳ مقررات ملی ساختمان\nهماهنگی با ناظر هماهنگ‌کننده پروژه\nاطلاع‌رسانی به مالک", false, null, 1, new Guid("f0000000-0000-0000-0000-000000000003"), "فرم درخواست معرفی مجری تست و تحویل", 1, "معرفی مجری ذیصلاح تست", null },
                    { new Guid("f3000000-0000-0000-0000-000000000002"), null, null, "مدارک کامل طراح، مجری و ناظر تأسیسات الکتریکی دریافت و بررسی می‌شود.", null, false, null, 2, new Guid("f0000000-0000-0000-0000-000000000003"), "اعلامیه طراح تأسیسات (مهر و امضاء)\nاعلامیه مجری تأسیسات (مهر و امضاء)\nمستندات بازرس برق\nشناسنامه ارت تأیید شده", 2, "دریافت و بررسی مدارک پروژه", null },
                    { new Guid("f3000000-0000-0000-0000-000000000003"), null, null, "تجهیزات تست کالیبره شده آماده و مشخصات تابلوهای توزیع بررسی می‌شود.", "بررسی تاریخ کالیبراسیون دستگاه‌ها\nمطالعه نقشه‌های تابلوی توزیع\nتهیه تست شیت خالی", false, null, 3, new Guid("f0000000-0000-0000-0000-000000000003"), null, 3, "آماده‌سازی تجهیزات تست", "FLUKE 1653B (مولتی‌فانکشن تست)\nKYORITSU 4105A (ارت‌تستر)\nکلمپ آمپرمتر\nولت‌متر دیجیتال" },
                    { new Guid("f3000000-0000-0000-0000-000000000004"), null, null, "صحت توالی فازها و درستی قطبیت اتصالات در تمامی مدارها بررسی می‌شود.", "توالی فاز R-S-T: راستگرد (مثبت)\nقطبیت هادی‌های L و N در تمامی پریزها\nقطبیت کلیدها (فاز باید قطع شود)", false, null, 4, new Guid("f0000000-0000-0000-0000-000000000003"), null, 4, "تست توالی فاز و قطبیت", "تستر توالی فاز\nFLUKE 1653B" },
                    { new Guid("f3000000-0000-0000-0000-000000000005"), "< ۱ MΩ → اصلاح عایق‌بندی", "≥ ۱ MΩ → قبول", "مقاومت عایق هادی‌های مدارها با ولتاژ آزمون DC اندازه‌گیری می‌شود.", "ولتاژ آزمون: ۵۰۰V DC برای مدارهای ۲۳۰/۴۰۰V\nحداقل مقاومت: ≥ ۱ مگا‌اهم\nتست در وضعیت بدون بار\nجداسازی تجهیزات حساس (SPD، UPS)", true, null, 5, new Guid("f0000000-0000-0000-0000-000000000003"), "جدول نتایج مقاومت عایقی", 5, "تست مقاومت عایقی", "FLUKE 1653B (Insulation Mode)" },
                    { new Guid("f3000000-0000-0000-0000-000000000006"), null, null, "امپدانس حلقه (Zs) برای تمامی مدارها اندازه‌گیری و با مقادیر مجاز مقایسه می‌شود.", "اندازه‌گیری Zs در انتهای هر مدار\nبررسی کفایت جریان خطا برای عملکرد MCB\nمحاسبه IPF (Prospective Fault Current)", false, null, 6, new Guid("f0000000-0000-0000-0000-000000000003"), "جدول مشخصات مدارهای تابلو توزیع", 6, "تست امپدانس حلقه اتصال کوتاه", "FLUKE 1653B (Loop Impedance Mode)" },
                    { new Guid("f3000000-0000-0000-0000-000000000007"), null, null, "عملکرد و زمان قطع تمامی کلیدهای جریان باقیمانده (RCCB/RCBO) آزمون می‌شود.", "جریان آزمون: ۱×IΔn (باید < ۳۰۰ms قطع شود)\nجریان آزمون: ۵×IΔn (باید < ۴۰ms قطع شود)\nتست در هر دو نیم‌سیکل مثبت و منفی\nتست دکمه Test روی هر RCD", false, null, 7, new Guid("f0000000-0000-0000-0000-000000000003"), "جدول نتایج تست RCD", 7, "تست کلید جریان باقیمانده (RCD)", "FLUKE 1653B (RCD Test Mode)" },
                    { new Guid("f3000000-0000-0000-0000-000000000008"), null, null, "مقاومت الکترود زمین اندازه‌گیری و صحت پیوستگی هادی‌های حفاظتی بررسی می‌شود.", "مقاومت الکترود زمین: ≤ ۱۰ اهم\nپیوستگی هادی‌های PE در تمامی مدارها\nاتصال همبندی اصلی (MEB)\nاتصال همبندی لوله‌های فلزی", false, null, 8, new Guid("f0000000-0000-0000-0000-000000000003"), null, 8, "تست سیستم ارت و همبندی", "KYORITSU 4105A\nFLUKE 1653B (Continuity Mode)" },
                    { new Guid("f3000000-0000-0000-0000-000000000009"), null, null, "تست شیت کامل با کلیه نتایج اندازه‌گیری، امضای طراح، مجری، ناظر و بازرس تکمیل می‌شود.", "کلیه نتایج تست‌ها ثبت شود\nموارد عدم انطباق مستند گردد\nپیشنهاد دوره بازرسی بعدی درج شود", false, null, 9, new Guid("f0000000-0000-0000-0000-000000000003"), "تست شیت تأسیسات برقی (مهر و امضاء طراح)\nتست شیت (مهر و امضاء مجری)\nتست شیت (مهر و امضاء ناظر)\nگزارش بازرسی تست و تحویل", 9, "تکمیل تست شیت و گزارش", null },
                    { new Guid("f3000000-0000-0000-0000-000000000010"), null, null, "مدارک کامل به کمیسیون تأسیسات دفتر اجرایی ارائه شده و گواهی پایان کار صادر می‌شود.", null, false, "صدور گواهی پایان کار برقی منوط به تکمیل موفق کلیه مراحل تست و تحویل است.", 10, new Guid("f0000000-0000-0000-0000-000000000003"), "پکیج کامل مستندات تست و تحویل\nروال اجرایی تحویل تأسیسات برقی\nفرم تأیید ناظر نهایی", 10, "ارائه به کمیسیون و صدور گواهی", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessFlows_Key",
                table: "ProcessFlows",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSteps_ProcessFlowId",
                table: "ProcessSteps",
                column: "ProcessFlowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessSteps");

            migrationBuilder.DropTable(
                name: "ProcessFlows");
        }
    }
}
