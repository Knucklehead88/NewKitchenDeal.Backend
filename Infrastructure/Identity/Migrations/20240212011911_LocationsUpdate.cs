using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class LocationsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_BusinessInfo_BusinessInfoId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalInfo_Location_LocationId",
                table: "PersonalInfo");

            migrationBuilder.DropIndex(
                name: "IX_PersonalInfo_LocationId",
                table: "PersonalInfo");

            migrationBuilder.DropIndex(
                name: "IX_Location_BusinessInfoId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "PersonalInfo");

            migrationBuilder.DropColumn(
                name: "BusinessInfoId",
                table: "Location");

            migrationBuilder.AddColumn<bool>(
                name: "CanReceiveTextMessages",
                table: "PersonalInfo",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BusinessInfoLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    BusinessInfoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessInfoLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessInfoLocation_BusinessInfo_BusinessInfoId",
                        column: x => x.BusinessInfoId,
                        principalTable: "BusinessInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessInfoLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalInfoLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    PersonalInfoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalInfoLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalInfoLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalInfoLocation_PersonalInfo_PersonalInfoId",
                        column: x => x.PersonalInfoId,
                        principalTable: "PersonalInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessInfoLocation_BusinessInfoId",
                table: "BusinessInfoLocation",
                column: "BusinessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessInfoLocation_LocationId",
                table: "BusinessInfoLocation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInfoLocation_LocationId",
                table: "PersonalInfoLocation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInfoLocation_PersonalInfoId",
                table: "PersonalInfoLocation",
                column: "PersonalInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessInfoLocation");

            migrationBuilder.DropTable(
                name: "PersonalInfoLocation");

            migrationBuilder.DropColumn(
                name: "CanReceiveTextMessages",
                table: "PersonalInfo");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "PersonalInfo",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessInfoId",
                table: "Location",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInfo_LocationId",
                table: "PersonalInfo",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_BusinessInfoId",
                table: "Location",
                column: "BusinessInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_BusinessInfo_BusinessInfoId",
                table: "Location",
                column: "BusinessInfoId",
                principalTable: "BusinessInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalInfo_Location_LocationId",
                table: "PersonalInfo",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }
    }
}
