using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameResultsInReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "secondSemesterPlan",
                table: "Reports",
                newName: "secondSemesterFact");

            migrationBuilder.RenameColumn(
                name: "firstSemesterPlan",
                table: "Reports",
                newName: "firstSemesterFact");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "secondSemesterFact",
                table: "Reports",
                newName: "secondSemesterPlan");

            migrationBuilder.RenameColumn(
                name: "firstSemesterFact",
                table: "Reports",
                newName: "firstSemesterPlan");
        }
    }
}
