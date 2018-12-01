using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.Ids.Migrations.HarveyIdsDb
{
    public partial class ChangeColumnShortLinkToValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullUrl",
                table: "ShortLinks",
                newName: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ShortLinks",
                newName: "FullUrl");
        }
    }
}
