using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.PIM.Application.Infrastructure.TransactionMigrations
{
    public partial class add_db_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GIWDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GIWDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Variant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    PriceId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionTypeId = table.Column<Guid>(nullable: false),
                    StockTypeId = table.Column<Guid>(nullable: false),
                    VariantId = table.Column<Guid>(nullable: false),
                    FromLocationId = table.Column<Guid>(nullable: false),
                    ToLocationId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Balance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Location_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactions_StockTypes_StockTypeId",
                        column: x => x.StockTypeId,
                        principalTable: "StockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Location_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionTypeId = table.Column<Guid>(nullable: false),
                    GIWDocumentId = table.Column<Guid>(nullable: false),
                    FromLocationId = table.Column<Guid>(nullable: false),
                    ToLocationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Location_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_GIWDocuments_GIWDocumentId",
                        column: x => x.GIWDocumentId,
                        principalTable: "GIWDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Location_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GIWDocumentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GIWDocumentId = table.Column<Guid>(nullable: false),
                    VariantId = table.Column<Guid>(nullable: false),
                    StockTypeId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GIWDocumentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GIWDocumentItems_GIWDocuments_GIWDocumentId",
                        column: x => x.GIWDocumentId,
                        principalTable: "GIWDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GIWDocumentItems_StockTypes_StockTypeId",
                        column: x => x.StockTypeId,
                        principalTable: "StockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GIWDocumentItems_Variant_VariantId",
                        column: x => x.VariantId,
                        principalTable: "Variant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GIWDocumentItems_GIWDocumentId",
                table: "GIWDocumentItems",
                column: "GIWDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_GIWDocumentItems_StockTypeId",
                table: "GIWDocumentItems",
                column: "StockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GIWDocumentItems_VariantId",
                table: "GIWDocumentItems",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_GIWDocuments_Name",
                table: "GIWDocuments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_FromLocationId",
                table: "InventoryTransactions",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_GIWDocumentId",
                table: "InventoryTransactions",
                column: "GIWDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ToLocationId",
                table: "InventoryTransactions",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_TransactionTypeId",
                table: "InventoryTransactions",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_FromLocationId",
                table: "StockTransactions",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_StockTypeId",
                table: "StockTransactions",
                column: "StockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ToLocationId",
                table: "StockTransactions",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTypes_Code",
                table: "StockTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_Code",
                table: "TransactionTypes",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GIWDocumentItems");

            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "Variant");

            migrationBuilder.DropTable(
                name: "GIWDocuments");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "StockTypes");
        }
    }
}
