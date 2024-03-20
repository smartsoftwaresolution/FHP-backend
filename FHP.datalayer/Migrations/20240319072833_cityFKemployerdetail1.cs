using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class cityFKemployerdetail1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmployerDetail_CityId",
                table: "EmployerDetail",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerDetail_City_CityId",
                table: "EmployerDetail",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerDetail_City_CityId",
                table: "EmployerDetail");

            migrationBuilder.DropIndex(
                name: "IX_EmployerDetail_CityId",
                table: "EmployerDetail");
        }
    }
}
