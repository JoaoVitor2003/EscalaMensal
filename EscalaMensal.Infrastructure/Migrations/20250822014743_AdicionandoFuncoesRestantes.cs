using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoFuncoesRestantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Funcoes",
                columns: new[] { "Id", "Abreviacao", "Cargo", "NivelMinimo", "Nome" },
                values: new object[,]
                {
                    { 6, "Ce1", 1, 3, "Ceroferario 1" },
                    { 7, "Ce2", 1, 3, "Ceroferario 2" },
                    { 8, "Cr", 1, 2, "Cruciferário" },
                    { 9, "O", 1, 1, "Ofertório" },
                    { 10, "T", 2, 2, "Turibulo" },
                    { 11, "N", 1, 1, "Naveta" },
                    { 12, "CP", 2, 1, "Cerimoniário de Procissão" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
