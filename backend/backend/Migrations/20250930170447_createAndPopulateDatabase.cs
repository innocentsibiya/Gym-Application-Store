using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class createAndPopulateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DiscountType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsageLimit = table.Column<int>(type: "integer", nullable: true),
                    UsedCount = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SKU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Dimensions = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Province = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AddressType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "integer", nullable: false),
                    ReorderLevel = table.Column<int>(type: "integer", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    AltText = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    OrderStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShippingAddressId = table.Column<long>(type: "bigint", nullable: false),
                    BillingAddressId = table.Column<long>(type: "bigint", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WishlistId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Wishlists_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DiscountApplied = table.Column<decimal>(type: "numeric(18,2)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Carrier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TrackingNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "ParentCategoryId", "Slug" },
                values: new object[,]
                {
                    { 1L, "Weights and resistance equipment", null, "Strength Training", null, "strength-training" },
                    { 2L, "Treadmills, bikes, ellipticals", null, "Cardio Equipment", null, "cardio-equipment" },
                    { 3L, "Mats, gloves, and small gear", null, "Accessories", null, "accessories" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "ContactName", "CreatedAt", "Email", "Name", "Phone" },
                values: new object[,]
                {
                    { 1L, null, null, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "contact@fitgear.com", "FitGear Supplier", "1234567890" },
                    { 2L, null, null, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "sales@powerlift.com", "PowerLift Supplier", "0987654321" },
                    { 3L, null, null, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "info@zenfit.com", "ZenFit Supplier", "1112223333" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "ParentCategoryId", "Slug" },
                values: new object[,]
                {
                    { 4L, null, null, "Dumbbells", 1L, "dumbbells" },
                    { 5L, null, null, "Kettlebells", 1L, "kettlebells" },
                    { 6L, null, null, "Barbells", 1L, "barbells" },
                    { 7L, null, null, "Weight Plates", 1L, "weight-plates" },
                    { 8L, null, null, "Treadmills", 2L, "treadmills" },
                    { 9L, null, null, "Exercise Bikes", 2L, "exercise-bikes" },
                    { 10L, null, null, "Ellipticals", 2L, "ellipticals" },
                    { 11L, null, null, "Rowing Machines", 2L, "rowing-machines" },
                    { 12L, null, null, "Yoga Mats", 3L, "yoga-mats" },
                    { 13L, null, null, "Foam Rollers", 3L, "foam-rollers" },
                    { 14L, null, null, "Resistance Bands", 3L, "resistance-bands" },
                    { 15L, null, null, "Gloves", 3L, "gloves" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "CreatedAt", "Description", "Dimensions", "DiscountPrice", "IsActive", "Name", "Price", "SKU", "Slug", "StockQuantity", "UpdatedAt", "Weight" },
                values: new object[,]
                {
                    { 1L, "Brand1", 4L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality dumbbells item number 1", null, null, true, "Dumbbells Product 1", 60m, "DUMBBELLS-001", "dumbbells-product-1", 25, null, null },
                    { 2L, "Brand2", 4L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality dumbbells item number 2", null, null, true, "Dumbbells Product 2", 70m, "DUMBBELLS-002", "dumbbells-product-2", 30, null, null },
                    { 3L, "Brand3", 4L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality dumbbells item number 3", null, null, true, "Dumbbells Product 3", 80m, "DUMBBELLS-003", "dumbbells-product-3", 35, null, null },
                    { 4L, "Brand4", 4L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality dumbbells item number 4", null, null, true, "Dumbbells Product 4", 90m, "DUMBBELLS-004", "dumbbells-product-4", 40, null, null },
                    { 5L, "Brand5", 4L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality dumbbells item number 5", null, null, true, "Dumbbells Product 5", 100m, "DUMBBELLS-005", "dumbbells-product-5", 45, null, null },
                    { 6L, "Brand1", 5L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality kettlebells item number 1", null, null, true, "Kettlebells Product 1", 60m, "KETTLEBELLS-001", "kettlebells-product-1", 25, null, null },
                    { 7L, "Brand2", 5L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality kettlebells item number 2", null, null, true, "Kettlebells Product 2", 70m, "KETTLEBELLS-002", "kettlebells-product-2", 30, null, null },
                    { 8L, "Brand3", 5L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality kettlebells item number 3", null, null, true, "Kettlebells Product 3", 80m, "KETTLEBELLS-003", "kettlebells-product-3", 35, null, null },
                    { 9L, "Brand4", 5L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality kettlebells item number 4", null, null, true, "Kettlebells Product 4", 90m, "KETTLEBELLS-004", "kettlebells-product-4", 40, null, null },
                    { 10L, "Brand5", 5L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality kettlebells item number 5", null, null, true, "Kettlebells Product 5", 100m, "KETTLEBELLS-005", "kettlebells-product-5", 45, null, null },
                    { 11L, "Brand1", 6L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality barbells item number 1", null, null, true, "Barbells Product 1", 60m, "BARBELLS-001", "barbells-product-1", 25, null, null },
                    { 12L, "Brand2", 6L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality barbells item number 2", null, null, true, "Barbells Product 2", 70m, "BARBELLS-002", "barbells-product-2", 30, null, null },
                    { 13L, "Brand3", 6L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality barbells item number 3", null, null, true, "Barbells Product 3", 80m, "BARBELLS-003", "barbells-product-3", 35, null, null },
                    { 14L, "Brand4", 6L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality barbells item number 4", null, null, true, "Barbells Product 4", 90m, "BARBELLS-004", "barbells-product-4", 40, null, null },
                    { 15L, "Brand5", 6L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality barbells item number 5", null, null, true, "Barbells Product 5", 100m, "BARBELLS-005", "barbells-product-5", 45, null, null },
                    { 16L, "Brand1", 7L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality weight plates item number 1", null, null, true, "Weight Plates Product 1", 60m, "WEIGHT-PLATES-001", "weight-plates-product-1", 25, null, null },
                    { 17L, "Brand2", 7L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality weight plates item number 2", null, null, true, "Weight Plates Product 2", 70m, "WEIGHT-PLATES-002", "weight-plates-product-2", 30, null, null },
                    { 18L, "Brand3", 7L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality weight plates item number 3", null, null, true, "Weight Plates Product 3", 80m, "WEIGHT-PLATES-003", "weight-plates-product-3", 35, null, null },
                    { 19L, "Brand4", 7L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality weight plates item number 4", null, null, true, "Weight Plates Product 4", 90m, "WEIGHT-PLATES-004", "weight-plates-product-4", 40, null, null },
                    { 20L, "Brand5", 7L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality weight plates item number 5", null, null, true, "Weight Plates Product 5", 100m, "WEIGHT-PLATES-005", "weight-plates-product-5", 45, null, null },
                    { 21L, "Brand1", 8L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality treadmills item number 1", null, null, true, "Treadmills Product 1", 60m, "TREADMILLS-001", "treadmills-product-1", 25, null, null },
                    { 22L, "Brand2", 8L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality treadmills item number 2", null, null, true, "Treadmills Product 2", 70m, "TREADMILLS-002", "treadmills-product-2", 30, null, null },
                    { 23L, "Brand3", 8L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality treadmills item number 3", null, null, true, "Treadmills Product 3", 80m, "TREADMILLS-003", "treadmills-product-3", 35, null, null },
                    { 24L, "Brand4", 8L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality treadmills item number 4", null, null, true, "Treadmills Product 4", 90m, "TREADMILLS-004", "treadmills-product-4", 40, null, null },
                    { 25L, "Brand5", 8L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality treadmills item number 5", null, null, true, "Treadmills Product 5", 100m, "TREADMILLS-005", "treadmills-product-5", 45, null, null },
                    { 26L, "Brand1", 9L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality exercise bikes item number 1", null, null, true, "Exercise Bikes Product 1", 60m, "EXERCISE-BIKES-001", "exercise-bikes-product-1", 25, null, null },
                    { 27L, "Brand2", 9L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality exercise bikes item number 2", null, null, true, "Exercise Bikes Product 2", 70m, "EXERCISE-BIKES-002", "exercise-bikes-product-2", 30, null, null },
                    { 28L, "Brand3", 9L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality exercise bikes item number 3", null, null, true, "Exercise Bikes Product 3", 80m, "EXERCISE-BIKES-003", "exercise-bikes-product-3", 35, null, null },
                    { 29L, "Brand4", 9L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality exercise bikes item number 4", null, null, true, "Exercise Bikes Product 4", 90m, "EXERCISE-BIKES-004", "exercise-bikes-product-4", 40, null, null },
                    { 30L, "Brand5", 9L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality exercise bikes item number 5", null, null, true, "Exercise Bikes Product 5", 100m, "EXERCISE-BIKES-005", "exercise-bikes-product-5", 45, null, null },
                    { 31L, "Brand1", 10L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality ellipticals item number 1", null, null, true, "Ellipticals Product 1", 60m, "ELLIPTICALS-001", "ellipticals-product-1", 25, null, null },
                    { 32L, "Brand2", 10L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality ellipticals item number 2", null, null, true, "Ellipticals Product 2", 70m, "ELLIPTICALS-002", "ellipticals-product-2", 30, null, null },
                    { 33L, "Brand3", 10L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality ellipticals item number 3", null, null, true, "Ellipticals Product 3", 80m, "ELLIPTICALS-003", "ellipticals-product-3", 35, null, null },
                    { 34L, "Brand4", 10L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality ellipticals item number 4", null, null, true, "Ellipticals Product 4", 90m, "ELLIPTICALS-004", "ellipticals-product-4", 40, null, null },
                    { 35L, "Brand5", 10L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality ellipticals item number 5", null, null, true, "Ellipticals Product 5", 100m, "ELLIPTICALS-005", "ellipticals-product-5", 45, null, null },
                    { 36L, "Brand1", 11L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality rowing machines item number 1", null, null, true, "Rowing Machines Product 1", 60m, "ROWING-MACHINES-001", "rowing-machines-product-1", 25, null, null },
                    { 37L, "Brand2", 11L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality rowing machines item number 2", null, null, true, "Rowing Machines Product 2", 70m, "ROWING-MACHINES-002", "rowing-machines-product-2", 30, null, null },
                    { 38L, "Brand3", 11L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality rowing machines item number 3", null, null, true, "Rowing Machines Product 3", 80m, "ROWING-MACHINES-003", "rowing-machines-product-3", 35, null, null },
                    { 39L, "Brand4", 11L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality rowing machines item number 4", null, null, true, "Rowing Machines Product 4", 90m, "ROWING-MACHINES-004", "rowing-machines-product-4", 40, null, null },
                    { 40L, "Brand5", 11L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality rowing machines item number 5", null, null, true, "Rowing Machines Product 5", 100m, "ROWING-MACHINES-005", "rowing-machines-product-5", 45, null, null },
                    { 41L, "Brand1", 12L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality yoga mats item number 1", null, null, true, "Yoga Mats Product 1", 60m, "YOGA-MATS-001", "yoga-mats-product-1", 25, null, null },
                    { 42L, "Brand2", 12L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality yoga mats item number 2", null, null, true, "Yoga Mats Product 2", 70m, "YOGA-MATS-002", "yoga-mats-product-2", 30, null, null },
                    { 43L, "Brand3", 12L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality yoga mats item number 3", null, null, true, "Yoga Mats Product 3", 80m, "YOGA-MATS-003", "yoga-mats-product-3", 35, null, null },
                    { 44L, "Brand4", 12L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality yoga mats item number 4", null, null, true, "Yoga Mats Product 4", 90m, "YOGA-MATS-004", "yoga-mats-product-4", 40, null, null },
                    { 45L, "Brand5", 12L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality yoga mats item number 5", null, null, true, "Yoga Mats Product 5", 100m, "YOGA-MATS-005", "yoga-mats-product-5", 45, null, null },
                    { 46L, "Brand1", 13L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality foam rollers item number 1", null, null, true, "Foam Rollers Product 1", 60m, "FOAM-ROLLERS-001", "foam-rollers-product-1", 25, null, null },
                    { 47L, "Brand2", 13L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality foam rollers item number 2", null, null, true, "Foam Rollers Product 2", 70m, "FOAM-ROLLERS-002", "foam-rollers-product-2", 30, null, null },
                    { 48L, "Brand3", 13L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality foam rollers item number 3", null, null, true, "Foam Rollers Product 3", 80m, "FOAM-ROLLERS-003", "foam-rollers-product-3", 35, null, null },
                    { 49L, "Brand4", 13L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality foam rollers item number 4", null, null, true, "Foam Rollers Product 4", 90m, "FOAM-ROLLERS-004", "foam-rollers-product-4", 40, null, null },
                    { 50L, "Brand5", 13L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality foam rollers item number 5", null, null, true, "Foam Rollers Product 5", 100m, "FOAM-ROLLERS-005", "foam-rollers-product-5", 45, null, null },
                    { 51L, "Brand1", 14L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality resistance bands item number 1", null, null, true, "Resistance Bands Product 1", 60m, "RESISTANCE-BANDS-001", "resistance-bands-product-1", 25, null, null },
                    { 52L, "Brand2", 14L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality resistance bands item number 2", null, null, true, "Resistance Bands Product 2", 70m, "RESISTANCE-BANDS-002", "resistance-bands-product-2", 30, null, null },
                    { 53L, "Brand3", 14L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality resistance bands item number 3", null, null, true, "Resistance Bands Product 3", 80m, "RESISTANCE-BANDS-003", "resistance-bands-product-3", 35, null, null },
                    { 54L, "Brand4", 14L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality resistance bands item number 4", null, null, true, "Resistance Bands Product 4", 90m, "RESISTANCE-BANDS-004", "resistance-bands-product-4", 40, null, null },
                    { 55L, "Brand5", 14L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality resistance bands item number 5", null, null, true, "Resistance Bands Product 5", 100m, "RESISTANCE-BANDS-005", "resistance-bands-product-5", 45, null, null },
                    { 56L, "Brand1", 15L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality gloves item number 1", null, null, true, "Gloves Product 1", 60m, "GLOVES-001", "gloves-product-1", 25, null, null },
                    { 57L, "Brand2", 15L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality gloves item number 2", null, null, true, "Gloves Product 2", 70m, "GLOVES-002", "gloves-product-2", 30, null, null },
                    { 58L, "Brand3", 15L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality gloves item number 3", null, null, true, "Gloves Product 3", 80m, "GLOVES-003", "gloves-product-3", 35, null, null },
                    { 59L, "Brand4", 15L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality gloves item number 4", null, null, true, "Gloves Product 4", 90m, "GLOVES-004", "gloves-product-4", 40, null, null },
                    { 60L, "Brand5", 15L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), "High quality gloves item number 5", null, null, true, "Gloves Product 5", 100m, "GLOVES-005", "gloves-product-5", 45, null, null }
                });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "Id", "LastUpdated", "ProductId", "QuantityAvailable", "ReorderLevel", "SupplierId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 1L, 25, 0, 2L },
                    { 2L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 2L, 30, 0, 3L },
                    { 3L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 3L, 35, 0, 1L },
                    { 4L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 4L, 40, 0, 2L },
                    { 5L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 5L, 45, 0, 3L },
                    { 6L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 6L, 25, 0, 1L },
                    { 7L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 7L, 30, 0, 2L },
                    { 8L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 8L, 35, 0, 3L },
                    { 9L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 9L, 40, 0, 1L },
                    { 10L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 10L, 45, 0, 2L },
                    { 11L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 11L, 25, 0, 3L },
                    { 12L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 12L, 30, 0, 1L },
                    { 13L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 13L, 35, 0, 2L },
                    { 14L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 14L, 40, 0, 3L },
                    { 15L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 15L, 45, 0, 1L },
                    { 16L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 16L, 25, 0, 2L },
                    { 17L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 17L, 30, 0, 3L },
                    { 18L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 18L, 35, 0, 1L },
                    { 19L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 19L, 40, 0, 2L },
                    { 20L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 20L, 45, 0, 3L },
                    { 21L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 21L, 25, 0, 1L },
                    { 22L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 22L, 30, 0, 2L },
                    { 23L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 23L, 35, 0, 3L },
                    { 24L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 24L, 40, 0, 1L },
                    { 25L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 25L, 45, 0, 2L },
                    { 26L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 26L, 25, 0, 3L },
                    { 27L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 27L, 30, 0, 1L },
                    { 28L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 28L, 35, 0, 2L },
                    { 29L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 29L, 40, 0, 3L },
                    { 30L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 30L, 45, 0, 1L },
                    { 31L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 31L, 25, 0, 2L },
                    { 32L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 32L, 30, 0, 3L },
                    { 33L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 33L, 35, 0, 1L },
                    { 34L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 34L, 40, 0, 2L },
                    { 35L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 35L, 45, 0, 3L },
                    { 36L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 36L, 25, 0, 1L },
                    { 37L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 37L, 30, 0, 2L },
                    { 38L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 38L, 35, 0, 3L },
                    { 39L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 39L, 40, 0, 1L },
                    { 40L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 40L, 45, 0, 2L },
                    { 41L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 41L, 25, 0, 3L },
                    { 42L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 42L, 30, 0, 1L },
                    { 43L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 43L, 35, 0, 2L },
                    { 44L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 44L, 40, 0, 3L },
                    { 45L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 45L, 45, 0, 1L },
                    { 46L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 46L, 25, 0, 2L },
                    { 47L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 47L, 30, 0, 3L },
                    { 48L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 48L, 35, 0, 1L },
                    { 49L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 49L, 40, 0, 2L },
                    { 50L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 50L, 45, 0, 3L },
                    { 51L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 51L, 25, 0, 1L },
                    { 52L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 52L, 30, 0, 2L },
                    { 53L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 53L, 35, 0, 3L },
                    { 54L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 54L, 40, 0, 1L },
                    { 55L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 55L, 45, 0, 2L },
                    { 56L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 56L, 25, 0, 3L },
                    { 57L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 57L, 30, 0, 1L },
                    { 58L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 58L, 35, 0, 2L },
                    { 59L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 59L, 40, 0, 3L },
                    { 60L, new DateTime(2025, 9, 29, 22, 0, 0, 0, DateTimeKind.Utc), 60L, 45, 0, 1L }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "AltText", "ImageUrl", "IsPrimary", "ProductId" },
                values: new object[,]
                {
                    { 11L, null, "images/dumbbells-product-1-main.jpg", true, 1L },
                    { 12L, null, "images/dumbbells-product-1-2.jpg", false, 1L },
                    { 13L, null, "images/dumbbells-product-1-3.jpg", false, 1L },
                    { 21L, null, "images/dumbbells-product-2-main.jpg", true, 2L },
                    { 22L, null, "images/dumbbells-product-2-2.jpg", false, 2L },
                    { 23L, null, "images/dumbbells-product-2-3.jpg", false, 2L },
                    { 31L, null, "images/dumbbells-product-3-main.jpg", true, 3L },
                    { 32L, null, "images/dumbbells-product-3-2.jpg", false, 3L },
                    { 33L, null, "images/dumbbells-product-3-3.jpg", false, 3L },
                    { 41L, null, "images/dumbbells-product-4-main.jpg", true, 4L },
                    { 42L, null, "images/dumbbells-product-4-2.jpg", false, 4L },
                    { 43L, null, "images/dumbbells-product-4-3.jpg", false, 4L },
                    { 51L, null, "images/dumbbells-product-5-main.jpg", true, 5L },
                    { 52L, null, "images/dumbbells-product-5-2.jpg", false, 5L },
                    { 53L, null, "images/dumbbells-product-5-3.jpg", false, 5L },
                    { 61L, null, "images/kettlebells-product-1-main.jpg", true, 6L },
                    { 62L, null, "images/kettlebells-product-1-2.jpg", false, 6L },
                    { 63L, null, "images/kettlebells-product-1-3.jpg", false, 6L },
                    { 71L, null, "images/kettlebells-product-2-main.jpg", true, 7L },
                    { 72L, null, "images/kettlebells-product-2-2.jpg", false, 7L },
                    { 73L, null, "images/kettlebells-product-2-3.jpg", false, 7L },
                    { 81L, null, "images/kettlebells-product-3-main.jpg", true, 8L },
                    { 82L, null, "images/kettlebells-product-3-2.jpg", false, 8L },
                    { 83L, null, "images/kettlebells-product-3-3.jpg", false, 8L },
                    { 91L, null, "images/kettlebells-product-4-main.jpg", true, 9L },
                    { 92L, null, "images/kettlebells-product-4-2.jpg", false, 9L },
                    { 93L, null, "images/kettlebells-product-4-3.jpg", false, 9L },
                    { 101L, null, "images/kettlebells-product-5-main.jpg", true, 10L },
                    { 102L, null, "images/kettlebells-product-5-2.jpg", false, 10L },
                    { 103L, null, "images/kettlebells-product-5-3.jpg", false, 10L },
                    { 111L, null, "images/barbells-product-1-main.jpg", true, 11L },
                    { 112L, null, "images/barbells-product-1-2.jpg", false, 11L },
                    { 113L, null, "images/barbells-product-1-3.jpg", false, 11L },
                    { 121L, null, "images/barbells-product-2-main.jpg", true, 12L },
                    { 122L, null, "images/barbells-product-2-2.jpg", false, 12L },
                    { 123L, null, "images/barbells-product-2-3.jpg", false, 12L },
                    { 131L, null, "images/barbells-product-3-main.jpg", true, 13L },
                    { 132L, null, "images/barbells-product-3-2.jpg", false, 13L },
                    { 133L, null, "images/barbells-product-3-3.jpg", false, 13L },
                    { 141L, null, "images/barbells-product-4-main.jpg", true, 14L },
                    { 142L, null, "images/barbells-product-4-2.jpg", false, 14L },
                    { 143L, null, "images/barbells-product-4-3.jpg", false, 14L },
                    { 151L, null, "images/barbells-product-5-main.jpg", true, 15L },
                    { 152L, null, "images/barbells-product-5-2.jpg", false, 15L },
                    { 153L, null, "images/barbells-product-5-3.jpg", false, 15L },
                    { 161L, null, "images/weight-plates-product-1-main.jpg", true, 16L },
                    { 162L, null, "images/weight-plates-product-1-2.jpg", false, 16L },
                    { 163L, null, "images/weight-plates-product-1-3.jpg", false, 16L },
                    { 171L, null, "images/weight-plates-product-2-main.jpg", true, 17L },
                    { 172L, null, "images/weight-plates-product-2-2.jpg", false, 17L },
                    { 173L, null, "images/weight-plates-product-2-3.jpg", false, 17L },
                    { 181L, null, "images/weight-plates-product-3-main.jpg", true, 18L },
                    { 182L, null, "images/weight-plates-product-3-2.jpg", false, 18L },
                    { 183L, null, "images/weight-plates-product-3-3.jpg", false, 18L },
                    { 191L, null, "images/weight-plates-product-4-main.jpg", true, 19L },
                    { 192L, null, "images/weight-plates-product-4-2.jpg", false, 19L },
                    { 193L, null, "images/weight-plates-product-4-3.jpg", false, 19L },
                    { 201L, null, "images/weight-plates-product-5-main.jpg", true, 20L },
                    { 202L, null, "images/weight-plates-product-5-2.jpg", false, 20L },
                    { 203L, null, "images/weight-plates-product-5-3.jpg", false, 20L },
                    { 211L, null, "images/treadmills-product-1-main.jpg", true, 21L },
                    { 212L, null, "images/treadmills-product-1-2.jpg", false, 21L },
                    { 213L, null, "images/treadmills-product-1-3.jpg", false, 21L },
                    { 221L, null, "images/treadmills-product-2-main.jpg", true, 22L },
                    { 222L, null, "images/treadmills-product-2-2.jpg", false, 22L },
                    { 223L, null, "images/treadmills-product-2-3.jpg", false, 22L },
                    { 231L, null, "images/treadmills-product-3-main.jpg", true, 23L },
                    { 232L, null, "images/treadmills-product-3-2.jpg", false, 23L },
                    { 233L, null, "images/treadmills-product-3-3.jpg", false, 23L },
                    { 241L, null, "images/treadmills-product-4-main.jpg", true, 24L },
                    { 242L, null, "images/treadmills-product-4-2.jpg", false, 24L },
                    { 243L, null, "images/treadmills-product-4-3.jpg", false, 24L },
                    { 251L, null, "images/treadmills-product-5-main.jpg", true, 25L },
                    { 252L, null, "images/treadmills-product-5-2.jpg", false, 25L },
                    { 253L, null, "images/treadmills-product-5-3.jpg", false, 25L },
                    { 261L, null, "images/exercise-bikes-product-1-main.jpg", true, 26L },
                    { 262L, null, "images/exercise-bikes-product-1-2.jpg", false, 26L },
                    { 263L, null, "images/exercise-bikes-product-1-3.jpg", false, 26L },
                    { 271L, null, "images/exercise-bikes-product-2-main.jpg", true, 27L },
                    { 272L, null, "images/exercise-bikes-product-2-2.jpg", false, 27L },
                    { 273L, null, "images/exercise-bikes-product-2-3.jpg", false, 27L },
                    { 281L, null, "images/exercise-bikes-product-3-main.jpg", true, 28L },
                    { 282L, null, "images/exercise-bikes-product-3-2.jpg", false, 28L },
                    { 283L, null, "images/exercise-bikes-product-3-3.jpg", false, 28L },
                    { 291L, null, "images/exercise-bikes-product-4-main.jpg", true, 29L },
                    { 292L, null, "images/exercise-bikes-product-4-2.jpg", false, 29L },
                    { 293L, null, "images/exercise-bikes-product-4-3.jpg", false, 29L },
                    { 301L, null, "images/exercise-bikes-product-5-main.jpg", true, 30L },
                    { 302L, null, "images/exercise-bikes-product-5-2.jpg", false, 30L },
                    { 303L, null, "images/exercise-bikes-product-5-3.jpg", false, 30L },
                    { 311L, null, "images/ellipticals-product-1-main.jpg", true, 31L },
                    { 312L, null, "images/ellipticals-product-1-2.jpg", false, 31L },
                    { 313L, null, "images/ellipticals-product-1-3.jpg", false, 31L },
                    { 321L, null, "images/ellipticals-product-2-main.jpg", true, 32L },
                    { 322L, null, "images/ellipticals-product-2-2.jpg", false, 32L },
                    { 323L, null, "images/ellipticals-product-2-3.jpg", false, 32L },
                    { 331L, null, "images/ellipticals-product-3-main.jpg", true, 33L },
                    { 332L, null, "images/ellipticals-product-3-2.jpg", false, 33L },
                    { 333L, null, "images/ellipticals-product-3-3.jpg", false, 33L },
                    { 341L, null, "images/ellipticals-product-4-main.jpg", true, 34L },
                    { 342L, null, "images/ellipticals-product-4-2.jpg", false, 34L },
                    { 343L, null, "images/ellipticals-product-4-3.jpg", false, 34L },
                    { 351L, null, "images/ellipticals-product-5-main.jpg", true, 35L },
                    { 352L, null, "images/ellipticals-product-5-2.jpg", false, 35L },
                    { 353L, null, "images/ellipticals-product-5-3.jpg", false, 35L },
                    { 361L, null, "images/rowing-machines-product-1-main.jpg", true, 36L },
                    { 362L, null, "images/rowing-machines-product-1-2.jpg", false, 36L },
                    { 363L, null, "images/rowing-machines-product-1-3.jpg", false, 36L },
                    { 371L, null, "images/rowing-machines-product-2-main.jpg", true, 37L },
                    { 372L, null, "images/rowing-machines-product-2-2.jpg", false, 37L },
                    { 373L, null, "images/rowing-machines-product-2-3.jpg", false, 37L },
                    { 381L, null, "images/rowing-machines-product-3-main.jpg", true, 38L },
                    { 382L, null, "images/rowing-machines-product-3-2.jpg", false, 38L },
                    { 383L, null, "images/rowing-machines-product-3-3.jpg", false, 38L },
                    { 391L, null, "images/rowing-machines-product-4-main.jpg", true, 39L },
                    { 392L, null, "images/rowing-machines-product-4-2.jpg", false, 39L },
                    { 393L, null, "images/rowing-machines-product-4-3.jpg", false, 39L },
                    { 401L, null, "images/rowing-machines-product-5-main.jpg", true, 40L },
                    { 402L, null, "images/rowing-machines-product-5-2.jpg", false, 40L },
                    { 403L, null, "images/rowing-machines-product-5-3.jpg", false, 40L },
                    { 411L, null, "images/yoga-mats-product-1-main.jpg", true, 41L },
                    { 412L, null, "images/yoga-mats-product-1-2.jpg", false, 41L },
                    { 413L, null, "images/yoga-mats-product-1-3.jpg", false, 41L },
                    { 421L, null, "images/yoga-mats-product-2-main.jpg", true, 42L },
                    { 422L, null, "images/yoga-mats-product-2-2.jpg", false, 42L },
                    { 423L, null, "images/yoga-mats-product-2-3.jpg", false, 42L },
                    { 431L, null, "images/yoga-mats-product-3-main.jpg", true, 43L },
                    { 432L, null, "images/yoga-mats-product-3-2.jpg", false, 43L },
                    { 433L, null, "images/yoga-mats-product-3-3.jpg", false, 43L },
                    { 441L, null, "images/yoga-mats-product-4-main.jpg", true, 44L },
                    { 442L, null, "images/yoga-mats-product-4-2.jpg", false, 44L },
                    { 443L, null, "images/yoga-mats-product-4-3.jpg", false, 44L },
                    { 451L, null, "images/yoga-mats-product-5-main.jpg", true, 45L },
                    { 452L, null, "images/yoga-mats-product-5-2.jpg", false, 45L },
                    { 453L, null, "images/yoga-mats-product-5-3.jpg", false, 45L },
                    { 461L, null, "images/foam-rollers-product-1-main.jpg", true, 46L },
                    { 462L, null, "images/foam-rollers-product-1-2.jpg", false, 46L },
                    { 463L, null, "images/foam-rollers-product-1-3.jpg", false, 46L },
                    { 471L, null, "images/foam-rollers-product-2-main.jpg", true, 47L },
                    { 472L, null, "images/foam-rollers-product-2-2.jpg", false, 47L },
                    { 473L, null, "images/foam-rollers-product-2-3.jpg", false, 47L },
                    { 481L, null, "images/foam-rollers-product-3-main.jpg", true, 48L },
                    { 482L, null, "images/foam-rollers-product-3-2.jpg", false, 48L },
                    { 483L, null, "images/foam-rollers-product-3-3.jpg", false, 48L },
                    { 491L, null, "images/foam-rollers-product-4-main.jpg", true, 49L },
                    { 492L, null, "images/foam-rollers-product-4-2.jpg", false, 49L },
                    { 493L, null, "images/foam-rollers-product-4-3.jpg", false, 49L },
                    { 501L, null, "images/foam-rollers-product-5-main.jpg", true, 50L },
                    { 502L, null, "images/foam-rollers-product-5-2.jpg", false, 50L },
                    { 503L, null, "images/foam-rollers-product-5-3.jpg", false, 50L },
                    { 511L, null, "images/resistance-bands-product-1-main.jpg", true, 51L },
                    { 512L, null, "images/resistance-bands-product-1-2.jpg", false, 51L },
                    { 513L, null, "images/resistance-bands-product-1-3.jpg", false, 51L },
                    { 521L, null, "images/resistance-bands-product-2-main.jpg", true, 52L },
                    { 522L, null, "images/resistance-bands-product-2-2.jpg", false, 52L },
                    { 523L, null, "images/resistance-bands-product-2-3.jpg", false, 52L },
                    { 531L, null, "images/resistance-bands-product-3-main.jpg", true, 53L },
                    { 532L, null, "images/resistance-bands-product-3-2.jpg", false, 53L },
                    { 533L, null, "images/resistance-bands-product-3-3.jpg", false, 53L },
                    { 541L, null, "images/resistance-bands-product-4-main.jpg", true, 54L },
                    { 542L, null, "images/resistance-bands-product-4-2.jpg", false, 54L },
                    { 543L, null, "images/resistance-bands-product-4-3.jpg", false, 54L },
                    { 551L, null, "images/resistance-bands-product-5-main.jpg", true, 55L },
                    { 552L, null, "images/resistance-bands-product-5-2.jpg", false, 55L },
                    { 553L, null, "images/resistance-bands-product-5-3.jpg", false, 55L },
                    { 561L, null, "images/gloves-product-1-main.jpg", true, 56L },
                    { 562L, null, "images/gloves-product-1-2.jpg", false, 56L },
                    { 563L, null, "images/gloves-product-1-3.jpg", false, 56L },
                    { 571L, null, "images/gloves-product-2-main.jpg", true, 57L },
                    { 572L, null, "images/gloves-product-2-2.jpg", false, 57L },
                    { 573L, null, "images/gloves-product-2-3.jpg", false, 57L },
                    { 581L, null, "images/gloves-product-3-main.jpg", true, 58L },
                    { 582L, null, "images/gloves-product-3-2.jpg", false, 58L },
                    { 583L, null, "images/gloves-product-3-3.jpg", false, 58L },
                    { 591L, null, "images/gloves-product-4-main.jpg", true, 59L },
                    { 592L, null, "images/gloves-product-4-2.jpg", false, 59L },
                    { 593L, null, "images/gloves-product-4-3.jpg", false, 59L },
                    { 601L, null, "images/gloves-product-5-main.jpg", true, 60L },
                    { 602L, null, "images/gloves-product-5-2.jpg", false, 60L },
                    { 603L, null, "images/gloves-product-5-3.jpg", false, 60L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_SupplierId",
                table: "Inventories",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillingAddressId",
                table: "Orders",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingAddressId",
                table: "Orders",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_ProductId",
                table: "WishlistItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
