using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    public partial class removeTotalFromModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firstSemesterFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "secondSemesterFact",
                table: "Reports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "firstSemesterFact",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "secondSemesterFact",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
