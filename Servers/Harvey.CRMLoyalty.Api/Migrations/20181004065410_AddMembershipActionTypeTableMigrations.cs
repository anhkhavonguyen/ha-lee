using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class AddMembershipActionTypeTableMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MembershipActionTypeId",
                table: "MembershipTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MembershipActionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipActionTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MembershipActionTypes",
                columns: new[] { "Id", "TypeName" },
                values: new object[,]
                {
                    { 0, "Migration" },
                    { 1, "New" },
                    { 2, "Upgraded" },
                    { 3, "Renew" },
                    { 4, "Extend" },
                    { 5, "Downgrade" },
                    { 6, "Void" },
                    { 7, "ChangeExpiry" },
                    { 8, "Comment" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTransactions_MembershipActionTypeId",
                table: "MembershipTransactions",
                column: "MembershipActionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipTransactions_MembershipActionTypes_MembershipActi~",
                table: "MembershipTransactions",
                column: "MembershipActionTypeId",
                principalTable: "MembershipActionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipTransactions_MembershipActionTypes_MembershipActi~",
                table: "MembershipTransactions");

            migrationBuilder.DropTable(
                name: "MembershipActionTypes");

            migrationBuilder.DropIndex(
                name: "IX_MembershipTransactions_MembershipActionTypeId",
                table: "MembershipTransactions");

            migrationBuilder.DropColumn(
                name: "MembershipActionTypeId",
                table: "MembershipTransactions");
        }
    }
}
