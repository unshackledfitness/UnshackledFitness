using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unshackled.Kitchen.Core.Data.Migrations.MSSQL
{
    /// <inheritdoc />
    public partial class v310 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RelativePath",
                table: "uk_RecipeImages",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "uk_RecipeImages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Container",
                table: "uk_RecipeImages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "uk_ProductImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Container = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_ProductImages_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_ProductImages_uk_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uk_Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeImages_DateCreatedUtc",
                table: "uk_RecipeImages",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeImages_DateLastModifiedUtc",
                table: "uk_RecipeImages",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductImages_DateCreatedUtc",
                table: "uk_ProductImages",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductImages_DateLastModifiedUtc",
                table: "uk_ProductImages",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductImages_HouseholdId",
                table: "uk_ProductImages",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductImages_ProductId",
                table: "uk_ProductImages",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uk_ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_uk_RecipeImages_DateCreatedUtc",
                table: "uk_RecipeImages");

            migrationBuilder.DropIndex(
                name: "IX_uk_RecipeImages_DateLastModifiedUtc",
                table: "uk_RecipeImages");

            migrationBuilder.AlterColumn<string>(
                name: "RelativePath",
                table: "uk_RecipeImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "uk_RecipeImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Container",
                table: "uk_RecipeImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
