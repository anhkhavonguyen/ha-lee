using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class UpdateMembershipTransactionReferenceMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MembershipTransactionReferenceId",
                table: "MembershipTransactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTransactions_MembershipTransactionReferenceId",
                table: "MembershipTransactions",
                column: "MembershipTransactionReferenceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipTransactions_MembershipTransactions_MembershipTra~",
                table: "MembershipTransactions",
                column: "MembershipTransactionReferenceId",
                principalTable: "MembershipTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipTransactions_MembershipTransactions_MembershipTra~",
                table: "MembershipTransactions");

            migrationBuilder.DropIndex(
                name: "IX_MembershipTransactions_MembershipTransactionReferenceId",
                table: "MembershipTransactions");

            migrationBuilder.DropColumn(
                name: "MembershipTransactionReferenceId",
                table: "MembershipTransactions");
        }
    }
}
