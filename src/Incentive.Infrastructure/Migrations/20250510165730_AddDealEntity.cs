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
            migrationBuilder.CreateTable(
                name: "Deals",
                schema: "IncentiveManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalespersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DealDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProjectId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    SalespersonId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deals_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_Projects_ProjectId1",
                        column: x => x.ProjectId1,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deals_Salespeople_SalespersonId",
                        column: x => x.SalespersonId,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Salespeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_Salespeople_SalespersonId1",
                        column: x => x.SalespersonId1,
                        principalSchema: "IncentiveManagement",
                        principalTable: "Salespeople",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ProjectId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ProjectId1",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SalespersonId",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "SalespersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SalespersonId1",
                schema: "IncentiveManagement",
                table: "Deals",
                column: "SalespersonId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deals",
                schema: "IncentiveManagement");
        }
    }
}
