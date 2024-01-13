using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonalInfoAndUpdateInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessName = table.Column<string>(type: "text", nullable: true),
                    HourlyRate = table.Column<string>(type: "text", nullable: true),
                    DailyRate = table.Column<string>(type: "text", nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    VideoPresentation = table.Column<string>(type: "text", nullable: true),
                    Projects = table.Column<string>(type: "text", nullable: true),
                    SpokenLanguages = table.Column<string[]>(type: "text[]", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessInfos_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonalInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    WhatssAppNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    FacebookProfile = table.Column<string>(type: "text", nullable: true),
                    TwitterProfile = table.Column<string>(type: "text", nullable: true),
                    TikTokProfile = table.Column<string>(type: "text", nullable: true),
                    ProfilePicture = table.Column<string>(type: "text", nullable: true),
                    AppUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalInfos_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    BusinessInfoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trade_BusinessInfos_BusinessInfoId",
                        column: x => x.BusinessInfoId,
                        principalTable: "BusinessInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppUserLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Longitude = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<int>(type: "integer", nullable: false),
                    PersonalInfoId = table.Column<int>(type: "integer", nullable: false),
                    BusinessInfoId = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_BusinessInfos_AppUserId",
                table: "BusinessInfos",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInfos_AppUserId",
                table: "PersonalInfos",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trade_BusinessInfoId",
                table: "Trade",
                column: "BusinessInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserLocation");

            migrationBuilder.DropTable(
                name: "Trade");

            migrationBuilder.DropTable(
                name: "PersonalInfos");

            migrationBuilder.DropTable(
                name: "BusinessInfos");
        }
    }
}
