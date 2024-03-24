using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OLAP.API.Infrastructure.Contexts.Data.Migrations
{
    /// <inheritdoc />
    public partial class SplitTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                schema: "Data",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "CountryName",
                schema: "Data",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "IndicatorCode",
                schema: "Data",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "IndicatorName",
                schema: "Data",
                table: "Data");

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                schema: "Data",
                table: "Data",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IndicatorId",
                schema: "Data",
                table: "Data",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Indicators",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IndicatorName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IndicatorCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Data_CountryId",
                schema: "Data",
                table: "Data",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Data_IndicatorId",
                schema: "Data",
                table: "Data",
                column: "IndicatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Data_Countries_CountryId",
                schema: "Data",
                table: "Data",
                column: "CountryId",
                principalSchema: "Data",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Data_Indicators_IndicatorId",
                schema: "Data",
                table: "Data",
                column: "IndicatorId",
                principalSchema: "Data",
                principalTable: "Indicators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_Countries_CountryId",
                schema: "Data",
                table: "Data");

            migrationBuilder.DropForeignKey(
                name: "FK_Data_Indicators_IndicatorId",
                schema: "Data",
                table: "Data");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "Indicators",
                schema: "Data");

            migrationBuilder.DropIndex(
                name: "IX_Data_CountryId",
                schema: "Data",
                table: "Data");

            migrationBuilder.DropIndex(
                name: "IX_Data_IndicatorId",
                schema: "Data",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "Data",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "IndicatorId",
                schema: "Data",
                table: "Data");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                schema: "Data",
                table: "Data",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                schema: "Data",
                table: "Data",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IndicatorCode",
                schema: "Data",
                table: "Data",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IndicatorName",
                schema: "Data",
                table: "Data",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }
    }
}
