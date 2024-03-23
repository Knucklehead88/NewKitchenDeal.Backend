using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ChangesToBusinessInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float[]>(
                name: "Bbox",
                table: "Location",
                type: "real[]",
                nullable: true,
                oldClrType: typeof(int[]),
                oldType: "integer[]",
                oldNullable: true);

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "HourlyRate",
            //    table: "BusinessInfo",
            //    type: "numeric",
            //    nullable: false,
            //    defaultValue: 0m,
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);
            migrationBuilder.Sql(@"ALTER TABLE ""BusinessInfo"" ALTER COLUMN ""HourlyRate"" TYPE DECIMAL USING ""HourlyRate""::decimal");

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "DailyRate",
            //    table: "BusinessInfo",
            //    type: "numeric",
            //    nullable: false,
            //    defaultValue: 0m,
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            migrationBuilder.Sql(@"ALTER TABLE ""BusinessInfo"" ALTER COLUMN ""DailyRate"" TYPE DECIMAL USING ""DailyRate""::decimal");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int[]>(
                name: "Bbox",
                table: "Location",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(float[]),
                oldType: "real[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HourlyRate",
                table: "BusinessInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "DailyRate",
                table: "BusinessInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
