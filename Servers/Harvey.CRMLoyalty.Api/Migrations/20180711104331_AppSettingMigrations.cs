using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class AppSettingMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppSettingTypeId",
                table: "AppSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppSettingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettingTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_AppSettingTypeId",
                table: "AppSettings",
                column: "AppSettingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettings_AppSettingTypes_AppSettingTypeId",
                table: "AppSettings",
                column: "AppSettingTypeId",
                principalTable: "AppSettingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSettings_AppSettingTypes_AppSettingTypeId",
                table: "AppSettings");

            migrationBuilder.DropTable(
                name: "AppSettingTypes");

            migrationBuilder.DropIndex(
                name: "IX_AppSettings_AppSettingTypeId",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "AppSettingTypeId",
                table: "AppSettings");
        }
    }
}
