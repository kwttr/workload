using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentTeacher");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");
        }
    }
}
