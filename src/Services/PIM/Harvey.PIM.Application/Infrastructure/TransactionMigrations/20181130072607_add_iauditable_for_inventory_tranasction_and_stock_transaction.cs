using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.PIM.Application.Infrastructure.TransactionMigrations
{
    public partial class add_iauditable_for_inventory_tranasction_and_stock_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GIWDocumentItems_Variant_VariantId",
                table: "GIWDocumentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_Location_FromLocationId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_Location_ToLocationId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Location_FromLocationId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Location_ToLocationId",
                table: "StockTransactions");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Variant");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_FromLocationId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_ToLocationId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransactions_FromLocationId",
                table: "InventoryTransactions");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransactions_ToLocationId",
                table: "InventoryTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GIWDocumentItems_VariantId",
                table: "GIWDocumentItems");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "StockTransactions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StockTransactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "StockTransactions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "StockTransactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "InventoryTransactions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "InventoryTransactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "InventoryTransactions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "InventoryTransactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "InventoryTransactions");

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Variant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    PriceId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variant", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_FromLocationId",
                table: "StockTransactions",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ToLocationId",
                table: "StockTransactions",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_FromLocationId",
                table: "InventoryTransactions",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ToLocationId",
                table: "InventoryTransactions",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GIWDocumentItems_VariantId",
                table: "GIWDocumentItems",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_GIWDocumentItems_Variant_VariantId",
                table: "GIWDocumentItems",
                column: "VariantId",
                principalTable: "Variant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Location_FromLocationId",
                table: "InventoryTransactions",
                column: "FromLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Location_ToLocationId",
                table: "InventoryTransactions",
                column: "ToLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Location_FromLocationId",
                table: "StockTransactions",
                column: "FromLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Location_ToLocationId",
                table: "StockTransactions",
                column: "ToLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
