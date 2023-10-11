using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workload.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Teachers");

            migrationBuilder.AddColumn<int>(
                name: "DegreeId",
                table: "Teachers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "Teachers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Degree",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degree", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DegreeId",
                table: "Teachers",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_PositionId",
                table: "Teachers",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Degree_DegreeId",
                table: "Teachers",
                column: "DegreeId",
                principalTable: "Degree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Position_PositionId",
                table: "Teachers",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Degree_DegreeId",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Position_PositionId",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "Degree");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_DegreeId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_PositionId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Teachers");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Teachers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
