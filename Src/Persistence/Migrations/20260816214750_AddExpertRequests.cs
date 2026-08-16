using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coreapi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExpertRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The generated AlterColumn on Clients.ApiKey was removed: that column's default is
            // regenerated on every build, so EF emits the same spurious drift into every
            // migration. It is never a real change.

            migrationBuilder.CreateTable(
                name: "ExpertRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NaCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpertRequests_CreatedAt",
                table: "ExpertRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertRequests_IsRead",
                table: "ExpertRequests",
                column: "IsRead");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpertRequests");

            // The matching Clients.ApiKey AlterColumn was removed here too — see Up().
        }
    }
}
