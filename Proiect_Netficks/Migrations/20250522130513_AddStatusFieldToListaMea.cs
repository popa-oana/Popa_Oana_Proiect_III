using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_Netficks.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusFieldToListaMea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiumOnly",
                table: "Seriale");

            migrationBuilder.DropColumn(
                name: "PremiumOnly",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "CardPlata",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ListaMea",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ListaMea");

            migrationBuilder.AddColumn<bool>(
                name: "PremiumOnly",
                table: "Seriale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PremiumOnly",
                table: "Filme",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CardPlata",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
