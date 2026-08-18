using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coreapi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSamtFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SamtIdentityCode",
                table: "ElectProjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SamtLicenseDate",
                table: "ElectProjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SamtLicenseExpireDate",
                table: "ElectProjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SamtLicenseNumber",
                table: "ElectProjects",
                type: "nvarchar(max)",
                nullable: true);

            // The AlterColumn on Clients.ApiKey that EF generates here is removed every
            // time: that column's default is regenerated on each build, so the drift is
            // never a real change. See docs 04-RECIPES B.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SamtIdentityCode",
                table: "ElectProjects");

            migrationBuilder.DropColumn(
                name: "SamtLicenseDate",
                table: "ElectProjects");

            migrationBuilder.DropColumn(
                name: "SamtLicenseExpireDate",
                table: "ElectProjects");

            migrationBuilder.DropColumn(
                name: "SamtLicenseNumber",
                table: "ElectProjects");

            // The AlterColumn on Clients.ApiKey that EF generates here is removed every
            // time: that column's default is regenerated on each build, so the drift is
            // never a real change. See docs 04-RECIPES B.
        }
    }
}
