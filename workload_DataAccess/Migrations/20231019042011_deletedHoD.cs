using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class deletedHoD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId1",
                table: "AspNetUsers",
                column: "DepartmentId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId1",
                table: "AspNetUsers",
                column: "DepartmentId1",
                principalTable: "Department",
                principalColumn: "Id");
        }
    }
}
