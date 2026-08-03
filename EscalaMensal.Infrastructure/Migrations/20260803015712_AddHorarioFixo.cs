using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHorarioFixo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HorariosFixos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Horario = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosFixos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HorariosFixos",
                columns: new[] { "Id", "Horario" },
                values: new object[,]
                {
                    { 1, new TimeOnly(7, 30, 0) },
                    { 2, new TimeOnly(9, 0, 0) },
                    { 3, new TimeOnly(11, 0, 0) },
                    { 4, new TimeOnly(19, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HorariosFixos");
        }
    }
}
