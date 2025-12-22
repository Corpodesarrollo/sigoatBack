using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGOATS.api.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoEvento",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IdTablero",
                table: "NoticiasDetalles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RedesSociales = table.Column<bool>(type: "bit", nullable: false),
                    ColorGovCo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdLogoIzquierdo = table.Column<long>(type: "bigint", nullable: true),
                    IdLogoDerecho = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracion");

            migrationBuilder.DropColumn(
                name: "TipoEvento",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "IdTablero",
                table: "NoticiasDetalles");
        }
    }
}
