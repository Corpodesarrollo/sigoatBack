using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGOATS.api.Infra.Migrations
{
    /// <inheritdoc />
    public partial class footer3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoOficial",
                table: "FooterInformacionInstitucional");

            migrationBuilder.AddColumn<long>(
                name: "IdLogoOficial",
                table: "FooterInformacionInstitucional",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdLogoOficial",
                table: "FooterInformacionInstitucional");

            migrationBuilder.AddColumn<string>(
                name: "LogoOficial",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
