using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NullableHoF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadOfDepartments_Department_DepartmentId",
                table: "HeadOfDepartments");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "HeadOfDepartments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadOfDepartments_Department_DepartmentId",
                table: "HeadOfDepartments",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadOfDepartments_Department_DepartmentId",
                table: "HeadOfDepartments");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "HeadOfDepartments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadOfDepartments_Department_DepartmentId",
                table: "HeadOfDepartments",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
