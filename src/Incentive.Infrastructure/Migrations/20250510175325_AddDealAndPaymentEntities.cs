using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDealAndPaymentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Projects_ProjectId1",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Salespeople_SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Salespeople_SalespersonId1",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_IncentiveRules_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropIndex(
                name: "IX_Deals_SalespersonId1",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "IncentiveType",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "TargetType",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                newName: "Target");

            migrationBuilder.RenameColumn(
                name: "MinBookingValue",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                newName: "TargetValue");

            migrationBuilder.RenameColumn(
                name: "MaxBookingValue",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                newName: "MinimumSalesThreshold");

            migrationBuilder.RenameColumn(
                name: "Value",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "SalespersonId1",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "ProjectId1",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "IncentiveRuleId");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "Source");

            migrationBuilder.RenameIndex(
                name: "IX_Deals_ProjectId1",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "IX_Deals_IncentiveRuleId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "IncentiveManagement",
                table: "Salespeople",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                schema: "IncentiveManagement",
                table: "Salespeople",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Frequency",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Monthly");

            migrationBuilder.AlterColumn<string>(
                name: "AppliedTo",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "User");

            migrationBuilder.AddColumn<decimal>(
                name: "Commission",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Incentive",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsIncludeSalary",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumIncentiveAmount",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumDealCountThreshold",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetDealCount",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ClosedByUserId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyType",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddress",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealName",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsReferralCommissionPaid",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDueDate",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurringFrequencyMonths",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReferralCommission",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralEmail",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralName",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralPhone",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmount",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxPercentage",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "IncentiveManagement",
                table: "Bookings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                schema: "IncentiveManagement",
                table: "Bookings",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                schema: "IncentiveManagement",
                table: "Bookings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "DealActivities",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DealId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    ActivityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealActivities_Deals_DealId",
                        column: x => x.DealId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DealId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReceivedByUserId = table.Column<string>(type: "text", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Deals_DealId",
                        column: x => x.DealId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealActivities_DealId",
                schema: "IncentiveManagement",
                table: "DealActivities",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DealId",
                schema: "IncentiveManagement",
                table: "Payments",
                column: "DealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_IncentiveRules_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "IncentiveRuleId",
                principalSchema: "IncentiveManagement",
                principalTable: "IncentiveRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_IncentiveRules_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_IncentiveRules_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Salespeople_SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_IncentiveRules_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropTable(
                name: "DealActivities",
                schema: "IncentiveManagement");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "IncentiveManagement");

            migrationBuilder.DropColumn(
                name: "Commission",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "Incentive",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "IsIncludeSalary",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "MaximumIncentiveAmount",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "MinimumDealCountThreshold",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "TargetDealCount",
                schema: "IncentiveManagement",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropColumn(
                name: "ClosedByUserId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "CustomerAddress",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "DealName",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "IsReferralCommissionPaid",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "PaymentDueDate",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "RecurringFrequencyMonths",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "ReferralCommission",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "ReferralEmail",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "ReferralName",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "ReferralPhone",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "TaxPercentage",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "TargetValue",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                newName: "MinBookingValue");

            migrationBuilder.RenameColumn(
                name: "Target",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "MinimumSalesThreshold",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                newName: "MaxBookingValue");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "SalespersonId1");

            migrationBuilder.RenameColumn(
                name: "Source",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "ProjectId1");

            migrationBuilder.RenameIndex(
                name: "IX_Deals_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals",
                newName: "IX_Deals_ProjectId1");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "IncentiveManagement",
                table: "Salespeople",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                schema: "IncentiveManagement",
                table: "Salespeople",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Frequency",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Monthly",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AppliedTo",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyType",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "USD");

            migrationBuilder.AddColumn<string>(
                name: "IncentiveType",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Percentage");

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "ItemBased");

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "IncentiveManagement",
                table: "Bookings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                schema: "IncentiveManagement",
                table: "Bookings",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                schema: "IncentiveManagement",
                table: "Bookings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SalespersonId1",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "SalespersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Projects_ProjectId1",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "ProjectId1",
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
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Salespeople_SalespersonId1",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "SalespersonId1",
                principalSchema: "IncentiveManagement",
                principalTable: "Salespeople",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncentiveRules_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentiveRules",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
