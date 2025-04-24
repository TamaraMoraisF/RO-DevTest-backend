using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RO.DevTest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RevertAspNetTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetSaleItems_AspNetProducts_ProductId",
                table: "AspNetSaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetSaleItems_AspNetSales_SaleId",
                table: "AspNetSaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetSales_AspNetCustomers_CustomerId",
                table: "AspNetSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetSales",
                table: "AspNetSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetSaleItems",
                table: "AspNetSaleItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetProducts",
                table: "AspNetProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetCustomers",
                table: "AspNetCustomers");

            migrationBuilder.RenameTable(
                name: "AspNetSales",
                newName: "Sales");

            migrationBuilder.RenameTable(
                name: "AspNetSaleItems",
                newName: "SaleItems");

            migrationBuilder.RenameTable(
                name: "AspNetProducts",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "AspNetCustomers",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetSales_CustomerId",
                table: "Sales",
                newName: "IX_Sales_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetSaleItems_SaleId",
                table: "SaleItems",
                newName: "IX_SaleItems_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetSaleItems_ProductId",
                table: "SaleItems",
                newName: "IX_SaleItems_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleItems",
                table: "SaleItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_Products_ProductId",
                table: "SaleItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_Sales_SaleId",
                table: "SaleItems",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                table: "Sales",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_Products_ProductId",
                table: "SaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_Sales_SaleId",
                table: "SaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleItems",
                table: "SaleItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Sales",
                newName: "AspNetSales");

            migrationBuilder.RenameTable(
                name: "SaleItems",
                newName: "AspNetSaleItems");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "AspNetProducts");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "AspNetCustomers");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_CustomerId",
                table: "AspNetSales",
                newName: "IX_AspNetSales_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_SaleItems_SaleId",
                table: "AspNetSaleItems",
                newName: "IX_AspNetSaleItems_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_SaleItems_ProductId",
                table: "AspNetSaleItems",
                newName: "IX_AspNetSaleItems_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetSales",
                table: "AspNetSales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetSaleItems",
                table: "AspNetSaleItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetProducts",
                table: "AspNetProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetCustomers",
                table: "AspNetCustomers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetSaleItems_AspNetProducts_ProductId",
                table: "AspNetSaleItems",
                column: "ProductId",
                principalTable: "AspNetProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetSaleItems_AspNetSales_SaleId",
                table: "AspNetSaleItems",
                column: "SaleId",
                principalTable: "AspNetSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetSales_AspNetCustomers_CustomerId",
                table: "AspNetSales",
                column: "CustomerId",
                principalTable: "AspNetCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
