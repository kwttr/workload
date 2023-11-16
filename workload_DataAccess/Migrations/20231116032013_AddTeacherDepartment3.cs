using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherDepartment3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentTeacher");

            migrationBuilder.CreateTable(
                name: "TeacherDepartment",
                columns: table => new
                {
                    TeacherId = table.Column<string>(type: "TEXT", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherDepartment", x => new { x.TeacherId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_TeacherDepartment_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherDepartment_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDepartment_DepartmentId",
                table: "TeacherDepartment",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherDepartment");

            migrationBuilder.CreateTable(
                name: "DepartmentTeacher",
                columns: table => new
                {
                    TeacherDepartmentsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeachersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTeacher", x => new { x.TeacherDepartmentsId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_DepartmentTeacher_AspNetUsers_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentTeacher_Department_TeacherDepartmentsId",
                        column: x => x.TeacherDepartmentsId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTeacher_TeachersId",
                table: "DepartmentTeacher",
                column: "TeachersId");
        }
    }
}
