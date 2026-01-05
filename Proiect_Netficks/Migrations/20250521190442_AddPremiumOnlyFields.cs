using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_Netficks.Migrations
{
    /// <inheritdoc />
    public partial class AddPremiumOnlyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PremiumOnly",
                table: "Seriale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Serial_ID",
                table: "IstoricVizionari",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PremiumOnly",
                table: "Filme",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricVizionari_Serial_ID",
                table: "IstoricVizionari",
                column: "Serial_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_IstoricVizionari_Seriale_Serial_ID",
                table: "IstoricVizionari",
                column: "Serial_ID",
                principalTable: "Seriale",
                principalColumn: "Serial_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IstoricVizionari_Seriale_Serial_ID",
                table: "IstoricVizionari");

            migrationBuilder.DropIndex(
                name: "IX_IstoricVizionari_Serial_ID",
                table: "IstoricVizionari");

            migrationBuilder.DropColumn(
                name: "PremiumOnly",
                table: "Seriale");

            migrationBuilder.DropColumn(
                name: "Serial_ID",
                table: "IstoricVizionari");

            migrationBuilder.DropColumn(
                name: "PremiumOnly",
                table: "Filme");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);
        }
    }
}
