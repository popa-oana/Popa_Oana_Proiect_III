using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_Netficks.Migrations
{
    /// <inheritdoc />
    public partial class Imcrying : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nume = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Data_Inregistrare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tip_Abonament = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genuri",
                columns: table => new
                {
                    Gen_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume_Gen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genuri", x => x.Gen_ID);
                });

            migrationBuilder.CreateTable(
                name: "Utilizator",
                columns: table => new
                {
                    Utilizator_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Parola = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Data_Inregistrare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tip_Abonament = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizator", x => x.Utilizator_ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Filme",
                columns: table => new
                {
                    Film_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gen_ID = table.Column<int>(type: "int", nullable: false),
                    Titlu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    An_Lansare = table.Column<int>(type: "int", nullable: false),
                    Durata = table.Column<int>(type: "int", nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filme", x => x.Film_ID);
                    table.ForeignKey(
                        name: "FK_Filme_Genuri_Gen_ID",
                        column: x => x.Gen_ID,
                        principalTable: "Genuri",
                        principalColumn: "Gen_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seriale",
                columns: table => new
                {
                    Serial_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gen_ID = table.Column<int>(type: "int", nullable: false),
                    Titlu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    An_Aparitie = table.Column<int>(type: "int", nullable: false),
                    Numar_Sezoane = table.Column<int>(type: "int", nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seriale", x => x.Serial_ID);
                    table.ForeignKey(
                        name: "FK_Seriale_Genuri_Gen_ID",
                        column: x => x.Gen_ID,
                        principalTable: "Genuri",
                        principalColumn: "Gen_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Abonamente",
                columns: table => new
                {
                    Abonament_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Utilizator_ID = table.Column<int>(type: "int", nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data_Sfarsit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonamente", x => x.Abonament_ID);
                    table.ForeignKey(
                        name: "FK_Abonamente_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Abonamente_Utilizator_Utilizator_ID",
                        column: x => x.Utilizator_ID,
                        principalTable: "Utilizator",
                        principalColumn: "Utilizator_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episoade",
                columns: table => new
                {
                    Episod_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Serial_ID = table.Column<int>(type: "int", nullable: false),
                    Titlu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Numar_Sezon = table.Column<int>(type: "int", nullable: false),
                    Numar_Episod = table.Column<int>(type: "int", nullable: false),
                    Durata = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episoade", x => x.Episod_ID);
                    table.ForeignKey(
                        name: "FK_Episoade_Seriale_Serial_ID",
                        column: x => x.Serial_ID,
                        principalTable: "Seriale",
                        principalColumn: "Serial_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListaMea",
                columns: table => new
                {
                    Lista_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Utilizator_ID = table.Column<int>(type: "int", nullable: false),
                    Film_ID = table.Column<int>(type: "int", nullable: true),
                    Serial_ID = table.Column<int>(type: "int", nullable: true),
                    Data_Creare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaMea", x => x.Lista_ID);
                    table.ForeignKey(
                        name: "FK_ListaMea_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaMea_Filme_Film_ID",
                        column: x => x.Film_ID,
                        principalTable: "Filme",
                        principalColumn: "Film_ID");
                    table.ForeignKey(
                        name: "FK_ListaMea_Seriale_Serial_ID",
                        column: x => x.Serial_ID,
                        principalTable: "Seriale",
                        principalColumn: "Serial_ID");
                    table.ForeignKey(
                        name: "FK_ListaMea_Utilizator_Utilizator_ID",
                        column: x => x.Utilizator_ID,
                        principalTable: "Utilizator",
                        principalColumn: "Utilizator_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IstoricVizionari",
                columns: table => new
                {
                    Vizionare_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Utilizator_ID = table.Column<int>(type: "int", nullable: false),
                    Film_ID = table.Column<int>(type: "int", nullable: true),
                    Episod_ID = table.Column<int>(type: "int", nullable: true),
                    Timp_Vizionare = table.Column<int>(type: "int", nullable: false),
                    Data_Vizionare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IstoricVizionari", x => x.Vizionare_ID);
                    table.ForeignKey(
                        name: "FK_IstoricVizionari_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IstoricVizionari_Episoade_Episod_ID",
                        column: x => x.Episod_ID,
                        principalTable: "Episoade",
                        principalColumn: "Episod_ID");
                    table.ForeignKey(
                        name: "FK_IstoricVizionari_Filme_Film_ID",
                        column: x => x.Film_ID,
                        principalTable: "Filme",
                        principalColumn: "Film_ID");
                    table.ForeignKey(
                        name: "FK_IstoricVizionari_Utilizator_Utilizator_ID",
                        column: x => x.Utilizator_ID,
                        principalTable: "Utilizator",
                        principalColumn: "Utilizator_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzii",
                columns: table => new
                {
                    Recenzie_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Utilizator_ID = table.Column<int>(type: "int", nullable: false),
                    Film_ID = table.Column<int>(type: "int", nullable: true),
                    Episod_ID = table.Column<int>(type: "int", nullable: true),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentariu = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Data_Postarii = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzii", x => x.Recenzie_ID);
                    table.ForeignKey(
                        name: "FK_Recenzii_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Recenzii_Episoade_Episod_ID",
                        column: x => x.Episod_ID,
                        principalTable: "Episoade",
                        principalColumn: "Episod_ID");
                    table.ForeignKey(
                        name: "FK_Recenzii_Filme_Film_ID",
                        column: x => x.Film_ID,
                        principalTable: "Filme",
                        principalColumn: "Film_ID");
                    table.ForeignKey(
                        name: "FK_Recenzii_Utilizator_Utilizator_ID",
                        column: x => x.Utilizator_ID,
                        principalTable: "Utilizator",
                        principalColumn: "Utilizator_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abonamente_UserId",
                table: "Abonamente",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Abonamente_Utilizator_ID",
                table: "Abonamente",
                column: "Utilizator_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Episoade_Serial_ID",
                table: "Episoade",
                column: "Serial_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Filme_Gen_ID",
                table: "Filme",
                column: "Gen_ID");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricVizionari_Episod_ID",
                table: "IstoricVizionari",
                column: "Episod_ID");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricVizionari_Film_ID",
                table: "IstoricVizionari",
                column: "Film_ID");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricVizionari_UserId",
                table: "IstoricVizionari",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricVizionari_Utilizator_ID",
                table: "IstoricVizionari",
                column: "Utilizator_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ListaMea_Film_ID",
                table: "ListaMea",
                column: "Film_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ListaMea_Serial_ID",
                table: "ListaMea",
                column: "Serial_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ListaMea_UserId",
                table: "ListaMea",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaMea_Utilizator_ID",
                table: "ListaMea",
                column: "Utilizator_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzii_Episod_ID",
                table: "Recenzii",
                column: "Episod_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzii_Film_ID",
                table: "Recenzii",
                column: "Film_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzii_UserId",
                table: "Recenzii",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzii_Utilizator_ID",
                table: "Recenzii",
                column: "Utilizator_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Seriale_Gen_ID",
                table: "Seriale",
                column: "Gen_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonamente");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "IstoricVizionari");

            migrationBuilder.DropTable(
                name: "ListaMea");

            migrationBuilder.DropTable(
                name: "Recenzii");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Episoade");

            migrationBuilder.DropTable(
                name: "Filme");

            migrationBuilder.DropTable(
                name: "Utilizator");

            migrationBuilder.DropTable(
                name: "Seriale");

            migrationBuilder.DropTable(
                name: "Genuri");
        }
    }
}
