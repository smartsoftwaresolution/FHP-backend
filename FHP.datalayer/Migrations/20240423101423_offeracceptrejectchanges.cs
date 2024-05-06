using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FHP.datalayer.Migrations
{
    public partial class offeracceptrejectchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Offer");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "Offer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsAvaliable",
                table: "Offer",
                type: "int",
                nullable: false,
                defaultValue: 0);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "IsAvaliable",
                table: "Offer");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Offer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
