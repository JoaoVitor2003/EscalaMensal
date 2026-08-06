using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCeroferario2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM CargoNivelFuncaoPermitidas WHERE FuncaoId = 7;");
            migrationBuilder.Sql("UPDATE ItensMissa SET FuncaoId = 6 WHERE FuncaoId = 7;");
            migrationBuilder.Sql("UPDATE HistoricosItemMissa SET FuncaoId = 6, FuncaoNome = 'Ceroferário', FuncaoAbreviacao = 'Ce' WHERE FuncaoId = 7;");
            migrationBuilder.Sql("UPDATE HistoricosItemMissa SET FuncaoNome = 'Ceroferário', FuncaoAbreviacao = 'Ce' WHERE FuncaoId = 6;");

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Abreviacao", "Nome" },
                values: new object[] { "Ce", "Ceroferário" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Abreviacao", "Nome" },
                values: new object[] { "Ce1", "Ceroferario 1" });

            migrationBuilder.InsertData(
                table: "Funcoes",
                columns: new[] { "Id", "Abreviacao", "Cargo", "EhMultipla", "NivelMinimo", "Nome", "Obrigatoria" },
                values: new object[] { 7, "Ce2", 1, false, 3, "Ceroferario 2", false });
        }
    }
}
