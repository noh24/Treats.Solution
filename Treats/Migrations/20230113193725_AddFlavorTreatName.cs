using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registrar.Migrations
{
    public partial class AddFlavorTreatName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FlavorTreats",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FlavorTreats");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
