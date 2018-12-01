using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.PIM.Application.Infrastructure.Migrations
{
    public partial class drop_variant_fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variants_Price_PriceId",
                table: "Variants");

            migrationBuilder.DropForeignKey(
                name: "FK_Variants_Products_ProductId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_PriceId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_ProductId",
                table: "Variants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Variants_PriceId",
                table: "Variants",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_ProductId",
                table: "Variants",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Price_PriceId",
                table: "Variants",
                column: "PriceId",
                principalTable: "Price",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Products_ProductId",
                table: "Variants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
