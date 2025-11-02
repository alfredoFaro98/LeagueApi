using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKickoffUtcToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "KickoffUtc",
                table: "Matches",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KickoffUtc",
                table: "Matches");
        }
    }
}
