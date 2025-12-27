using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGOATS.api.Infra.Migrations
{
    /// <inheritdoc />
    public partial class enlacesInteres2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnlaceInteres");

            migrationBuilder.RenameColumn(
                name: "TipoApertura",
                table: "EnlacesInteres",
                newName: "Target");

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "EnlacesInteres",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<long>(
                name: "IdPagina",
                table: "EnlacesInteres",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPagina",
                table: "EnlacesInteres");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "EnlacesInteres",
                newName: "TipoApertura");

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "EnlacesInteres",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EnlaceInteres",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true),
                    IdPagina = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnlaceInteres", x => x.Id);
                });
        }
    }
}
