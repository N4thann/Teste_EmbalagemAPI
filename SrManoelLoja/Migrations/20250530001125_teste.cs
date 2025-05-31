using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SrManoelLoja.Migrations
{
    /// <inheritdoc />
    public partial class teste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Pedido_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PedidoExterno_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Pedido_id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Produto_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    COD_Prod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Altura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Largura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comprimento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Produto_Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Pedido_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_PedidoId",
                table: "Produtos",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Pedidos");
        }
    }
}
