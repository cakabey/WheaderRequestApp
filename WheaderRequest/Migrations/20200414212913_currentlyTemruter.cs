using Microsoft.EntityFrameworkCore.Migrations;

namespace WheaderRequest.Migrations
{
    public partial class currentlyTemruter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Temperature",
                table: "SummaryData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "SummaryData");
        }
    }
}
