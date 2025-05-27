using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDealfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_IncentiveRules_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_IncentiveEarnings_IncentiveRules_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropIndex(
                name: "IX_Deals_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "RoleBasedIncentivePlan_IncentiveAfterExceedingTarget",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                newName: "RoleBasedIncentivePlan_IsCumulative");

            migrationBuilder.RenameColumn(
                name: "IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                newName: "IncentivePlanId");

            migrationBuilder.RenameIndex(
                name: "IX_IncentiveEarnings_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                newName: "IX_IncentiveEarnings_IncentivePlanId");

            migrationBuilder.Sql(@"
                ALTER TABLE ""IncentiveManagement"".""IncentiveEarnings""
                ALTER COLUMN ""UserId"" TYPE uuid
                USING ""UserId""::uuid;
            ");


            migrationBuilder.Sql(@"
                ALTER TABLE ""IncentiveManagement"".""Deals""
                ALTER COLUMN ""UserId"" TYPE uuid
                USING ""UserId""::uuid;
            ");


            migrationBuilder.Sql(@"
                ALTER TABLE ""IncentiveManagement"".""Deals""
                ALTER COLUMN ""ClosedByUserId"" TYPE uuid
                USING ""ClosedByUserId""::uuid;
            ");


            migrationBuilder.AddColumn<Guid>(
                name: "IncentivePlanId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Deals_IncentivePlanId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "IncentivePlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_IncentivePlans_IncentivePlanId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "IncentivePlanId",
                principalSchema: "IncentiveManagement",
                principalTable: "IncentivePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncentiveEarnings_IncentivePlans_IncentivePlanId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                column: "IncentivePlanId",
                principalSchema: "IncentiveManagement",
                principalTable: "IncentivePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_IncentivePlans_IncentivePlanId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_IncentiveEarnings_IncentivePlans_IncentivePlanId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings");

            migrationBuilder.DropIndex(
                name: "IX_Deals_IncentivePlanId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "IncentivePlanId",
                schema: "IncentiveManagement",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "RoleBasedIncentivePlan_IsCumulative",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                newName: "RoleBasedIncentivePlan_IncentiveAfterExceedingTarget");

            migrationBuilder.RenameColumn(
                name: "IncentivePlanId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                newName: "IncentiveRuleId");

            migrationBuilder.RenameIndex(
                name: "IX_IncentiveEarnings_IncentivePlanId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                newName: "IX_IncentiveEarnings_IncentiveRuleId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "ClosedByUserId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "IncentiveRuleId");

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
                name: "FK_IncentiveEarnings_IncentiveRules_IncentiveRuleId",
                schema: "IncentiveManagement",
                table: "IncentiveEarnings",
                column: "IncentiveRuleId",
                principalSchema: "IncentiveManagement",
                principalTable: "IncentiveRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
