using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGOATS.api.Infra.Migrations
{
    /// <inheritdoc />
    public partial class footer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorFuentePrimaria",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorFuenteSecundaria",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorPrimario",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorSecundario",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnlaceContactenos",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnlaceFacebook",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnlaceInstagram",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnlaceTwitter",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnlaceYouTube",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoOficial",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipografia",
                table: "FooterInformacionInstitucional",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorFuentePrimaria",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "ColorFuenteSecundaria",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "ColorPrimario",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "ColorSecundario",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "EnlaceContactenos",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "EnlaceFacebook",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "EnlaceInstagram",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "EnlaceTwitter",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "EnlaceYouTube",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "LogoOficial",
                table: "FooterInformacionInstitucional");

            migrationBuilder.DropColumn(
                name: "Tipografia",
                table: "FooterInformacionInstitucional");
        }
    }
}
