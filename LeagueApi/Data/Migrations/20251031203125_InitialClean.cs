using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompetitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    HomeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AwayId = table.Column<int>(type: "INTEGER", nullable: false),
                    HomeGoals = table.Column<int>(type: "INTEGER", nullable: true),
                    AwayGoals = table.Column<int>(type: "INTEGER", nullable: true),
                    HomeCorners = table.Column<int>(type: "INTEGER", nullable: true),
                    AwayCorners = table.Column<int>(type: "INTEGER", nullable: true),
                    HomeShotsTotal = table.Column<int>(type: "INTEGER", nullable: true),
                    AwayShotsTotal = table.Column<int>(type: "INTEGER", nullable: true),
                    HomeShotsOnTarget = table.Column<int>(type: "INTEGER", nullable: true),
                    AwayShotsOnTarget = table.Column<int>(type: "INTEGER", nullable: true),
                    HomeShotsOffTarget = table.Column<int>(type: "INTEGER", nullable: true),
                    AwayShotsOffTarget = table.Column<int>(type: "INTEGER", nullable: true),
                    HomeYellowCards = table.Column<int>(type: "INTEGER", nullable: true),
                    AwayYellowCards = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_AwayId",
                        column: x => x.AwayId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayId",
                table: "Matches",
                column: "AwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CompetitionId_Year",
                table: "Matches",
                columns: new[] { "CompetitionId", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CompetitionId_Year_HomeId_AwayId",
                table: "Matches",
                columns: new[] { "CompetitionId", "Year", "HomeId", "AwayId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeId",
                table: "Matches",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
