using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class UpdateIpAdressForTransactionMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrowserName",
                table: "WalletTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "WalletTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "WalletTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrowserName",
                table: "PointTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "PointTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "PointTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrowserName",
                table: "MembershipTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "MembershipTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "MembershipTransactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserName",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "BrowserName",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "BrowserName",
                table: "MembershipTransactions");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "MembershipTransactions");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "MembershipTransactions");
        }
    }
}
