using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Harvey.CRMLoyalty.Api.Migrations
{
    public partial class initCRMLoyaltyDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PhoneCountryCode = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    ProfileImage = table.Column<string>(nullable: true),
                    JoinedDate = table.Column<DateTime>(nullable: true),
                    LastUsed = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
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
                name: "MembershipTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outlets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    PhoneCountryCode = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    OutletImage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outlets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PointTransactionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTransactionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    PhoneCountryCode = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    ProfileImage = table.Column<string>(nullable: true),
                    TypeOfStaff = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    StaffId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true),
                    OutletId = table.Column<string>(nullable: true),
                    MembershipTypeId = table.Column<int>(nullable: false),
                    ExpiredDate = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipTransactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipTransactions_MembershipTypes_MembershipTypeId",
                        column: x => x.MembershipTypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembershipTransactions_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipTransactions_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PointTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    StaffId = table.Column<string>(nullable: true),
                    OutletId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true),
                    BalanceDebit = table.Column<decimal>(nullable: false),
                    BalanceCredit = table.Column<decimal>(nullable: false),
                    BalanceTotal = table.Column<decimal>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Debit = table.Column<decimal>(nullable: false),
                    Credit = table.Column<decimal>(nullable: false),
                    ExpiredDate = table.Column<DateTime>(nullable: true),
                    Voided = table.Column<bool>(nullable: false),
                    VoidedById = table.Column<string>(nullable: true),
                    PointTransactionTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointTransactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointTransactions_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointTransactions_PointTransactionType_PointTransactionType~",
                        column: x => x.PointTransactionTypeId,
                        principalTable: "PointTransactionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointTransaction_Staff",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VoidedPointTransaction_Staff",
                        column: x => x.VoidedById,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Staff_Outlets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: true),
                    StaffId = table.Column<string>(nullable: false),
                    OutletId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff_Outlets", x => new { x.StaffId, x.OutletId });
                    table.ForeignKey(
                        name: "FK_Staff_Outlets_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Staff_Outlets_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    StaffId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true),
                    OutletId = table.Column<string>(nullable: true),
                    Balance = table.Column<decimal>(nullable: false),
                    ExpiredDate = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTransactions_CustomerId",
                table: "MembershipTransactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTransactions_MembershipTypeId",
                table: "MembershipTransactions",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTransactions_OutletId",
                table: "MembershipTransactions",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTransactions_StaffId",
                table: "MembershipTransactions",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_CustomerId",
                table: "PointTransactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_OutletId",
                table: "PointTransactions",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_PointTransactionTypeId",
                table: "PointTransactions",
                column: "PointTransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_StaffId",
                table: "PointTransactions",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_VoidedById",
                table: "PointTransactions",
                column: "VoidedById");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Outlets_OutletId",
                table: "Staff_Outlets",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_CustomerId",
                table: "WalletTransactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_OutletId",
                table: "WalletTransactions",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_StaffId",
                table: "WalletTransactions",
                column: "StaffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "ErrorLogEntries");

            migrationBuilder.DropTable(
                name: "ErrorLogTypies");

            migrationBuilder.DropTable(
                name: "MembershipTransactions");

            migrationBuilder.DropTable(
                name: "PointTransactions");

            migrationBuilder.DropTable(
                name: "Staff_Outlets");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "MembershipTypes");

            migrationBuilder.DropTable(
                name: "PointTransactionType");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Outlets");

            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
