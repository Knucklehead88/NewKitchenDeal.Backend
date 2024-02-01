using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddTradesAndLanguagesSeparation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Language_BusinessInfo_BusinessInfoId",
                table: "Language");

            migrationBuilder.DropForeignKey(
                name: "FK_Trade_BusinessInfo_BusinessInfoId",
                table: "Trade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trade",
                table: "Trade");

            migrationBuilder.DropIndex(
                name: "IX_Trade_BusinessInfoId",
                table: "Trade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Language",
                table: "Language");

            migrationBuilder.DropIndex(
                name: "IX_Language_BusinessInfoId",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "BusinessInfoId",
                table: "Trade");

            migrationBuilder.DropColumn(
                name: "BusinessInfoId",
                table: "Language");

            migrationBuilder.RenameTable(
                name: "Trade",
                newName: "Trades");

            migrationBuilder.RenameTable(
                name: "Language",
                newName: "Languages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trades",
                table: "Trades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BusinessInfoLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    BusinessInfoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessInfoLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessInfoLanguage_BusinessInfo_BusinessInfoId",
                        column: x => x.BusinessInfoId,
                        principalTable: "BusinessInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessInfoLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessInfoTrade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TradeId = table.Column<int>(type: "integer", nullable: false),
                    BusinessInfoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessInfoTrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessInfoTrade_BusinessInfo_BusinessInfoId",
                        column: x => x.BusinessInfoId,
                        principalTable: "BusinessInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessInfoTrade_Trades_TradeId",
                        column: x => x.TradeId,
                        principalTable: "Trades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessInfoLanguage_BusinessInfoId",
                table: "BusinessInfoLanguage",
                column: "BusinessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessInfoLanguage_LanguageId",
                table: "BusinessInfoLanguage",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessInfoTrade_BusinessInfoId",
                table: "BusinessInfoTrade",
                column: "BusinessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessInfoTrade_TradeId",
                table: "BusinessInfoTrade",
                column: "TradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessInfoLanguage");

            migrationBuilder.DropTable(
                name: "BusinessInfoTrade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trades",
                table: "Trades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.RenameTable(
                name: "Trades",
                newName: "Trade");

            migrationBuilder.RenameTable(
                name: "Languages",
                newName: "Language");

            migrationBuilder.AddColumn<int>(
                name: "BusinessInfoId",
                table: "Trade",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessInfoId",
                table: "Language",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trade",
                table: "Trade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Language",
                table: "Language",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Trade_BusinessInfoId",
                table: "Trade",
                column: "BusinessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Language_BusinessInfoId",
                table: "Language",
                column: "BusinessInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Language_BusinessInfo_BusinessInfoId",
                table: "Language",
                column: "BusinessInfoId",
                principalTable: "BusinessInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trade_BusinessInfo_BusinessInfoId",
                table: "Trade",
                column: "BusinessInfoId",
                principalTable: "BusinessInfo",
                principalColumn: "Id");
        }
    }
}
