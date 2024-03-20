using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class countrycitystatefkinemployeedetail2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDetail_CityId",
                table: "EmployeeDetail",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDetail_City_CityId",
                table: "EmployeeDetail",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDetail_City_CityId",
                table: "EmployeeDetail");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDetail_CityId",
                table: "EmployeeDetail");
        }
    }
}
