using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_Netficks.Migrations
{
    /// <inheritdoc />
    public partial class WEWILLMAYBEKILLDORIN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vizionare_ID",
                table: "IstoricVizionari",
                newName: "Istoric_ID");

            migrationBuilder.AddColumn<string>(
                name: "ImagineUrl",
                table: "Seriale",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerUrl",
                table: "Seriale",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Serial_ID",
                table: "Recenzii",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagineUrl",
                table: "Filme",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerUrl",
                table: "Filme",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descriere",
                table: "Episoade",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Episoade",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recenzii_Serial_ID",
                table: "Recenzii",
                column: "Serial_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Recenzii_Seriale_Serial_ID",
                table: "Recenzii",
                column: "Serial_ID",
                principalTable: "Seriale",
                principalColumn: "Serial_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recenzii_Seriale_Serial_ID",
                table: "Recenzii");

            migrationBuilder.DropIndex(
                name: "IX_Recenzii_Serial_ID",
                table: "Recenzii");

            migrationBuilder.DropColumn(
                name: "ImagineUrl",
                table: "Seriale");

            migrationBuilder.DropColumn(
                name: "TrailerUrl",
                table: "Seriale");

            migrationBuilder.DropColumn(
                name: "Serial_ID",
                table: "Recenzii");

            migrationBuilder.DropColumn(
                name: "ImagineUrl",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "TrailerUrl",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Descriere",
                table: "Episoade");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Episoade");

            migrationBuilder.RenameColumn(
                name: "Istoric_ID",
                table: "IstoricVizionari",
                newName: "Vizionare_ID");
        }
    }
}
