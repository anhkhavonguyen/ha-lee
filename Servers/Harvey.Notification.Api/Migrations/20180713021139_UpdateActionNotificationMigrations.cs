using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.Notification.Api.Migrations
{
    public partial class UpdateActionNotificationMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Action",
                table: "Notifications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Notifications");
        }
    }
}
