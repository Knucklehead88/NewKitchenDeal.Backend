using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class BusinessInfoAndLanguageUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "BusinessInfo");

            migrationBuilder.AddColumn<int[]>(
                name: "Bbox",
                table: "Location",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapBoxId",
                table: "Location",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StartDateOfWork",
                table: "BusinessInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bbox",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "MapBoxId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "StartDateOfWork",
                table: "BusinessInfo");

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "BusinessInfo",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
