using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustesGeraisMudancaDeLogica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensEscala_Escalas_EscalaId",
                table: "ItensEscala");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensEscala_Funcoes_FuncaoId",
                table: "ItensEscala");

            migrationBuilder.DropIndex(
                name: "IX_ItensEscala_EscalaId",
                table: "ItensEscala");

            migrationBuilder.DropColumn(
                name: "Dia",
                table: "ItensEscala");

            migrationBuilder.DropColumn(
                name: "EscalaId",
                table: "ItensEscala");

            migrationBuilder.DropColumn(
                name: "Horario",
                table: "ItensEscala");

            migrationBuilder.RenameColumn(
                name: "FuncaoId",
                table: "ItensEscala",
                newName: "MissaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensEscala_FuncaoId",
                table: "ItensEscala",
                newName: "IX_ItensEscala_MissaId");

            migrationBuilder.AddColumn<int>(
                name: "MissasId",
                table: "ItensEscala",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataFim",
                table: "Escalas",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataInicio",
                table: "Escalas",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateTable(
                name: "Missas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dia = table.Column<DateOnly>(type: "date", nullable: false),
                    Horario = table.Column<TimeOnly>(type: "time", nullable: false),
                    FuncaoId = table.Column<int>(type: "int", nullable: false),
                    EscalaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Missas_Escalas_EscalaId",
                        column: x => x.EscalaId,
                        principalTable: "Escalas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Missas_Funcoes_FuncaoId",
                        column: x => x.FuncaoId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensEscala_MissasId",
                table: "ItensEscala",
                column: "MissasId");

            migrationBuilder.CreateIndex(
                name: "IX_Missas_EscalaId",
                table: "Missas",
                column: "EscalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Missas_FuncaoId",
                table: "Missas",
                column: "FuncaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensEscala_Missas_MissaId",
                table: "ItensEscala",
                column: "MissaId",
                principalTable: "Missas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensEscala_Missas_MissasId",
                table: "ItensEscala",
                column: "MissasId",
                principalTable: "Missas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensEscala_Missas_MissaId",
                table: "ItensEscala");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensEscala_Missas_MissasId",
                table: "ItensEscala");

            migrationBuilder.DropTable(
                name: "Missas");

            migrationBuilder.DropIndex(
                name: "IX_ItensEscala_MissasId",
                table: "ItensEscala");

            migrationBuilder.DropColumn(
                name: "MissasId",
                table: "ItensEscala");

            migrationBuilder.DropColumn(
                name: "DataFim",
                table: "Escalas");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Escalas");

            migrationBuilder.RenameColumn(
                name: "MissaId",
                table: "ItensEscala",
                newName: "FuncaoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensEscala_MissaId",
                table: "ItensEscala",
                newName: "IX_ItensEscala_FuncaoId");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Dia",
                table: "ItensEscala",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "EscalaId",
                table: "ItensEscala",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Horario",
                table: "ItensEscala",
                type: "time",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensEscala_EscalaId",
                table: "ItensEscala",
                column: "EscalaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensEscala_Escalas_EscalaId",
                table: "ItensEscala",
                column: "EscalaId",
                principalTable: "Escalas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensEscala_Funcoes_FuncaoId",
                table: "ItensEscala",
                column: "FuncaoId",
                principalTable: "Funcoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
