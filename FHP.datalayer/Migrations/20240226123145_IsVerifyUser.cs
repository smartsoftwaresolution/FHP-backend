using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class IsVerifyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerify",
                table: "User",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerify",
                table: "User");
        }
    }
}
