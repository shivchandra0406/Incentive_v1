using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIncentivePlanConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncentivePlans_Project_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans");

            migrationBuilder.AlterColumn<string>(
                name: "TieredIncentivePlan_MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TargetType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleBasedIncentivePlan_MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleBasedIncentivePlan_CalculationType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectBasedIncentivePlan_MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CalculationType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwardType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncentivePlans_TeamId",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncentivePlans_Project_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncentivePlans_Teams_TeamId",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                column: "TeamId",
                principalSchema: "IncentiveManagement",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncentivePlans_Project_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans");

            migrationBuilder.DropForeignKey(
                name: "FK_IncentivePlans_Teams_TeamId",
                schema: "IncentiveManagement",
                table: "IncentivePlans");

            migrationBuilder.DropIndex(
                name: "IX_IncentivePlans_TeamId",
                schema: "IncentiveManagement",
                table: "IncentivePlans");

            migrationBuilder.AlterColumn<int>(
                name: "TieredIncentivePlan_MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TargetType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoleBasedIncentivePlan_MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoleBasedIncentivePlan_CalculationType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectBasedIncentivePlan_MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CalculationType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AwardType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IncentivePlans_Project_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
