using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RO.DevTest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameSalesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_Sales_SaleId",
                table: "SaleItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleItems",
                table: "SaleItems");

            migrationBuilder.RenameTable(
                name: "Sales",
                newName: "AspNetSales");

            migrationBuilder.RenameTable(
                name: "SaleItems",
                newName: "AspNetSaleItems");

            migrationBuilder.RenameIndex(
                name: "IX_SaleItems_SaleId",
                table: "AspNetSaleItems",
                newName: "IX_AspNetSaleItems_SaleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetSales",
                table: "AspNetSales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetSaleItems",
                table: "AspNetSaleItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetSaleItems_AspNetSales_SaleId",
                table: "AspNetSaleItems",
                column: "SaleId",
                principalTable: "AspNetSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetSaleItems_AspNetSales_SaleId",
                table: "AspNetSaleItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetSales",
                table: "AspNetSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetSaleItems",
                table: "AspNetSaleItems");

            migrationBuilder.RenameTable(
                name: "AspNetSales",
                newName: "Sales");

            migrationBuilder.RenameTable(
                name: "AspNetSaleItems",
                newName: "SaleItems");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetSaleItems_SaleId",
                table: "SaleItems",
                newName: "IX_SaleItems_SaleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleItems",
                table: "SaleItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_Sales_SaleId",
                table: "SaleItems",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
