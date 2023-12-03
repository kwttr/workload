using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    public partial class idk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiresPasswordChange",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiresPasswordChange",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);
        }
    }
}
