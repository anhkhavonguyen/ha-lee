using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.Notification.Api.Migrations
{
    public partial class ErrorLogSourceMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLogEntries_ErrorLogTypes_ErrorLogTypeId",
                table: "ErrorLogEntries");

            migrationBuilder.DropTable(
                name: "ErrorLogTypes");

            migrationBuilder.RenameColumn(
                name: "ErrorLogTypeId",
                table: "ErrorLogEntries",
                newName: "ErrorLogSourceId");

            migrationBuilder.RenameIndex(
                name: "IX_ErrorLogEntries_ErrorLogTypeId",
                table: "ErrorLogEntries",
                newName: "IX_ErrorLogEntries_ErrorLogSourceId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLogEntries_ErrorLogSources_ErrorLogSourceId",
                table: "ErrorLogEntries",
                column: "ErrorLogSourceId",
                principalTable: "ErrorLogSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLogEntries_ErrorLogSources_ErrorLogSourceId",
                table: "ErrorLogEntries");

            migrationBuilder.DropTable(
                name: "ErrorLogSources");

            migrationBuilder.RenameColumn(
                name: "ErrorLogSourceId",
                table: "ErrorLogEntries",
                newName: "ErrorLogTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ErrorLogEntries_ErrorLogSourceId",
                table: "ErrorLogEntries",
                newName: "IX_ErrorLogEntries_ErrorLogTypeId");

            migrationBuilder.CreateTable(
                name: "ErrorLogTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    TypeName = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLogEntries_ErrorLogTypes_ErrorLogTypeId",
                table: "ErrorLogEntries",
                column: "ErrorLogTypeId",
                principalTable: "ErrorLogTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
