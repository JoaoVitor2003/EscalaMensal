using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Obrigatoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Obrigatoria",
                table: "Funcoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Obrigatoria",
                value: true);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Obrigatoria",
                value: true);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Obrigatoria",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Obrigatoria",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Obrigatoria",
                table: "Funcoes");
        }
    }
}
