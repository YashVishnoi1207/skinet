using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CartEntityAddedUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PorductName",
                table: "CartItem",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartItem",
                newName: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "CartItem",
                newName: "PorductName");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartItem",
                newName: "Id");
        }
    }
}
