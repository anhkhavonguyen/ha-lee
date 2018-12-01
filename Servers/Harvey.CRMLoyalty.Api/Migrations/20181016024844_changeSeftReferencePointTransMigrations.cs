using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class changeSeftReferencePointTransMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PointTransactions_PointTransactionReferenceId",
                table: "PointTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_PointTransactionReferenceId",
                table: "PointTransactions",
                column: "PointTransactionReferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PointTransactions_PointTransactionReferenceId",
                table: "PointTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_PointTransactionReferenceId",
                table: "PointTransactions",
                column: "PointTransactionReferenceId",
                unique: true);
        }
    }
}
