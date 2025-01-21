using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unshackled.Kitchen.Core.Data.Migrations.MSSQL
{
    /// <inheritdoc />
    public partial class v310a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitLabel",
                table: "uk_ShoppingListRecipeItems",
                newName: "IngredientAmountUnitLabel");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "uk_ShoppingListRecipeItems",
                newName: "IngredientAmount");

            migrationBuilder.AddColumn<int>(
                name: "IngredientAmountUnitType",
                table: "uk_ShoppingListRecipeItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsUnitMismatch",
                table: "uk_ShoppingListRecipeItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ServingSizeUnitType",
                table: "uk_ShoppingListRecipeItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "uk_MealDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                name: "uk_MealPlanRecipes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatePlanned = table.Column<DateOnly>(type: "date", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    MealDefinitionId = table.Column<long>(type: "bigint", nullable: true),
                    Scale = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uk_MealPlanRecipes");

            migrationBuilder.DropTable(
                name: "uk_MealDefinitions");

            migrationBuilder.DropColumn(
                name: "IngredientAmountUnitType",
                table: "uk_ShoppingListRecipeItems");

            migrationBuilder.DropColumn(
                name: "IsUnitMismatch",
                table: "uk_ShoppingListRecipeItems");

            migrationBuilder.DropColumn(
                name: "ServingSizeUnitType",
                table: "uk_ShoppingListRecipeItems");

            migrationBuilder.RenameColumn(
                name: "IngredientAmountUnitLabel",
                table: "uk_ShoppingListRecipeItems",
                newName: "UnitLabel");

            migrationBuilder.RenameColumn(
                name: "IngredientAmount",
                table: "uk_ShoppingListRecipeItems",
                newName: "Amount");
        }
    }
}
