using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustesNovos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Funcoes",
                columns: new[] { "Id", "Abreviacao", "Cargo", "NivelMinimo", "Nome" },
                values: new object[,]
                {
                    { 1, "CG", 2, 3, "Cerimoniário Geral" },
                    { 2, "CA1", 2, 1, "Cerimoniário Auxiliar 1" },
                    { 3, "CA2", 2, 1, "Cerimoniário Auxiliar 2" },
                    { 4, "Li", 2, 3, "Librífero" },
                    { 5, "L", 1, 2, "Leitor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
