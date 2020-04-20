using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WheaderRequest.Migrations
{
    public partial class summaryDataTableadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SummaryData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessId = table.Column<int>(nullable: false),
                    DayDate = table.Column<DateTime>(nullable: false),
                    PlaceId = table.Column<string>(nullable: true),
                    PlaceName = table.Column<string>(nullable: true),
                    Lat = table.Column<string>(nullable: true),
                    Lon = table.Column<string>(nullable: true),
                    DailyMinTemperature = table.Column<string>(nullable: true),
                    DailyMaxTemperature = table.Column<string>(nullable: true),
                    WeeklyMinTemperature = table.Column<string>(nullable: true),
                    WeeklyMaxTemperature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummaryData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SummaryData");
        }
    }
}
