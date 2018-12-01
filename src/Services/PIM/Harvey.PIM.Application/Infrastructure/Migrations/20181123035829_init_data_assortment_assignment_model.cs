using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.PIM.Application.Infrastructure.Migrations
{
    public partial class init_data_assortment_assignment_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssortmentAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AssortmentId = table.Column<Guid>(nullable: false),
                    ReferenceId = table.Column<Guid>(nullable: false),
                    EntityType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssortmentAssignments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssortmentAssignments_AssortmentId_ReferenceId_EntityType",
                table: "AssortmentAssignments",
                columns: new[] { "AssortmentId", "ReferenceId", "EntityType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssortmentAssignments");
        }
    }
}
