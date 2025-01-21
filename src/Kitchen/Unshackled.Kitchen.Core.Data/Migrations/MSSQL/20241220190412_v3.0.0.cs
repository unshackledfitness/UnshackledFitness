using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unshackled.Kitchen.Core.Data.Migrations.MSSQL
{
    /// <inheritdoc />
    public partial class v300 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uk_RecipeStepIngredients");

            migrationBuilder.CreateTable(
                name: "uk_RecipeImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    Container = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_RecipeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_RecipeImages_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeImages_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeImages_HouseholdId",
                table: "uk_RecipeImages",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeImages_RecipeId",
                table: "uk_RecipeImages",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uk_RecipeImages");

            migrationBuilder.CreateTable(
                name: "uk_RecipeStepIngredients",
                columns: table => new
                {
                    RecipeStepId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeIngredientId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_RecipeStepIngredients", x => new { x.RecipeStepId, x.RecipeIngredientId });
                    table.ForeignKey(
                        name: "FK_uk_RecipeStepIngredients_uk_RecipeIngredients_RecipeIngredientId",
                        column: x => x.RecipeIngredientId,
                        principalTable: "uk_RecipeIngredients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeStepIngredients_uk_RecipeSteps_RecipeStepId",
                        column: x => x.RecipeStepId,
                        principalTable: "uk_RecipeSteps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeStepIngredients_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeStepIngredients_RecipeId",
                table: "uk_RecipeStepIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeStepIngredients_RecipeIngredientId",
                table: "uk_RecipeStepIngredients",
                column: "RecipeIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeStepIngredients_RecipeStepId",
                table: "uk_RecipeStepIngredients",
                column: "RecipeStepId");
        }
    }
}
