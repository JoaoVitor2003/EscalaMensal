using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdemEmItemMissa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "ItensMissa",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "ItensMissa");
        }
    }
}
