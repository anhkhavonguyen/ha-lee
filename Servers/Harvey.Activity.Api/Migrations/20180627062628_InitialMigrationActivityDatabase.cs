using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.Activity.Api.Migrations
{
    public partial class InitialMigrationActivityDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    ActionTypeId = table.Column<string>(nullable: true),
                    ActionAreaId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogEntries",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ErrorTypeId = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    Source = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogTypies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    TypeName = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogTypies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionTypies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ActionActivityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTypies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionTypies_Activities_ActionActivityId",
                        column: x => x.ActionActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AreaActivities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    AreaPath = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ActionActivityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaActivities_Activities_ActionActivityId",
                        column: x => x.ActionActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionTypies_ActionActivityId",
                table: "ActionTypies",
                column: "ActionActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AreaActivities_ActionActivityId",
                table: "AreaActivities",
                column: "ActionActivityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionTypies");

            migrationBuilder.DropTable(
                name: "AreaActivities");

            migrationBuilder.DropTable(
                name: "ErrorLogEntries");

            migrationBuilder.DropTable(
                name: "ErrorLogTypies");

            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
