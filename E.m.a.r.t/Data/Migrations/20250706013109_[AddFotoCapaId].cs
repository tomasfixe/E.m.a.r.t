using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E.m.a.r.t.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFotoCapaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FotoCapaId",
                table: "Colecoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Colecoes_FotoCapaId",
                table: "Colecoes",
                column: "FotoCapaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colecoes_Fotografias_FotoCapaId",
                table: "Colecoes",
                column: "FotoCapaId",
                principalTable: "Fotografias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colecoes_Fotografias_FotoCapaId",
                table: "Colecoes");

            migrationBuilder.DropIndex(
                name: "IX_Colecoes_FotoCapaId",
                table: "Colecoes");

            migrationBuilder.DropColumn(
                name: "FotoCapaId",
                table: "Colecoes");
        }
    }
}
