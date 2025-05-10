using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectSalespersonBookingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Salespeople_SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_IncentiveEarnings_Bookings_BookingId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropForeignKey(
                name: "FK_IncentiveEarnings_Salespeople_SalespersonId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropForeignKey(
                name: "FK_IncentiveRules_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropTable(
                name: "Bookings",
                schema: "IncentiveManagement");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "IncentiveManagement");

            migrationBuilder.DropTable(
                name: "Salespeople",
                schema: "IncentiveManagement");

            migrationBuilder.DropIndex(
                name: "IX_IncentiveRules_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropIndex(
                name: "IX_IncentiveEarnings_BookingId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropIndex(
                name: "IX_Deals_ProjectId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "BookingId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "SalespersonId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                newName: "DealId");

            migrationBuilder.RenameIndex(
                name: "IX_IncentiveEarnings_SalespersonId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                newName: "IX_IncentiveEarnings_DealId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_IncentiveEarnings_Deals_DealId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                column: "DealId",
                principalSchema: "IncentiveManagement",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncentiveEarnings_Deals_DealId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.RenameColumn(
                name: "DealId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                newName: "SalespersonId");

            migrationBuilder.RenameIndex(
                name: "IX_IncentiveEarnings_DealId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                newName: "IX_IncentiveEarnings_SalespersonId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salespeople",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmployeeId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salespeople", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalespersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BookingValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CustomerEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Salespeople_SalespersonId",
                        column: x => x.SalespersonId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Salespeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncentiveRules_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IncentiveEarnings_BookingId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ProjectId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "SalespersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ProjectId",
                schema: "IncentiveManagement",
                table: "Bookings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SalespersonId",
                schema: "IncentiveManagement",
                table: "Bookings",
                column: "SalespersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Salespeople_SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "SalespersonId",
                principalSchema: "IncentiveManagement",
                principalTable: "Salespeople",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncentiveEarnings_Bookings_BookingId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                column: "BookingId",
                principalSchema: "IncentiveManagement",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncentiveEarnings_Salespeople_SalespersonId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                column: "SalespersonId",
                principalSchema: "IncentiveManagement",
                principalTable: "Salespeople",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncentiveRules_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
