using Microsoft.EntityFrameworkCore.Migrations;

namespace AdoptaPatitaMVC.Migrations
{
    public partial class UpdateSolicitud2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolicitudRefugios",
                columns: table => new
                {
                    SolicitudRefugioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefugioId = table.Column<int>(type: "int", nullable: false),
                    EsAceptado = table.Column<bool>(type: "bit", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    returnUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudRefugios", x => x.SolicitudRefugioId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudRefugios");
        }
    }
}
