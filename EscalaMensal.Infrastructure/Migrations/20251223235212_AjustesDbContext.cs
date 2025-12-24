using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustesDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Missas_MissaId",
                table: "ItensMissa");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Missas_MissasId",
                table: "ItensMissa");

            migrationBuilder.DropIndex(
                name: "IX_ItensMissa_MissasId",
                table: "ItensMissa");

            migrationBuilder.DropColumn(
                name: "MissasId",
                table: "ItensMissa");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensMissa_Missas_MissaId",
                table: "ItensMissa",
                column: "MissaId",
                principalTable: "Missas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Missas_MissaId",
                table: "ItensMissa");

            migrationBuilder.AddColumn<int>(
                name: "MissasId",
                table: "ItensMissa",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensMissa_MissasId",
                table: "ItensMissa",
                column: "MissasId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensMissa_Missas_MissaId",
                table: "ItensMissa",
                column: "MissaId",
                principalTable: "Missas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensMissa_Missas_MissasId",
                table: "ItensMissa",
                column: "MissasId",
                principalTable: "Missas",
                principalColumn: "Id");
        }
    }
}
