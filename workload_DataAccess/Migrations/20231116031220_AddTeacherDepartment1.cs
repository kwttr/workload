using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherDepartment1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherDepartments",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeacherId1 = table.Column<string>(type: "TEXT", nullable: true),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_TeacherDepartments_AspNetUsers_TeacherId1",
                        column: x => x.TeacherId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherDepartments_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDepartments_DepartmentId",
                table: "TeacherDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDepartments_TeacherId1",
                table: "TeacherDepartments",
                column: "TeacherId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherDepartments");
        }
    }
}
