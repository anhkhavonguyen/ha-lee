using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class addCustomerCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Outlets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Outlets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCode",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "CustomerCode",
                table: "Customers");
        }
    }
}
