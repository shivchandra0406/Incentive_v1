using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetBasedIncentivePlanConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TargetBasedIncentivePlan_TargetType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TargetBasedIncentivePlan_MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TargetBasedIncentivePlan_CalculationType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TargetBasedIncentivePlan_TargetType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TargetBasedIncentivePlan_MetricType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TargetBasedIncentivePlan_CalculationType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
