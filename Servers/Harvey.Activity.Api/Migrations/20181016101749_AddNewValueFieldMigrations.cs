using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.Activity.Api.Migrations
{
    public partial class AddNewValueFieldMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Activities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Activities");
        }
    }
}
