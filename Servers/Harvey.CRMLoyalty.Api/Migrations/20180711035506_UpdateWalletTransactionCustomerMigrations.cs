using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class UpdateWalletTransactionCustomerMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_Staffs_StaffId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "WalletTransactions");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "WalletTransactions",
                newName: "Debit");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "WalletTransactions",
                newName: "Credit");

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceCredit",
                table: "WalletTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceDebit",
                table: "WalletTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceTotal",
                table: "WalletTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Voided",
                table: "WalletTransactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VoidedById",
                table: "WalletTransactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_VoidedById",
                table: "WalletTransactions",
                column: "VoidedById");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransaction_Staff",
                table: "WalletTransactions",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_VoidedWalletTransaction_Staff",
                table: "WalletTransactions",
                column: "VoidedById",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransaction_Staff",
                table: "WalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_VoidedWalletTransaction_Staff",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_VoidedById",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "BalanceCredit",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "BalanceDebit",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "BalanceTotal",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "Voided",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "VoidedById",
                table: "WalletTransactions");

            migrationBuilder.RenameColumn(
                name: "Debit",
                table: "WalletTransactions",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Credit",
                table: "WalletTransactions",
                newName: "Balance");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDate",
                table: "WalletTransactions",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_Staffs_StaffId",
                table: "WalletTransactions",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
