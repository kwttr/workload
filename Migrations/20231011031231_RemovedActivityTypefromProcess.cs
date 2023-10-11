using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace workload.Migrations
{
    /// <inheritdoc />
    public partial class RemovedActivityTypefromProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessActivityType_Activities_ActivityTypeId",
                table: "ProcessActivityType");

            migrationBuilder.DropIndex(
                name: "IX_ProcessActivityType_ActivityTypeId",
                table: "ProcessActivityType");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeId",
                table: "ProcessActivityType",
                newName: "NormHours");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProcessActivityType",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Degree",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Доцент" },
                    { 2, "Профессор" },
                    { 3, "Кандидат" },
                    { 4, "Доктор" }
                });

            migrationBuilder.InsertData(
                table: "Position",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Аспирант" },
                    { 2, "Ассистент" },
                    { 3, "Ведущий научный сотрудник" },
                    { 4, "Главный научный сотрудник" },
                    { 5, "Преподаватель" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Degree",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Degree",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Degree",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Degree",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Position",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Position",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Position",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Position",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Position",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProcessActivityType");

            migrationBuilder.RenameColumn(
                name: "NormHours",
                table: "ProcessActivityType",
                newName: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessActivityType_ActivityTypeId",
                table: "ProcessActivityType",
                column: "ActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessActivityType_Activities_ActivityTypeId",
                table: "ProcessActivityType",
                column: "ActivityTypeId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
