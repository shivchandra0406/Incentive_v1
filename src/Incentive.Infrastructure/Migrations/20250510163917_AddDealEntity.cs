using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDealEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppliedTo",
                table: "IncentiveRules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "User");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyType",
                table: "IncentiveRules",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "USD");

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "IncentiveRules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Monthly");

            migrationBuilder.AddColumn<string>(
                name: "IncentiveType",
                table: "IncentiveRules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Percentage");

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                table: "IncentiveRules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "ItemBased");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "IncentiveRules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "IncentiveRules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedTo",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "IncentiveType",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "IncentiveRules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IncentiveRules");
        }
    }
}
