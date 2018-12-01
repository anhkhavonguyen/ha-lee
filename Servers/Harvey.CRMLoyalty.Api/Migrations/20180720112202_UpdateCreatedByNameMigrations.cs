using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class UpdateCreatedByNameMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "WalletTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByName",
                table: "WalletTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Staffs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByName",
                table: "Staffs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "PointTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByName",
                table: "PointTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Outlets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByName",
                table: "Outlets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "MembershipTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByName",
                table: "MembershipTransactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "ErrorLogTypies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByName",
                table: "ErrorLogTypies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByName",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "AppSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateByName",
                table: "AppSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "UpdateByName",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "UpdateByName",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "UpdateByName",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "UpdateByName",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "MembershipTransactions");

            migrationBuilder.DropColumn(
                name: "UpdateByName",
                table: "MembershipTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "ErrorLogTypies");

            migrationBuilder.DropColumn(
                name: "UpdateByName",
                table: "ErrorLogTypies");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdateByName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "UpdateByName",
                table: "AppSettings");
        }
    }
}
