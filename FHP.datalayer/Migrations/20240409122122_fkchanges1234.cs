using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class fkchanges1234 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JobPosting_UserId",
                table: "JobPosting",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosting_User_UserId",
                table: "JobPosting",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPosting_User_UserId",
                table: "JobPosting");

            migrationBuilder.DropIndex(
                name: "IX_JobPosting_UserId",
                table: "JobPosting");
        }
    }
}
