using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectEntityWithRealEstateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                schema: "IncentiveManagement",
                table: "Projects",
                newName: "DateListed");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                schema: "IncentiveManagement",
                table: "Projects",
                newName: "ListingExpiryDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalValue",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "AgentContact",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AgentName",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Amenities",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Area",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Bathrooms",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bedrooms",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImagesMedia",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "For Sale");

            migrationBuilder.AddColumn<int>(
                name: "YearBuilt",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentContact",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AgentName",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Amenities",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Area",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Bathrooms",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Bedrooms",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ImagesMedia",
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

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "YearBuilt",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ListingExpiryDate",
                schema: "IncentiveManagement",
                table: "Projects",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "DateListed",
                schema: "IncentiveManagement",
                table: "Projects",
                newName: "StartDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalValue",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "IncentiveManagement",
                table: "Projects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);
        }
    }
}
