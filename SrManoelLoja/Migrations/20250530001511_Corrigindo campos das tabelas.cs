using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SrManoelLoja.Migrations
{
    /// <inheritdoc />
    public partial class Corrigindocamposdastabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Produto_Id",
                table: "Produtos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Pedido_id",
                table: "Pedidos",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Produtos",
                newName: "Produto_Id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Pedidos",
                newName: "Pedido_id");
        }
    }
}
