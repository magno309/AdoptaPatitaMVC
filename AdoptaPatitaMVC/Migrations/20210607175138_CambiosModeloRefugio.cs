using Microsoft.EntityFrameworkCore.Migrations;

namespace AdoptaPatitaMVC.Migrations
{
    public partial class CambiosModeloRefugio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenURL",
                table: "Refugios",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
