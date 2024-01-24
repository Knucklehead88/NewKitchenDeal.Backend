using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocationForPIAndBI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserLocation");

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    PersonalInfoId = table.Column<int>(type: "integer", nullable: true),
                    BusinesslInfoId = table.Column<int>(type: "integer", nullable: true),
                    BusinessInfoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_BusinessInfos_BusinessInfoId",
                        column: x => x.BusinessInfoId,
                        principalTable: "BusinessInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Location_PersonalInfos_PersonalInfoId",
                        column: x => x.PersonalInfoId,
                        principalTable: "PersonalInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Location_BusinessInfoId",
                table: "Location",
                column: "BusinessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_PersonalInfoId",
                table: "Location",
                column: "PersonalInfoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.CreateTable(
                name: "AppUserLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonalInfoId = table.Column<int>(type: "integer", nullable: false),
                    BusinessInfoId = table.Column<int>(type: "integer", nullable: true),
                    Latitude = table.Column<int>(type: "integer", nullable: false),
                    Longitude = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserLocation_BusinessInfos_BusinessInfoId",
                        column: x => x.BusinessInfoId,
                        principalTable: "BusinessInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppUserLocation_PersonalInfos_PersonalInfoId",
                        column: x => x.PersonalInfoId,
                        principalTable: "PersonalInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLocation_BusinessInfoId",
                table: "AppUserLocation",
                column: "BusinessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLocation_PersonalInfoId",
                table: "AppUserLocation",
                column: "PersonalInfoId",
                unique: true);
        }
    }
}
