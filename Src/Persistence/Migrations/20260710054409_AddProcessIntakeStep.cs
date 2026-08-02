using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coreapi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessIntakeStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Spurious Clients.ApiKey default-drift removed (kept surgical), same as
            // the earlier landing migrations.
            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000001"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000002"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000003"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 4, 4 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000004"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 5, 5 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000005"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 6, 6 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000006"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 7, 7 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000007"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 8, 8 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000008"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 9, 9 });

            migrationBuilder.InsertData(
                table: "ProcessSteps",
                columns: new[] { "Id", "DecisionNo", "DecisionYes", "Description", "Details", "IsDecision", "Note", "Number", "ProcessFlowId", "RequiredDocs", "SortOrder", "Title", "Tools" },
                values: new object[] { new Guid("f1000000-0000-0000-0000-000000000000"), null, null, "برای بازرسی برق یا سیستم ارت، ابتدا مالک به شرکت توزیع مراجعه می‌کند. شرکت توزیع برای درخواست مالک یک پرونده در کارتابل دفتر اجرایی ایجاد می‌کند و ادامهٔ فرآیند در دفتر اجرایی انجام می‌شود.", "مراجعهٔ مالک به شرکت توزیع برق\nثبت درخواست و تشکیل پرونده در کارتابل دفتر اجرایی\nارجاع پرونده به دفتر اجرایی برای ادامهٔ فرآیند", false, null, 1, new Guid("f0000000-0000-0000-0000-000000000001"), null, 1, "مراجعهٔ مالک و تشکیل پرونده", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000001"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000002"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000003"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000004"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 4, 4 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000005"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 5, 5 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000006"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 6, 6 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000007"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 7, 7 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "Id",
                keyValue: new Guid("f1000000-0000-0000-0000-000000000008"),
                columns: new[] { "Number", "SortOrder" },
                values: new object[] { 8, 8 });
        }
    }
}
