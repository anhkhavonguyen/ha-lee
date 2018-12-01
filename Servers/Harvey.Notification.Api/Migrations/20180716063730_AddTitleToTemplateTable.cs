using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.Notification.Api.Migrations
{
    public partial class AddTitleToTemplateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Templates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Templates");
        }
    }
}
