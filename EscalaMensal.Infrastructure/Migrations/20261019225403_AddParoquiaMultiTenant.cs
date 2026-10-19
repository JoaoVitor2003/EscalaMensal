using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddParoquiaMultiTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParoquiaId",
                table: "MissasPadrao",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParoquiaId",
                table: "Membros",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParoquiaId",
                table: "HorariosFixos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParoquiaId",
                table: "Funcoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CriadoPorUsuarioId",
                table: "Escalas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParoquiaId",
                table: "Escalas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EscalaCompartilhamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EscalaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Permissao = table.Column<int>(type: "int", nullable: false),
                    CodigoAcesso = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CriadoPorUsuarioId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalaCompartilhamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscalaCompartilhamentos_Escalas_EscalaId",
                        column: x => x.EscalaId,
                        principalTable: "Escalas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EscalaCompartilhamentos_Usuarios_CriadoPorUsuarioId",
                        column: x => x.CriadoPorUsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EscalaCompartilhamentos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paroquias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Endereco = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CodigoAcesso = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CriadoPorUsuarioId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paroquias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paroquias_Usuarios_CriadoPorUsuarioId",
                        column: x => x.CriadoPorUsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReivindicacoesParoquia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ParoquiaId = table.Column<int>(type: "int", nullable: false),
                    Justificativa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefoneContato = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataResposta = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReivindicacoesParoquia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReivindicacoesParoquia_Paroquias_ParoquiaId",
                        column: x => x.ParoquiaId,
                        principalTable: "Paroquias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReivindicacoesParoquia_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacoesAcessoParoquia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ParoquiaId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataResposta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RespondidoPorUsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacoesAcessoParoquia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitacoesAcessoParoquia_Paroquias_ParoquiaId",
                        column: x => x.ParoquiaId,
                        principalTable: "Paroquias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacoesAcessoParoquia_Usuarios_RespondidoPorUsuarioId",
                        column: x => x.RespondidoPorUsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitacoesAcessoParoquia_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioParoquias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ParoquiaId = table.Column<int>(type: "int", nullable: false),
                    DataVinculo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TentativasCodigoErrado = table.Column<int>(type: "int", nullable: false),
                    BloqueadoParaCodigo = table.Column<bool>(type: "bit", nullable: false),
                    DataUltimoBloqueio = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioParoquias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioParoquias_Paroquias_ParoquiaId",
                        column: x => x.ParoquiaId,
                        principalTable: "Paroquias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioParoquias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 11,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funcoes",
                keyColumn: "Id",
                keyValue: 12,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "HorariosFixos",
                keyColumn: "Id",
                keyValue: 1,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "HorariosFixos",
                keyColumn: "Id",
                keyValue: 2,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "HorariosFixos",
                keyColumn: "Id",
                keyValue: 3,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "HorariosFixos",
                keyColumn: "Id",
                keyValue: 4,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Membros",
                keyColumn: "Id",
                keyValue: 1,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Membros",
                keyColumn: "Id",
                keyValue: 2,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Membros",
                keyColumn: "Id",
                keyValue: 3,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Membros",
                keyColumn: "Id",
                keyValue: 4,
                column: "ParoquiaId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_MissasPadrao_ParoquiaId",
                table: "MissasPadrao",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Membros_ParoquiaId",
                table: "Membros",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosFixos_ParoquiaId",
                table: "HorariosFixos",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcoes_ParoquiaId",
                table: "Funcoes",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Escalas_CriadoPorUsuarioId",
                table: "Escalas",
                column: "CriadoPorUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Escalas_ParoquiaId",
                table: "Escalas",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_EscalaCompartilhamentos_CriadoPorUsuarioId",
                table: "EscalaCompartilhamentos",
                column: "CriadoPorUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EscalaCompartilhamentos_EscalaId",
                table: "EscalaCompartilhamentos",
                column: "EscalaId");

            migrationBuilder.CreateIndex(
                name: "IX_EscalaCompartilhamentos_UsuarioId",
                table: "EscalaCompartilhamentos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Paroquias_CodigoAcesso",
                table: "Paroquias",
                column: "CodigoAcesso");

            migrationBuilder.CreateIndex(
                name: "IX_Paroquias_CriadoPorUsuarioId",
                table: "Paroquias",
                column: "CriadoPorUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReivindicacoesParoquia_ParoquiaId",
                table: "ReivindicacoesParoquia",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReivindicacoesParoquia_UsuarioId",
                table: "ReivindicacoesParoquia",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesAcessoParoquia_ParoquiaId",
                table: "SolicitacoesAcessoParoquia",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesAcessoParoquia_RespondidoPorUsuarioId",
                table: "SolicitacoesAcessoParoquia",
                column: "RespondidoPorUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesAcessoParoquia_UsuarioId",
                table: "SolicitacoesAcessoParoquia",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioParoquias_ParoquiaId",
                table: "UsuarioParoquias",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioParoquias_UsuarioId_ParoquiaId",
                table: "UsuarioParoquias",
                columns: new[] { "UsuarioId", "ParoquiaId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Escalas_Paroquias_ParoquiaId",
                table: "Escalas",
                column: "ParoquiaId",
                principalTable: "Paroquias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Escalas_Usuarios_CriadoPorUsuarioId",
                table: "Escalas",
                column: "CriadoPorUsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Funcoes_Paroquias_ParoquiaId",
                table: "Funcoes",
                column: "ParoquiaId",
                principalTable: "Paroquias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HorariosFixos_Paroquias_ParoquiaId",
                table: "HorariosFixos",
                column: "ParoquiaId",
                principalTable: "Paroquias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Membros_Paroquias_ParoquiaId",
                table: "Membros",
                column: "ParoquiaId",
                principalTable: "Paroquias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MissasPadrao_Paroquias_ParoquiaId",
                table: "MissasPadrao",
                column: "ParoquiaId",
                principalTable: "Paroquias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Escalas_Paroquias_ParoquiaId",
                table: "Escalas");

            migrationBuilder.DropForeignKey(
                name: "FK_Escalas_Usuarios_CriadoPorUsuarioId",
                table: "Escalas");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcoes_Paroquias_ParoquiaId",
                table: "Funcoes");

            migrationBuilder.DropForeignKey(
                name: "FK_HorariosFixos_Paroquias_ParoquiaId",
                table: "HorariosFixos");

            migrationBuilder.DropForeignKey(
                name: "FK_Membros_Paroquias_ParoquiaId",
                table: "Membros");

            migrationBuilder.DropForeignKey(
                name: "FK_MissasPadrao_Paroquias_ParoquiaId",
                table: "MissasPadrao");

            migrationBuilder.DropTable(
                name: "EscalaCompartilhamentos");

            migrationBuilder.DropTable(
                name: "ReivindicacoesParoquia");

            migrationBuilder.DropTable(
                name: "SolicitacoesAcessoParoquia");

            migrationBuilder.DropTable(
                name: "UsuarioParoquias");

            migrationBuilder.DropTable(
                name: "Paroquias");

            migrationBuilder.DropIndex(
                name: "IX_MissasPadrao_ParoquiaId",
                table: "MissasPadrao");

            migrationBuilder.DropIndex(
                name: "IX_Membros_ParoquiaId",
                table: "Membros");

            migrationBuilder.DropIndex(
                name: "IX_HorariosFixos_ParoquiaId",
                table: "HorariosFixos");

            migrationBuilder.DropIndex(
                name: "IX_Funcoes_ParoquiaId",
                table: "Funcoes");

            migrationBuilder.DropIndex(
                name: "IX_Escalas_CriadoPorUsuarioId",
                table: "Escalas");

            migrationBuilder.DropIndex(
                name: "IX_Escalas_ParoquiaId",
                table: "Escalas");

            migrationBuilder.DropColumn(
                name: "ParoquiaId",
                table: "MissasPadrao");

            migrationBuilder.DropColumn(
                name: "ParoquiaId",
                table: "Membros");

            migrationBuilder.DropColumn(
                name: "ParoquiaId",
                table: "HorariosFixos");

            migrationBuilder.DropColumn(
                name: "ParoquiaId",
                table: "Funcoes");

            migrationBuilder.DropColumn(
                name: "CriadoPorUsuarioId",
                table: "Escalas");

            migrationBuilder.DropColumn(
                name: "ParoquiaId",
                table: "Escalas");
        }
    }
}
