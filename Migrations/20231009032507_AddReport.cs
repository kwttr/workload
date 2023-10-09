using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload.Migrations
{
    /// <inheritdoc />
    public partial class AddReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentDegree",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "Reports",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Reports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TeacherId",
                table: "Reports",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Teachers_TeacherId",
                table: "Reports",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Teachers_TeacherId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TeacherId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CurrentDegree",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reports");
        }
    }
}
