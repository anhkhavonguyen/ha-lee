using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class UpdateErrorLogSourceMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogTypies");

            migrationBuilder.DropColumn(
                name: "ErrorTypeId",
                table: "ErrorLogEntries");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "ErrorLogEntries",
                newName: "ErrorSourceId");

            migrationBuilder.CreateTable(
                name: "ErrorLogSources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    SourceName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogSources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogEntries_ErrorSourceId",
                table: "ErrorLogEntries",
                column: "ErrorSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLogEntries_ErrorLogSources_ErrorSourceId",
                table: "ErrorLogEntries",
                column: "ErrorSourceId",
                principalTable: "ErrorLogSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLogEntries_ErrorLogSources_ErrorSourceId",
                table: "ErrorLogEntries");

            migrationBuilder.DropTable(
                name: "ErrorLogSources");

            migrationBuilder.DropIndex(
                name: "IX_ErrorLogEntries_ErrorSourceId",
                table: "ErrorLogEntries");

            migrationBuilder.RenameColumn(
                name: "ErrorSourceId",
                table: "ErrorLogEntries",
                newName: "Source");

            migrationBuilder.AddColumn<string>(
                name: "ErrorTypeId",
                table: "ErrorLogEntries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ErrorLogTypies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedByName = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    TypeName = table.Column<int>(nullable: false),
                    UpdateByName = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogTypies", x => x.Id);
                });
        }
    }
}
