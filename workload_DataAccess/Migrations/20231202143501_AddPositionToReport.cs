using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    public partial class AddPositionToReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentPosition",
                table: "Reports",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPosition",
                table: "Reports");
        }
    }
}
