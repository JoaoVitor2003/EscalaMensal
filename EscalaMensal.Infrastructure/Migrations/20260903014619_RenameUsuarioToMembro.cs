using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalaMensal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameUsuarioToMembro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosItemMissa_Usuarios_UsuarioId",
                table: "HistoricosItemMissa");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Usuarios_UsuarioId",
                table: "ItensMissa");

            migrationBuilder.DropForeignKey(
                name: "FK_Restricoes_Usuarios_UsuarioId",
                table: "Restricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Usuarios_UsuarioVinculadoId",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "Membros");

            migrationBuilder.RenameColumn(
                name: "UsuarioVinculadoId",
                table: "Membros",
                newName: "MembroVinculadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_UsuarioVinculadoId",
                table: "Membros",
                newName: "IX_Membros_MembroVinculadoId");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Restricoes",
                newName: "MembroId");

            migrationBuilder.RenameIndex(
                name: "IX_Restricoes_UsuarioId",
                table: "Restricoes",
                newName: "IX_Restricoes_MembroId");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "ItensMissa",
                newName: "MembroId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensMissa_UsuarioId",
                table: "ItensMissa",
                newName: "IX_ItensMissa_MembroId");

            migrationBuilder.RenameColumn(
                name: "UsuarioNome",
                table: "HistoricosItemMissa",
                newName: "MembroNome");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "HistoricosItemMissa",
                newName: "MembroId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricosItemMissa_UsuarioId",
                table: "HistoricosItemMissa",
                newName: "IX_HistoricosItemMissa_MembroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Membros_Membros_MembroVinculadoId",
                table: "Membros",
                column: "MembroVinculadoId",
                principalTable: "Membros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosItemMissa_Membros_MembroId",
                table: "HistoricosItemMissa",
                column: "MembroId",
                principalTable: "Membros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensMissa_Membros_MembroId",
                table: "ItensMissa",
                column: "MembroId",
                principalTable: "Membros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restricoes_Membros_MembroId",
                table: "Restricoes",
                column: "MembroId",
                principalTable: "Membros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membros_Membros_MembroVinculadoId",
                table: "Membros");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosItemMissa_Membros_MembroId",
                table: "HistoricosItemMissa");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensMissa_Membros_MembroId",
                table: "ItensMissa");

            migrationBuilder.DropForeignKey(
                name: "FK_Restricoes_Membros_MembroId",
                table: "Restricoes");

            migrationBuilder.RenameTable(
                name: "Membros",
                newName: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "MembroVinculadoId",
                table: "Usuarios",
                newName: "UsuarioVinculadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Membros_MembroVinculadoId",
                table: "Usuarios",
                newName: "IX_Usuarios_UsuarioVinculadoId");

            migrationBuilder.RenameColumn(
                name: "MembroId",
                table: "Restricoes",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Restricoes_MembroId",
                table: "Restricoes",
                newName: "IX_Restricoes_UsuarioId");

            migrationBuilder.RenameColumn(
                name: "MembroId",
                table: "ItensMissa",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensMissa_MembroId",
                table: "ItensMissa",
                newName: "IX_ItensMissa_UsuarioId");

            migrationBuilder.RenameColumn(
                name: "MembroNome",
                table: "HistoricosItemMissa",
                newName: "UsuarioNome");

            migrationBuilder.RenameColumn(
                name: "MembroId",
                table: "HistoricosItemMissa",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricosItemMissa_MembroId",
                table: "HistoricosItemMissa",
                newName: "IX_HistoricosItemMissa_UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Usuarios_UsuarioVinculadoId",
                table: "Usuarios",
                column: "UsuarioVinculadoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosItemMissa_Usuarios_UsuarioId",
                table: "HistoricosItemMissa",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensMissa_Usuarios_UsuarioId",
                table: "ItensMissa",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restricoes_Usuarios_UsuarioId",
                table: "Restricoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
