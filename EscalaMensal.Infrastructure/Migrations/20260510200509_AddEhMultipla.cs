using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEhMultipla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EhMultipla",
                table: "Funcoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 11,
                column: "EhMultipla",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 12,
                column: "EhMultipla",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EhMultipla",
                table: "Funcoes");
        }
    }
}
