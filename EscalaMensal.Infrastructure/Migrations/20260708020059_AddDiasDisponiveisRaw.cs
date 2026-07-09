using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDiasDisponiveisRaw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisponivelQuarta",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DisponivelQuinta",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DisponivelSabado",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "DiasDisponiveisRaw",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "DiasDisponiveisRaw",
                value: "Saturday");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "DiasDisponiveisRaw",
                value: "Wednesday,Thursday,Saturday");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "DiasDisponiveisRaw",
                value: "Wednesday,Thursday,Saturday");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "DiasDisponiveisRaw",
                value: "Wednesday,Thursday,Saturday");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiasDisponiveisRaw",
                table: "Usuarios");

            migrationBuilder.AddColumn<bool>(
                name: "DisponivelQuarta",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisponivelQuinta",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisponivelSabado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DisponivelQuarta", "DisponivelQuinta", "DisponivelSabado" },
                values: new object[] { false, false, true });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DisponivelQuarta", "DisponivelQuinta", "DisponivelSabado" },
                values: new object[] { true, true, true });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DisponivelQuarta", "DisponivelQuinta", "DisponivelSabado" },
                values: new object[] { true, true, true });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DisponivelQuarta", "DisponivelQuinta", "DisponivelSabado" },
                values: new object[] { true, true, true });
        }
    }
}
