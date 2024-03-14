using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class adminjobtitleinEmployeeAvailability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminJobDescription",
                table: "EmployeeAvailability",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminJobTitle",
                table: "EmployeeAvailability",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminJobDescription",
                table: "EmployeeAvailability");

            migrationBuilder.DropColumn(
                name: "AdminJobTitle",
                table: "EmployeeAvailability");
        }
    }
}
