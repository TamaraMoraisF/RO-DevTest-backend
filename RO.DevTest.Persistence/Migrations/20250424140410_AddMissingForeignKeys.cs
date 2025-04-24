using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RO.DevTest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetSales_CustomerId",
                table: "AspNetSales",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetSaleItems_ProductId",
                table: "AspNetSaleItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetSaleItems_AspNetProducts_ProductId",
                table: "AspNetSaleItems",
                column: "ProductId",
                principalTable: "AspNetProducts",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetSaleItems_AspNetProducts_ProductId",
                table: "AspNetSaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetSales_AspNetCustomers_CustomerId",
                table: "AspNetSales");

            migrationBuilder.DropIndex(
                name: "IX_AspNetSales_CustomerId",
                table: "AspNetSales");

            migrationBuilder.DropIndex(
                name: "IX_AspNetSaleItems_ProductId",
                table: "AspNetSaleItems");
        }
    }
}
