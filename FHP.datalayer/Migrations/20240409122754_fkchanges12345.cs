using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class fkchanges12345 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfessionalDetail_UserId",
                table: "EmployeeProfessionalDetail",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProfessionalDetail_User_UserId",
                table: "EmployeeProfessionalDetail",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProfessionalDetail_User_UserId",
                table: "EmployeeProfessionalDetail");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeProfessionalDetail_UserId",
                table: "EmployeeProfessionalDetail");
        }
    }
}
