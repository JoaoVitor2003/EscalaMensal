using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHorasPreferenciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraPreferencial",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "HorasPreferenciaisRaw",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "HorasPreferenciaisRaw",
                value: "10:00");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "HorasPreferenciaisRaw",
                value: "07:30");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "HorasPreferenciaisRaw",
                value: "07:30");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "HorasPreferenciaisRaw",
                value: "07:30");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorasPreferenciaisRaw",
                table: "Usuarios");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraPreferencial",
                table: "Usuarios",
                type: "time",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "HoraPreferencial",
                value: new TimeOnly(10, 0, 0));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "HoraPreferencial",
                value: new TimeOnly(7, 30, 0));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "HoraPreferencial",
                value: new TimeOnly(7, 30, 0));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "HoraPreferencial",
                value: new TimeOnly(7, 30, 0));
        }
    }
}
