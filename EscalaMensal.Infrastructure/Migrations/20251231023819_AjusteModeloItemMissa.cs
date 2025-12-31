using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteModeloItemMissa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Cargo", "Nivel", "Nome" },
                values: new object[] { 2, 2, "Pedro" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "Cargo", "DiasEscalados", "DisponivelQuarta", "DisponivelQuinta", "DisponivelSabado", "HoraPreferencial", "Nivel", "Nome", "UsuarioVinculadoId" },
                values: new object[,]
                {
                    { 3, false, 2, null, true, true, true, new TimeOnly(7, 30, 0), 2, "Anna", null },
                    { 4, false, 1, null, true, true, true, new TimeOnly(7, 30, 0), 1, "Maria", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Cargo", "Nivel", "Nome" },
                values: new object[] { 1, 1, "Maria" });
        }
    }
}
