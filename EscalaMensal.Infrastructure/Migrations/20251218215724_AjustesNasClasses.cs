using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustesNasClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Funcoes_FuncaoId",
                table: "Missas");

            migrationBuilder.DropIndex(
                name: "IX_Missas_FuncaoId",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "FuncaoId",
                table: "Missas");

            migrationBuilder.AddColumn<int>(
                name: "FuncaoId",
                table: "ItensMissa",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItensMissa_FuncaoId",
                table: "ItensMissa",
                column: "FuncaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensMissa_Funcoes_FuncaoId",
                table: "ItensMissa",
                column: "FuncaoId",
                principalTable: "Funcoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Funcoes_FuncaoId",
                table: "ItensMissa");

            migrationBuilder.DropIndex(
                name: "IX_ItensMissa_FuncaoId",
                table: "ItensMissa");

            migrationBuilder.DropColumn(
                name: "FuncaoId",
                table: "ItensMissa");

            migrationBuilder.AddColumn<int>(
                name: "FuncaoId",
                table: "Missas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Missas_FuncaoId",
                table: "Missas",
                column: "FuncaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missas_Funcoes_FuncaoId",
                table: "Missas",
                column: "FuncaoId",
                principalTable: "Funcoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
