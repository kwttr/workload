using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class reworkModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "Patronymic");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Reports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "aprilFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "decemberFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "februaryFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "firstSemesterPlan",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "hodName",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "hodPatronymic",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "hodSecondName",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "januaryFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "juneFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "marchFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "mayFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "novemberFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "octoberFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "secondSemesterPlan",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "septemberFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "surveyFirstSemester",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "surveySecondSemester",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "totalWorkFact",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "totalWorkPlan",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DepartmentId",
                table: "Reports",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Department_DepartmentId",
                table: "Reports",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Department_DepartmentId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_DepartmentId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "aprilFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "decemberFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "februaryFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "firstSemesterPlan",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "hodName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "hodPatronymic",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "hodSecondName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "januaryFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "juneFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "marchFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "mayFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "novemberFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "octoberFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "secondSemesterPlan",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "septemberFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "surveyFirstSemester",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "surveySecondSemester",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "totalWorkFact",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "totalWorkPlan",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Patronymic",
                table: "AspNetUsers",
                newName: "FullName");
        }
    }
}
