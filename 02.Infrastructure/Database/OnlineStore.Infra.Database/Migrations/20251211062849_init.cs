using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineStore.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Laptop products", "Laptop" },
                    { 2, "Mobile Products", "Mobile" },
                    { 3, "Accessories Products", "Accessories" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Balance", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 1, 300000m, "123", 1, "ali" },
                    { 2, 500000m, "zk123", 0, "zahra.k" },
                    { 3, 400000m, "1234", 0, "sahar" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Price", "Stock", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Gaming Laptop Ryzen 7, RTX 3060", 52000m, 10, "Laptop Asus ROG Strix G15" },
                    { 2, 1, "Apple M1, 8GB RAM, 256GB SSD", 45000m, 12, "Laptop Apple MacBook Air M1" },
                    { 3, 1, "Core i7, 16GB RAM, 512GB SSD", 58000m, 8, "Laptop Dell XPS 13" },
                    { 4, 1, "Core i5, 8GB RAM, 256GB SSD", 28000m, 15, "Laptop HP Pavilion 15" },
                    { 5, 1, "Ryzen 7, RTX 3050", 49000m, 9, "Laptop Lenovo Legion 5" },
                    { 6, 2, "Flagship Snapdragon 8 Gen2", 34000m, 20, "Samsung Galaxy S23" },
                    { 7, 2, "128GB, Super Retina XDR", 42000m, 17, "Apple iPhone 14" },
                    { 8, 2, "Great mid-range, 120Hz", 11000m, 30, "Xiaomi Redmi Note 12" },
                    { 9, 2, "Tensor G2, Amazing Camera", 28000m, 12, "Google Pixel 7" },
                    { 10, 2, "Reliable with clean Android", 9000m, 18, "Nokia X20" },
                    { 11, 3, "Ergonomic Wireless Mouse", 3500m, 22, "Logitech MX Master 3S" },
                    { 12, 3, "ANC, Spatial Audio", 9500m, 15, "Apple AirPods Pro 2" },
                    { 13, 3, "High-capacity Power Bank", 1800m, 25, "Anker PowerCore 20k" },
                    { 14, 3, "Noise Cancelling Wireless", 3800m, 14, "Sony WH-CH710N Headphones" }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "FileName", "FilePath", "ProductId" },
                values: new object[,]
                {
                    { 1, "asus_g15.jpg", "/images/products/asus_g15.jpg", 1 },
                    { 2, "macbook_air_m1.jpg", "/images/products/macbook_air_m1.jpg", 2 },
                    { 3, "dell_xps13.jpg", "/images/products/dell_xps13.jpg", 3 },
                    { 4, "hp_pavilion15.jpg", "/images/products/hp_pavilion15.jpg", 4 },
                    { 5, "lenovo_legion5.jpg", "/images/products/lenovo_legion5.jpg", 5 },
                    { 6, "samsung_s23.jpg", "/images/products/samsung_s23.jpg", 6 },
                    { 7, "iphone14.jpg", "/images/products/iphone14.jpg", 7 },
                    { 8, "redmi_note12.jpg", "/images/products/redmi_note12.jpg", 8 },
                    { 9, "pixel7.jpg", "/images/products/pixel7.jpg", 9 },
                    { 10, "nokia_x20.jpg", "/images/products/nokia_x20.jpg", 10 },
                    { 11, "mx_master3s.jpg", "/images/products/mx_master3s.jpg", 11 },
                    { 12, "airpods_pro2.jpg", "/images/products/airpods_pro2.jpg", 12 },
                    { 13, "anker_20k.jpg", "/images/products/anker_20k.jpg", 13 },
                    { 14, "sony_ch710n.jpg", "/images/products/sony_ch710n.jpg", 14 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
