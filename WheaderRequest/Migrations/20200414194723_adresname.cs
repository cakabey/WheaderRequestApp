using Microsoft.EntityFrameworkCore.Migrations;

namespace WheaderRequest.Migrations
{
    public partial class adresname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdressName",
                table: "WsProcessInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdressName",
                table: "WsProcessInfo");
        }
    }
}
