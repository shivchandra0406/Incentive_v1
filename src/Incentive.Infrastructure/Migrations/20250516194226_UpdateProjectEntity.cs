using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncentivePlans_Project_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                schema: "IncentiveManagement",
                table: "Project");

            migrationBuilder.RenameTable(
                name: "Project",
                schema: "IncentiveManagement",
                newName: "Projects",
                newSchema: "IncentiveManagement");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                schema: "IncentiveManagement",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncentivePlans_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncentivePlans_Projects_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                schema: "IncentiveManagement",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "IncentiveManagement",
                newName: "Project",
                newSchema: "IncentiveManagement");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                schema: "IncentiveManagement",
                table: "Project",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncentivePlans_Project_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                column: "ProjectId",
                principalSchema: "IncentiveManagement",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
