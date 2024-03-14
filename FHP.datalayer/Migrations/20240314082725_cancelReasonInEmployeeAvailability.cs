using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class cancelReasonInEmployeeAvailability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancelReasons",
                table: "EmployeeAvailability",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelReasons",
                table: "EmployeeAvailability");
        }
    }
}
