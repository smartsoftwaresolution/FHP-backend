using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class JobSkillDetailcollectionfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JobSkillDetail_JobId",
                table: "JobSkillDetail",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkillDetail_JobPosting_JobId",
                table: "JobSkillDetail",
                column: "JobId",
                principalTable: "JobPosting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSkillDetail_JobPosting_JobId",
                table: "JobSkillDetail");

            migrationBuilder.DropIndex(
                name: "IX_JobSkillDetail_JobId",
                table: "JobSkillDetail");
        }
    }
}
