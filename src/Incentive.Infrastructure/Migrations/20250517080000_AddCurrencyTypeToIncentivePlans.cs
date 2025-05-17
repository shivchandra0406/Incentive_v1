using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <summary>
    /// Migration to add CurrencyType field to IncentivePlanBase
    /// </summary>
    public partial class AddCurrencyTypeToIncentivePlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyType",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                type: "text",
                nullable: false,
                defaultValue: "Rupees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyType",
                schema: "IncentiveManagement",
                table: "IncentivePlans");
        }
    }
}
