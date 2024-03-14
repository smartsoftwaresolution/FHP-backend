using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class updatedOnInEmployeeAvailability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "EmployeeAvailability",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "EmployeeAvailability");
        }
    }
}
