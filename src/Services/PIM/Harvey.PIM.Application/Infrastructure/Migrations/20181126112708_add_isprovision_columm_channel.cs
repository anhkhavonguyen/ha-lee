using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.PIM.Application.Infrastructure.Migrations
{
    public partial class add_isprovision_columm_channel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProvision",
                table: "Channels",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProvision",
                table: "Channels");
        }
    }
}
