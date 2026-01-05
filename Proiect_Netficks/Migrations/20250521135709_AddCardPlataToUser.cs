using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_Netficks.Migrations
{
    /// <inheritdoc />
    public partial class AddCardPlataToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardPlata",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardPlata",
                table: "AspNetUsers");
        }
    }
}
