using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E.m.a.r.t.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColecaoFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Fotografias",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "ColecaoFK",
                table: "Fotografias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Colecoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colecoes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotografias_ColecaoFK",
                table: "Fotografias",
                column: "ColecaoFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografias_Colecoes_ColecaoFK",
                table: "Fotografias",
                column: "ColecaoFK",
                principalTable: "Colecoes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografias_Colecoes_ColecaoFK",
                table: "Fotografias");

            migrationBuilder.DropTable(
                name: "Colecoes");

            migrationBuilder.DropIndex(
                name: "IX_Fotografias_ColecaoFK",
                table: "Fotografias");

            migrationBuilder.DropColumn(
                name: "ColecaoFK",
                table: "Fotografias");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Fotografias",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }
    }
}
