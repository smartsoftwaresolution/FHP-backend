using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class adminjobtitleinjobPostng : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminJobDescription",
                table: "JobPosting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminJobTitle",
                table: "JobPosting",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminJobDescription",
                table: "JobPosting");

            migrationBuilder.DropColumn(
                name: "AdminJobTitle",
                table: "JobPosting");
        }
    }
}
