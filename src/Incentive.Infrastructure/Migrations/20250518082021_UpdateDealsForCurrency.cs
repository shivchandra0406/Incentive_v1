using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDealsForCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingExpiryDate",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MLSListingId",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OwnershipDetails",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.Sql(@"
                ALTER TABLE ""IncentiveManagement"".""Deals""
                ALTER COLUMN ""CurrencyType"" TYPE integer
                USING ""CurrencyType""::integer;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ListingExpiryDate",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MLSListingId",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnershipDetails",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyType",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 50);
        }
    }
}
