using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unshackled.Kitchen.Core.Data.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class v310 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "uk_MealDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_MealDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_MealDefinitions_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_ProductImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Container = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RelativePath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    IsFeatured = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "uk_MealPlanRecipes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatePlanned = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    MealDefinitionId = table.Column<long>(type: "INTEGER", nullable: true),
                    Scale = table.Column<decimal>(type: "TEXT", precision: 4, scale: 2, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_MealPlanRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_MealPlanRecipes_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_MealPlanRecipes_uk_MealDefinitions_MealDefinitionId",
                        column: x => x.MealDefinitionId,
                        principalTable: "uk_MealDefinitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_MealPlanRecipes_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
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
                name: "IX_uk_MealDefinitions_DateCreatedUtc",
                table: "uk_MealDefinitions",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealDefinitions_DateLastModifiedUtc",
                table: "uk_MealDefinitions",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealDefinitions_HouseholdId",
                table: "uk_MealDefinitions",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealDefinitions_SortOrder",
                table: "uk_MealDefinitions",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealDefinitions_Title",
                table: "uk_MealDefinitions",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealPlanRecipes_DateCreatedUtc",
                table: "uk_MealPlanRecipes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealPlanRecipes_DateLastModifiedUtc",
                table: "uk_MealPlanRecipes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealPlanRecipes_DatePlanned_MealDefinitionId",
                table: "uk_MealPlanRecipes",
                columns: new[] { "DatePlanned", "MealDefinitionId" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealPlanRecipes_HouseholdId",
                table: "uk_MealPlanRecipes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealPlanRecipes_MealDefinitionId",
                table: "uk_MealPlanRecipes",
                column: "MealDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MealPlanRecipes_RecipeId",
                table: "uk_MealPlanRecipes",
                column: "RecipeId");

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
                name: "uk_MealPlanRecipes");

            migrationBuilder.DropTable(
                name: "uk_ProductImages");

            migrationBuilder.DropTable(
                name: "uk_MealDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_uk_RecipeImages_DateCreatedUtc",
                table: "uk_RecipeImages");

            migrationBuilder.DropIndex(
                name: "IX_uk_RecipeImages_DateLastModifiedUtc",
                table: "uk_RecipeImages");
        }
    }
}
