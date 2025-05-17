using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTargetBasedIncentivePlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetBasedIncentivePlan_SalaryPercentage",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                newName: "Salary");

            migrationBuilder.RenameColumn(
                name: "TargetBasedIncentivePlan_IsCumulative",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                newName: "ProvideAdditionalIncentiveOnExceeding");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                newName: "TargetBasedIncentivePlan_SalaryPercentage");

            migrationBuilder.RenameColumn(
                name: "ProvideAdditionalIncentiveOnExceeding",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                newName: "TargetBasedIncentivePlan_IsCumulative");
        }
    }
}
