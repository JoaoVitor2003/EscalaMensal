using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissaPadraoFuncaoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuncaoMissaPadrao",
                columns: table => new
                {
                    FuncoesId = table.Column<int>(type: "int", nullable: false),
                    MissaPadraoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncaoMissaPadrao", x => new { x.FuncoesId, x.MissaPadraoId });
                    table.ForeignKey(
                        name: "FK_FuncaoMissaPadrao_Funcoes_FuncoesId",
                        column: x => x.FuncoesId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuncaoMissaPadrao_MissasPadrao_MissaPadraoId",
                        column: x => x.MissaPadraoId,
                        principalTable: "MissasPadrao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuncaoMissaPadrao_MissaPadraoId",
                table: "FuncaoMissaPadrao",
                column: "MissaPadraoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuncaoMissaPadrao");
        }
    }
}
