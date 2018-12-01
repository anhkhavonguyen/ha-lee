using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class addFirstOutletToCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstOutlet",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstOutlet",
                table: "Customers");
        }
    }
}
