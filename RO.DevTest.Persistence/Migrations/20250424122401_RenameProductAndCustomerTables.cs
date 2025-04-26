using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RO.DevTest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductAndCustomerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "AspNetProducts");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "AspNetCustomers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetProducts",
                table: "AspNetProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetCustomers",
                table: "AspNetCustomers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetProducts",
                table: "AspNetProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetCustomers",
                table: "AspNetCustomers");

            migrationBuilder.RenameTable(
                name: "AspNetProducts",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "AspNetCustomers",
                newName: "Customers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");
        }
    }
}
