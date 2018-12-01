using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.PIM.Application.Infrastructure.Migrations
{
    public partial class init_data_channelassignment_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    ReferenceId = table.Column<Guid>(nullable: false),
                    EntityType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelAssignments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelAssignments_ChannelId_ReferenceId_EntityType",
                table: "ChannelAssignments",
                columns: new[] { "ChannelId", "ReferenceId", "EntityType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelAssignments");
        }
    }
}
