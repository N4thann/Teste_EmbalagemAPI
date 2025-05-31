using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SrManoelLoja.Migrations
{
    /// <inheritdoc />
    public partial class Correçãoemcamposdasentidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "COD_Prod",
                table: "Produtos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "COD_Prod",
                table: "Produtos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
