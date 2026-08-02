using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coreapi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddContactMessageIsRead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only the ContactMessages.IsRead column is added here. The spurious
            // Clients.ApiKey default-value alter (a pre-existing random-default drift)
            // was intentionally removed to keep this migration surgical.
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ContactMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ContactMessages");
        }
    }
}
