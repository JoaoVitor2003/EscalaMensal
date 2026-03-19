using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Escalas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    DataFim = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escalas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abreviacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cargo = table.Column<int>(type: "int", nullable: false),
                    NivelMinimo = table.Column<int>(type: "int", nullable: false),
                    Obrigatoria = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Cargo = table.Column<int>(type: "int", nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    HoraPreferencial = table.Column<TimeOnly>(type: "time", nullable: true),
                    DisponivelSabado = table.Column<bool>(type: "bit", nullable: false),
                    DisponivelQuarta = table.Column<bool>(type: "bit", nullable: false),
                    DisponivelQuinta = table.Column<bool>(type: "bit", nullable: false),
                    LimitePermitido = table.Column<int>(type: "int", nullable: true, defaultValue: 3),
                    UsuarioVinculadoId = table.Column<int>(type: "int", nullable: true),
                    DiasEscalados = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Usuarios_UsuarioVinculadoId",
                        column: x => x.UsuarioVinculadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Missas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dia = table.Column<DateOnly>(type: "date", nullable: false),
                    Horario = table.Column<TimeOnly>(type: "time", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "CargoNivelFuncaoPermitidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cargo = table.Column<int>(type: "int", nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    FuncaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoNivelFuncaoPermitidas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CargoNivelFuncaoPermitidas_Funcoes_FuncaoId",
                        column: x => x.FuncaoId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricosEscala",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FuncaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosEscala", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricosEscala_Funcoes_FuncaoId",
                        column: x => x.FuncaoId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoricosEscala_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Restricoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restricoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restricoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensMissa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MissaId = table.Column<int>(type: "int", nullable: false),
                    FuncaoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensMissa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensMissa_Funcoes_FuncaoId",
                        column: x => x.FuncaoId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensMissa_Missas_MissaId",
                        column: x => x.MissaId,
                        principalTable: "Missas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensMissa_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Funcoes",
                columns: new[] { "Id", "Abreviacao", "Cargo", "NivelMinimo", "Nome", "Obrigatoria" },
                values: new object[,]
                {
                    { 1, "CG", 2, 3, "Cerimoniário Geral", true },
                    { 2, "CA1", 2, 1, "Cerimoniário Auxiliar 1", false },
                    { 3, "CA2", 2, 1, "Cerimoniário Auxiliar 2", false },
                    { 4, "Li", 2, 3, "Librífero", true },
                    { 5, "L", 1, 2, "Leitor", false },
                    { 6, "Ce1", 1, 3, "Ceroferario 1", false },
                    { 7, "Ce2", 1, 3, "Ceroferario 2", false },
                    { 8, "Cr", 1, 2, "Cruciferário", false },
                    { 9, "O", 1, 1, "Ofertório", false },
                    { 10, "T", 2, 2, "Turibulo", false },
                    { 11, "N", 1, 1, "Naveta", false },
                    { 12, "CP", 2, 1, "Cerimoniário de Procissão", false }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "Cargo", "DiasEscalados", "DisponivelQuarta", "DisponivelQuinta", "DisponivelSabado", "HoraPreferencial", "LimitePermitido", "Nivel", "Nome", "UsuarioVinculadoId" },
                values: new object[,]
                {
                    { 1, true, 2, null, false, false, true, new TimeOnly(10, 0, 0), 3, 3, "João", null },
                    { 2, false, 2, null, true, true, true, new TimeOnly(7, 30, 0), 3, 2, "Pedro", null },
                    { 3, false, 2, null, true, true, true, new TimeOnly(7, 30, 0), 3, 2, "Anna", null },
                    { 4, false, 1, null, true, true, true, new TimeOnly(7, 30, 0), 3, 1, "Maria", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CargoNivelFuncaoPermitidas_Cargo_Nivel_FuncaoId",
                table: "CargoNivelFuncaoPermitidas",
                columns: new[] { "Cargo", "Nivel", "FuncaoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CargoNivelFuncaoPermitidas_FuncaoId",
                table: "CargoNivelFuncaoPermitidas",
                column: "FuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosEscala_FuncaoId",
                table: "HistoricosEscala",
                column: "FuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosEscala_UsuarioId",
                table: "HistoricosEscala",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensMissa_FuncaoId",
                table: "ItensMissa",
                column: "FuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensMissa_MissaId",
                table: "ItensMissa",
                column: "MissaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensMissa_UsuarioId",
                table: "ItensMissa",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Missas_EscalaId",
                table: "Missas",
                column: "EscalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Restricoes_UsuarioId",
                table: "Restricoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UsuarioVinculadoId",
                table: "Usuarios",
                column: "UsuarioVinculadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargoNivelFuncaoPermitidas");

            migrationBuilder.DropTable(
                name: "HistoricosEscala");

            migrationBuilder.DropTable(
                name: "ItensMissa");

            migrationBuilder.DropTable(
                name: "Restricoes");

            migrationBuilder.DropTable(
                name: "Funcoes");

            migrationBuilder.DropTable(
                name: "Missas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Escalas");
        }
    }
}
