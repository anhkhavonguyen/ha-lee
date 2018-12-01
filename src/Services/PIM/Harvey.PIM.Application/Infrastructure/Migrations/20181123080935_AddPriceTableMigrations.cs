using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.PIM.Application.Infrastructure.Migrations
{
    public partial class AddPriceTableMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PriceId",
                table: "Variants",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ListPrice = table.Column<float>(nullable: false),
                    StaffPrice = table.Column<float>(nullable: false),
                    MemberPrice = table.Column<float>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Variants_PriceId",
                table: "Variants",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Price_PriceId",
                table: "Variants",
                column: "PriceId",
                principalTable: "Price",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variants_Price_PriceId",
                table: "Variants");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropIndex(
                name: "IX_Variants_PriceId",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Variants");
        }
    }
}
