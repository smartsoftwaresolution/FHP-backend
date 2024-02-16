using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class screenupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SourceName",
                table: "Screen",
                newName: "ScreenName");

            migrationBuilder.RenameColumn(
                name: "SourceCode",
                table: "Screen",
                newName: "ScreenCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScreenName",
                table: "Screen",
                newName: "SourceName");

            migrationBuilder.RenameColumn(
                name: "ScreenCode",
                table: "Screen",
                newName: "SourceCode");
        }
    }
}
