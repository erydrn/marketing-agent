using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingAgent.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalLeadId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Timeline = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Urgency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverallScore = table.Column<int>(type: "int", nullable: false),
                    Tier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompletenessScore = table.Column<int>(type: "int", nullable: false),
                    EngagementScore = table.Column<int>(type: "int", nullable: false),
                    ReadinessScore = table.Column<int>(type: "int", nullable: false),
                    SourceQualityScore = table.Column<int>(type: "int", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadScores_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadSourceAttributions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Campaign = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Medium = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UtmSource = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UtmMedium = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UtmCampaign = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UtmContent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UtmTerm = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Referrer = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LandingPage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CapturedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadSourceAttributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadSourceAttributions_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CreatedAt",
                table: "Leads",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_DeletedAt",
                table: "Leads",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Email",
                table: "Leads",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ExternalLeadId",
                table: "Leads",
                column: "ExternalLeadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Postcode",
                table: "Leads",
                column: "Postcode");

            migrationBuilder.CreateIndex(
                name: "IX_LeadScores_LeadId",
                table: "LeadScores",
                column: "LeadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadScores_OverallScore",
                table: "LeadScores",
                column: "OverallScore");

            migrationBuilder.CreateIndex(
                name: "IX_LeadScores_Tier",
                table: "LeadScores",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_LeadSourceAttributions_Campaign",
                table: "LeadSourceAttributions",
                column: "Campaign");

            migrationBuilder.CreateIndex(
                name: "IX_LeadSourceAttributions_Channel",
                table: "LeadSourceAttributions",
                column: "Channel");

            migrationBuilder.CreateIndex(
                name: "IX_LeadSourceAttributions_LeadId",
                table: "LeadSourceAttributions",
                column: "LeadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadSourceAttributions_Source",
                table: "LeadSourceAttributions",
                column: "Source");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadScores");

            migrationBuilder.DropTable(
                name: "LeadSourceAttributions");

            migrationBuilder.DropTable(
                name: "Leads");
        }
    }
}
