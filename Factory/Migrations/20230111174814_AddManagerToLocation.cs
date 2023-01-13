using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registrar.Migrations
{
    public partial class AddManagerToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Locations",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ManagerId",
                table: "Locations",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_AspNetUsers_ManagerId",
                table: "Locations",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_AspNetUsers_ManagerId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ManagerId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Locations");
        }
    }
}
