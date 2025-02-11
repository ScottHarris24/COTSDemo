using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace COTSDemo.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderDetailStatus = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "BillingAddress", "Created", "CreatedBy", "FirstName", "LastName", "LastUpdated", "LastUpdatedBy", "ShippingAddress" },
                values: new object[,]
                {
                    { 1, "Billing Address 1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "First 1", "Last 1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "Shipping Address 1" },
                    { 2, "Billing Address 2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "First 2", "Last 2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "Shipping Address 2" },
                    { 3, "Billing Address 3", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "First 3", "Last 3", new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "Shipping Address 3" },
                    { 4, "Billing Address 4", new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "First 4", "Last 4", new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", "Shipping Address 4" },
                    { 5, "Billing Address 5", new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "First 5", "Last 5", new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", "Shipping Address 5" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Created", "CreatedBy", "LastUpdated", "LastUpdatedBy", "Name", "Price", "QuantityOnHand" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "Product 1", 1.00m, 10 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "Product 2", 2.21m, 10 },
                    { 3, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", "Product 3", 3.32m, 25 },
                    { 4, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", "Product 4", 4.50m, 1 },
                    { 5, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", "Product 5", 4.75m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Created", "CreatedBy", "CustomerId", "LastUpdated", "LastUpdatedBy", "OrderDate", "ShippedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 1, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 1, new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 2, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 2, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 2, new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "Created", "CreatedBy", "LastUpdated", "LastUpdatedBy", "OrderDetailStatus", "OrderId", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 3, 1, 1, 5 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 4, 1, 2, 1 },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", 4, 1, 3, 100 },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 4, 2, 1, 1 },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 4, 2, 3, 8 },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 5, 5, 1, 1 },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 3, 6, 1, 5 },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 4, 6, 2, 1 },
                    { 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", 4, 6, 3, 100 },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 4, 7, 1, 1 },
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 4, 7, 3, 8 },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User1", 5, 10, 1, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
