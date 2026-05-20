using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLimitePermitidoToEscala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LimitePermitido",
                table: "Escalas",
                type: "int",
                nullable: false,
                defaultValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitePermitido",
                table: "Escalas");
        }
    }
}
