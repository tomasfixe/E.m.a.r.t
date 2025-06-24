using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E.m.a.r.t.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTabelaUtilizadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComprasId",
                table: "Fotografias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtilizadoresId",
                table: "Fotografias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodPostal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NIF = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    CompradorFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compras_Utilizadores_CompradorFK",
                        column: x => x.CompradorFK,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotografias_ComprasId",
                table: "Fotografias",
                column: "ComprasId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotografias_UtilizadoresId",
                table: "Fotografias",
                column: "UtilizadoresId");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CompradorFK",
                table: "Compras",
                column: "CompradorFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografias_Compras_ComprasId",
                table: "Fotografias",
                column: "ComprasId",
                principalTable: "Compras",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografias_Utilizadores_UtilizadoresId",
                table: "Fotografias",
                column: "UtilizadoresId",
                principalTable: "Utilizadores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografias_Compras_ComprasId",
                table: "Fotografias");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotografias_Utilizadores_UtilizadoresId",
                table: "Fotografias");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Fotografias_ComprasId",
                table: "Fotografias");

            migrationBuilder.DropIndex(
                name: "IX_Fotografias_UtilizadoresId",
                table: "Fotografias");

            migrationBuilder.DropColumn(
                name: "ComprasId",
                table: "Fotografias");

            migrationBuilder.DropColumn(
                name: "UtilizadoresId",
                table: "Fotografias");
        }
    }
}
