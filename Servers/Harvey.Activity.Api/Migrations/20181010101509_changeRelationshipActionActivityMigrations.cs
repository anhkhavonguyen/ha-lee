using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.Activity.Api.Migrations
{
    public partial class changeRelationshipActionActivityMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionTypies_Activities_ActionActivityId",
                table: "ActionTypies");

            migrationBuilder.DropForeignKey(
                name: "FK_AreaActivities_Activities_ActionActivityId",
                table: "AreaActivities");

            migrationBuilder.DropIndex(
                name: "IX_AreaActivities_ActionActivityId",
                table: "AreaActivities");

            migrationBuilder.DropIndex(
                name: "IX_ActionTypies_ActionActivityId",
                table: "ActionTypies");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "AreaActivities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Activities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "ActionTypies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActionAreaId",
                table: "Activities",
                column: "ActionAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActionTypeId",
                table: "Activities",
                column: "ActionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AreaActivities_ActionAreaId",
                table: "Activities",
                column: "ActionAreaId",
                principalTable: "AreaActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActionTypies_ActionTypeId",
                table: "Activities",
                column: "ActionTypeId",
                principalTable: "ActionTypies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AreaActivities_ActionAreaId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActionTypies_ActionTypeId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ActionAreaId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ActionTypeId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "AreaActivities");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "ActionTypies");

            migrationBuilder.CreateIndex(
                name: "IX_AreaActivities_ActionActivityId",
                table: "AreaActivities",
                column: "ActionActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionTypies_ActionActivityId",
                table: "ActionTypies",
                column: "ActionActivityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTypies_Activities_ActionActivityId",
                table: "ActionTypies",
                column: "ActionActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AreaActivities_Activities_ActionActivityId",
                table: "AreaActivities",
                column: "ActionActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
