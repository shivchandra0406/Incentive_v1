using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnAdditionalIncentiveOnExceeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalIncentiveOnExceeding",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "numeric(5,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalIncentiveOnExceeding",
                schema: "IncentiveManagement",
                table: "IncentivePlans");
        }
    }
}
