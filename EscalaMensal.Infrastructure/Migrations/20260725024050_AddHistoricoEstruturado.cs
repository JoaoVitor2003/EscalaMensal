using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoricoEstruturado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosEscala_Funcoes_FuncaoId",
                table: "HistoricosEscala");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosEscala_Usuarios_UsuarioId",
                table: "HistoricosEscala");

            migrationBuilder.DropIndex(
                name: "IX_HistoricosEscala_FuncaoId",
                table: "HistoricosEscala");

            migrationBuilder.DropIndex(
                name: "IX_HistoricosEscala_UsuarioId",
                table: "HistoricosEscala");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "HistoricosEscala",
                newName: "LimitePermitido");

            migrationBuilder.RenameColumn(
                name: "FuncaoId",
                table: "HistoricosEscala",
                newName: "EscalaOriginalId");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "HistoricosEscala",
                newName: "DataFinalizacao");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataFim",
                table: "HistoricosEscala",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataInicio",
                table: "HistoricosEscala",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateTable(
                name: "HistoricosMissa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MissaOriginalId = table.Column<int>(type: "int", nullable: false),
                    HistoricoEscalaId = table.Column<int>(type: "int", nullable: false),
                    Dia = table.Column<DateOnly>(type: "date", nullable: false),
                    Horario = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosMissa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricosMissa_HistoricosEscala_HistoricoEscalaId",
                        column: x => x.HistoricoEscalaId,
                        principalTable: "HistoricosEscala",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricosItemMissa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemMissaOriginalId = table.Column<int>(type: "int", nullable: false),
                    HistoricoMissaId = table.Column<int>(type: "int", nullable: false),
                    FuncaoId = table.Column<int>(type: "int", nullable: false),
                    FuncaoNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuncaoAbreviacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    UsuarioNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosItemMissa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricosItemMissa_Funcoes_FuncaoId",
                        column: x => x.FuncaoId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoricosItemMissa_HistoricosMissa_HistoricoMissaId",
                        column: x => x.HistoricoMissaId,
                        principalTable: "HistoricosMissa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricosItemMissa_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosItemMissa_FuncaoId",
                table: "HistoricosItemMissa",
                column: "FuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosItemMissa_HistoricoMissaId",
                table: "HistoricosItemMissa",
                column: "HistoricoMissaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosItemMissa_UsuarioId",
                table: "HistoricosItemMissa",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosMissa_HistoricoEscalaId",
                table: "HistoricosMissa",
                column: "HistoricoEscalaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricosItemMissa");

            migrationBuilder.DropTable(
                name: "HistoricosMissa");

            migrationBuilder.DropColumn(
                name: "DataFim",
                table: "HistoricosEscala");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "HistoricosEscala");

            migrationBuilder.RenameColumn(
                name: "LimitePermitido",
                table: "HistoricosEscala",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "EscalaOriginalId",
                table: "HistoricosEscala",
                newName: "FuncaoId");

            migrationBuilder.RenameColumn(
                name: "DataFinalizacao",
                table: "HistoricosEscala",
                newName: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosEscala_FuncaoId",
                table: "HistoricosEscala",
                column: "FuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosEscala_UsuarioId",
                table: "HistoricosEscala",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosEscala_Funcoes_FuncaoId",
                table: "HistoricosEscala",
                column: "FuncaoId",
                principalTable: "Funcoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosEscala_Usuarios_UsuarioId",
                table: "HistoricosEscala",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
