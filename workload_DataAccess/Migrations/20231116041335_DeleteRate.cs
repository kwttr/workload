using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "AspNetRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "AspNetRoles",
                type: "TEXT",
                nullable: true);
        }
    }
}
