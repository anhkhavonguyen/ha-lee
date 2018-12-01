using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.Ids.Migrations.HarveyIdsDb
{
    public partial class AddPhoneCountryCodeDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneCountryCode",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneCountryCode",
                table: "AspNetUsers");
        }
    }
}
