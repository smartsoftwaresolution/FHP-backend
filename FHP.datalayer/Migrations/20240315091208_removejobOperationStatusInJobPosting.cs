using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class removejobOperationStatusInJobPosting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobOperationStatus",
                table: "JobPosting");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobOperationStatus",
                table: "JobPosting",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
