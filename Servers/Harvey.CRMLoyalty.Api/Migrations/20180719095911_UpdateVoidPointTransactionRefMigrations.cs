using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class UpdateVoidPointTransactionRefMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WalletTransactionReferenceId",
                table: "WalletTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PointTransactionReferenceId",
                table: "PointTransactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletTransactionReferenceId",
                table: "WalletTransactions",
                column: "WalletTransactionReferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_PointTransactionReferenceId",
                table: "PointTransactions",
                column: "PointTransactionReferenceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransactions_PointTransactions_PointTransactionReferen~",
                table: "PointTransactions",
                column: "PointTransactionReferenceId",
                principalTable: "PointTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_WalletTransactions_WalletTransactionRefe~",
                table: "WalletTransactions",
                column: "WalletTransactionReferenceId",
                principalTable: "WalletTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointTransactions_PointTransactions_PointTransactionReferen~",
                table: "PointTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_WalletTransactions_WalletTransactionRefe~",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_WalletTransactionReferenceId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PointTransactions_PointTransactionReferenceId",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "WalletTransactionReferenceId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "PointTransactionReferenceId",
                table: "PointTransactions");
        }
    }
}
