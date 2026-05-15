using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coreapi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBaleIdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaleId",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaleId",
                table: "AspNetUsers");
        }
    }
}
