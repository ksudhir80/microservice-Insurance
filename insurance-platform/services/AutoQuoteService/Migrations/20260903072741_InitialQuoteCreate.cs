using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoQuoteService.Migrations
{
    /// <inheritdoc />
    public partial class InitialQuoteCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoQuotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuoteId = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    VehicleMake = table.Column<string>(type: "text", nullable: false),
                    VehicleModel = table.Column<string>(type: "text", nullable: false),
                    VehicleYear = table.Column<int>(type: "integer", nullable: false),
                    Vin = table.Column<string>(type: "text", nullable: false),
                    DriverDateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    LicenseState = table.Column<string>(type: "text", nullable: false),
                    Accidents = table.Column<int>(type: "integer", nullable: false),
                    Violations = table.Column<int>(type: "integer", nullable: false),
                    BodilyInjury = table.Column<string>(type: "text", nullable: false),
                    PropertyDamage = table.Column<decimal>(type: "numeric", nullable: false),
                    Collision = table.Column<bool>(type: "boolean", nullable: false),
                    Comprehensive = table.Column<bool>(type: "boolean", nullable: false),
                    BasePremium = table.Column<decimal>(type: "numeric", nullable: false),
                    RiskAdjustment = table.Column<decimal>(type: "numeric", nullable: false),
                    CoverageAdjustment = table.Column<decimal>(type: "numeric", nullable: false),
                    Premium = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyPremium = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoQuotes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoQuotes");
        }
    }
}
