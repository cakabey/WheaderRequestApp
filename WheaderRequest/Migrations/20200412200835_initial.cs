using Microsoft.EntityFrameworkCore.Migrations;

namespace WheaderRequest.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdressInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlaceId = table.Column<string>(nullable: true),
                    Licence = table.Column<string>(nullable: true),
                    OsmType = table.Column<string>(nullable: true),
                    OsmId = table.Column<string>(nullable: true),
                    Lat = table.Column<string>(nullable: true),
                    Lon = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Importance = table.Column<double>(nullable: false),
                    RequestInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdressInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdressInfos");
        }
    }
}
