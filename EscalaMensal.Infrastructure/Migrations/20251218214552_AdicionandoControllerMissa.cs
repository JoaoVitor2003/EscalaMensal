using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoControllerMissa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensEscala_Missas_MissaId",
                table: "ItensEscala");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensEscala_Missas_MissasId",
                table: "ItensEscala");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensEscala_Usuarios_UsuarioId",
                table: "ItensEscala");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensEscala",
                table: "ItensEscala");

            migrationBuilder.RenameTable(
                name: "ItensEscala",
                newName: "ItensMissa");

            migrationBuilder.RenameIndex(
                name: "IX_ItensEscala_UsuarioId",
                table: "ItensMissa",
                newName: "IX_ItensMissa_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensEscala_MissasId",
                table: "ItensMissa",
                newName: "IX_ItensMissa_MissasId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensEscala_MissaId",
                table: "ItensMissa",
                newName: "IX_ItensMissa_MissaId");

            migrationBuilder.AddColumn<int>(
                name: "DiasEscalados",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensMissa",
                table: "ItensMissa",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "DiasEscalados",
                value: null);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "DiasEscalados",
                value: null);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ItensMissa_Usuarios_UsuarioId",
                table: "ItensMissa",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Missas_MissaId",
                table: "ItensMissa");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Missas_MissasId",
                table: "ItensMissa");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Usuarios_UsuarioId",
                table: "ItensMissa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensMissa",
                table: "ItensMissa");

            migrationBuilder.DropColumn(
                name: "DiasEscalados",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "ItensMissa",
                newName: "ItensEscala");

            migrationBuilder.RenameIndex(
                name: "IX_ItensMissa_UsuarioId",
                table: "ItensEscala",
                newName: "IX_ItensEscala_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensMissa_MissasId",
                table: "ItensEscala",
                newName: "IX_ItensEscala_MissasId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensMissa_MissaId",
                table: "ItensEscala",
                newName: "IX_ItensEscala_MissaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensEscala",
                table: "ItensEscala",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ItensEscala_Usuarios_UsuarioId",
                table: "ItensEscala",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
