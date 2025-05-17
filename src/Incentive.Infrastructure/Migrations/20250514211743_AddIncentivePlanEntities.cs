using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incentive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIncentivePlanEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TotalValue = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncentivePlans",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PlanType = table.Column<string>(type: "text", nullable: false),
                    PeriodType = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    PlanDiscriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MetricType = table.Column<int>(type: "integer", nullable: true),
                    TargetValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ConsistencyMonths = table.Column<int>(type: "integer", nullable: true),
                    AwardType = table.Column<int>(type: "integer", nullable: true),
                    AwardValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GiftDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProjectBasedIncentivePlan_MetricType = table.Column<int>(type: "integer", nullable: true),
                    ProjectBasedIncentivePlan_TargetValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    IncentiveValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CalculationType = table.Column<int>(type: "integer", nullable: true),
                    IncentiveAfterExceedingTarget = table.Column<bool>(type: "boolean", nullable: true),
                    Role = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsTeamBased = table.Column<bool>(type: "boolean", nullable: true),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    TargetType = table.Column<int>(type: "integer", nullable: true),
                    SalaryPercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    RoleBasedIncentivePlan_MetricType = table.Column<int>(type: "integer", nullable: true),
                    RoleBasedIncentivePlan_TargetValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    RoleBasedIncentivePlan_CalculationType = table.Column<int>(type: "integer", nullable: true),
                    RoleBasedIncentivePlan_IncentiveValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    IsCumulative = table.Column<bool>(type: "boolean", nullable: true),
                    RoleBasedIncentivePlan_IncentiveAfterExceedingTarget = table.Column<bool>(type: "boolean", nullable: true),
                    IncludeSalaryInTarget = table.Column<bool>(type: "boolean", nullable: true),
                    TargetBasedIncentivePlan_TargetType = table.Column<int>(type: "integer", nullable: true),
                    TargetBasedIncentivePlan_SalaryPercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    TargetBasedIncentivePlan_MetricType = table.Column<int>(type: "integer", nullable: true),
                    TargetBasedIncentivePlan_TargetValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TargetBasedIncentivePlan_CalculationType = table.Column<int>(type: "integer", nullable: true),
                    TargetBasedIncentivePlan_IncentiveValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TargetBasedIncentivePlan_IsCumulative = table.Column<bool>(type: "boolean", nullable: true),
                    TargetBasedIncentivePlan_IncentiveAfterExceedingTarget = table.Column<bool>(type: "boolean", nullable: true),
                    TargetBasedIncentivePlan_IncludeSalaryInTarget = table.Column<bool>(type: "boolean", nullable: true),
                    TieredIncentivePlan_MetricType = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncentivePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncentivePlans_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Role = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeftDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TieredIncentiveTiers",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TieredIncentivePlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ToValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IncentiveValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CalculationType = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TieredIncentiveTiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TieredIncentiveTiers_IncentivePlans_TieredIncentivePlanId",
                        column: x => x.TieredIncentivePlanId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "IncentivePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncentivePlans_ProjectId",
                schema: "IncentiveManagement",
                table: "IncentivePlans",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_TeamId",
                schema: "IncentiveManagement",
                table: "TeamMembers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TieredIncentiveTiers_TieredIncentivePlanId",
                schema: "IncentiveManagement",
                table: "TieredIncentiveTiers",
                column: "TieredIncentivePlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMembers",
                schema: "IncentiveManagement");

            migrationBuilder.DropTable(
                name: "TieredIncentiveTiers",
                schema: "IncentiveManagement");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "IncentiveManagement");

            migrationBuilder.DropTable(
                name: "IncentivePlans",
                schema: "IncentiveManagement");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "IncentiveManagement");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ClosedByUserId",
                schema: "IncentiveManagement",
                table: "Deals",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
