using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGOATS.api.Infra.Migrations
{
    /// <inheritdoc />
    public partial class noticias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Detalle",
                table: "Noticias",
                newName: "UrlRecurso");

            migrationBuilder.AddColumn<string>(
                name: "Enlace",
                table: "Noticias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IdImagen",
                table: "Noticias",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "Noticias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Posicion",
                table: "Noticias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resumen",
                table: "Noticias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Target",
                table: "Noticias",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enlace",
                table: "Noticias");

            migrationBuilder.DropColumn(
                name: "IdImagen",
                table: "Noticias");

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "Noticias");

            migrationBuilder.DropColumn(
                name: "Posicion",
                table: "Noticias");

            migrationBuilder.DropColumn(
                name: "Resumen",
                table: "Noticias");

            migrationBuilder.DropColumn(
                name: "Target",
                table: "Noticias");

            migrationBuilder.RenameColumn(
                name: "UrlRecurso",
                table: "Noticias",
                newName: "Detalle");
        }
    }
}
