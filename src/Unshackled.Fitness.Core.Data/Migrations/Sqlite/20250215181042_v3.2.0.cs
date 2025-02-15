using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unshackled.Fitness.Core.Data.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class v320 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "uf_Cookbooks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Cookbooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Cookbooks_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Households",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContentUid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Households", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Households_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_CookbookInvites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CookbookId = table.Column<long>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Permissions = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_CookbookInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_CookbookInvites_uf_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "uf_Cookbooks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_CookbookMembers",
                columns: table => new
                {
                    CookbookId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false),
                    PermissionLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_CookbookMembers", x => new { x.CookbookId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_uf_CookbookMembers_uf_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "uf_Cookbooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_CookbookMembers_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_HouseholdInvites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Permissions = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_HouseholdInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_HouseholdInvites_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_HouseholdMembers",
                columns: table => new
                {
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false),
                    PermissionLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_HouseholdMembers", x => new { x.HouseholdId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_uf_HouseholdMembers_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_HouseholdMembers_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_MealPrepPlanSlots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_MealPrepPlanSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_MealPrepPlanSlots_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ProductBundles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ProductBundles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_ProductBundles_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ProductCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_ProductCategories_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Recipes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContentUid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    CookTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    PrepTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalServings = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Recipes_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Stores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Stores_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Tags_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContentUid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, collation: "NOCASE"),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, collation: "NOCASE"),
                    ProductCategoryId = table.Column<long>(type: "INTEGER", nullable: true),
                    IsAutoSkipped = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasNutritionInfo = table.Column<bool>(type: "INTEGER", nullable: false),
                    ServingSize = table.Column<decimal>(type: "TEXT", precision: 8, scale: 3, nullable: false),
                    ServingSizeN = table.Column<decimal>(type: "TEXT", precision: 13, scale: 6, nullable: false),
                    ServingSizeUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ServingSizeUnitLabel = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false, collation: "NOCASE"),
                    ServingSizeMetric = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    ServingSizeMetricN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    ServingSizeMetricUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ServingsPerContainer = table.Column<decimal>(type: "TEXT", precision: 8, scale: 3, nullable: false),
                    Calories = table.Column<int>(type: "INTEGER", nullable: false),
                    CaloriesFromFat = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalFat = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    TotalFatUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalFatN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    SaturatedFat = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    SaturatedFatUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    SaturatedFatN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    TransFat = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    TransFatUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TransFatN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    PolyunsaturatedFat = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    PolyunsaturatedFatUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    PolyunsaturatedFatN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    MonounsaturatedFat = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    MonounsaturatedFatUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    MonounsaturatedFatN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Cholesterol = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    CholesterolUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    CholesterolN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    TotalCarbohydrates = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    TotalCarbohydratesUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalCarbohydratesN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    DietaryFiber = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    DietaryFiberUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    DietaryFiberN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    SolubleFiber = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    SolubleFiberUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    SolubleFiberN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    InsolubleFiber = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    InsolubleFiberUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    InsolubleFiberN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    TotalSugars = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    TotalSugarsUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalSugarsN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    AddedSugars = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    AddedSugarsUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    AddedSugarsN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    SugarAlcohols = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    SugarAlcoholsUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    SugarAlcoholsN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Protein = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    ProteinUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ProteinN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Biotin = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    BiotinUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    BiotinN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Choline = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    CholineUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    CholineN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Folate = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    FolateUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    FolateN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Niacin = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    NiacinUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    NiacinN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    PantothenicAcid = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    PantothenicAcidUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    PantothenicAcidN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Riboflavin = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    RiboflavinUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    RiboflavinN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Thiamin = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    ThiaminUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ThiaminN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    VitaminA = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    VitaminAUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    VitaminAN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    VitaminB6 = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    VitaminB6Unit = table.Column<int>(type: "INTEGER", nullable: false),
                    VitaminB6N = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    VitaminB12 = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    VitaminB12Unit = table.Column<int>(type: "INTEGER", nullable: false),
                    VitaminB12N = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    VitaminC = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    VitaminCUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    VitaminCN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    VitaminD = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    VitaminDUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    VitaminDN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    VitaminE = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    VitaminEUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    VitaminEN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    VitaminK = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    VitaminKUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    VitaminKN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Calcium = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    CalciumUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    CalciumN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Chloride = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    ChlorideUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ChlorideN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Chromium = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    ChromiumUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ChromiumN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Copper = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    CopperUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    CopperN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Iodine = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    IodineUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    IodineN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Iron = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    IronUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    IronN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Magnesium = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    MagnesiumUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    MagnesiumN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Manganese = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    ManganeseUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ManganeseN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Molybdenum = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    MolybdenumUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    MolybdenumN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Phosphorus = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    PhosphorusUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    PhosphorusN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Potassium = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    PotassiumUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    PotassiumN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Selenium = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    SeleniumUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    SeleniumN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Sodium = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    SodiumUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    SodiumN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    Zinc = table.Column<decimal>(type: "TEXT", precision: 7, scale: 2, nullable: false),
                    ZincUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ZincN = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Products_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_Products_uf_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "uf_ProductCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_CookbookRecipes",
                columns: table => new
                {
                    CookbookId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_CookbookRecipes", x => new { x.CookbookId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_uf_CookbookRecipes_uf_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "uf_Cookbooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_CookbookRecipes_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_CookbookRecipes_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_CookbookRecipes_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_MealPrepPlanRecipes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatePlanned = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    SlotId = table.Column<long>(type: "INTEGER", nullable: true),
                    Scale = table.Column<decimal>(type: "TEXT", precision: 4, scale: 2, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_MealPrepPlanRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_MealPrepPlanRecipes_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_MealPrepPlanRecipes_uf_MealPrepPlanSlots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "uf_MealPrepPlanSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_MealPrepPlanRecipes_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_RecipeImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Container = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    RelativePath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    MimeType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    IsFeatured = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_RecipeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_RecipeImages_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_RecipeImages_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_RecipeIngredientGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_RecipeIngredientGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_RecipeIngredientGroups_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_RecipeIngredientGroups_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_RecipeNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_RecipeNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_RecipeNotes_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_RecipeNotes_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_RecipeSteps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Instructions = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_RecipeSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_RecipeSteps_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_RecipeSteps_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    StoreId = table.Column<long>(type: "INTEGER", nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ShoppingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_ShoppingLists_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ShoppingLists_uf_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "uf_Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_StoreLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StoreId = table.Column<long>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, collation: "NOCASE"),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_StoreLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_StoreLocations_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_StoreLocations_uf_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "uf_Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_RecipeTags",
                columns: table => new
                {
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    TagId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_RecipeTags", x => new { x.RecipeId, x.TagId });
                    table.ForeignKey(
                        name: "FK_uf_RecipeTags_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_RecipeTags_uf_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "uf_Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ProductBundleItems",
                columns: table => new
                {
                    ProductBundleId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ProductBundleItems", x => new { x.ProductBundleId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_uf_ProductBundleItems_uf_ProductBundles_ProductBundleId",
                        column: x => x.ProductBundleId,
                        principalTable: "uf_ProductBundles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ProductBundleItems_uf_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uf_Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ProductImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Container = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    RelativePath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    MimeType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    IsFeatured = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_ProductImages_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ProductImages_uf_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uf_Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ProductSubstitutions",
                columns: table => new
                {
                    IngredientKey = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ProductSubstitutions", x => new { x.IngredientKey, x.ProductId });
                    table.ForeignKey(
                        name: "FK_uf_ProductSubstitutions_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ProductSubstitutions_uf_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uf_Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_RecipeIngredients",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    ListGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 8, scale: 3, nullable: false),
                    AmountN = table.Column<decimal>(type: "TEXT", precision: 15, scale: 3, nullable: false),
                    AmountUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountLabel = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false, collation: "NOCASE"),
                    AmountText = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, collation: "NOCASE"),
                    PrepNote = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, collation: "NOCASE"),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_RecipeIngredients_uf_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uf_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_RecipeIngredients_uf_RecipeIngredientGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "uf_RecipeIngredientGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_RecipeIngredients_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ShoppingListItems",
                columns: table => new
                {
                    ShoppingListId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInCart = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ShoppingListItems", x => new { x.ShoppingListId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_uf_ShoppingListItems_uf_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uf_Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ShoppingListItems_uf_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "uf_ShoppingLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ShoppingListRecipeItems",
                columns: table => new
                {
                    ShoppingListId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    InstanceId = table.Column<int>(type: "INTEGER", nullable: false),
                    IngredientKey = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IngredientAmount = table.Column<decimal>(type: "TEXT", precision: 8, scale: 3, nullable: false),
                    IngredientAmountUnitLabel = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    PortionUsed = table.Column<decimal>(type: "TEXT", precision: 15, scale: 10, nullable: false),
                    IngredientAmountUnitType = table.Column<int>(type: "INTEGER", nullable: false),
                    ServingSizeUnitType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsUnitMismatch = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ShoppingListRecipeItems", x => new { x.ShoppingListId, x.ProductId, x.RecipeId, x.InstanceId });
                    table.ForeignKey(
                        name: "FK_uf_ShoppingListRecipeItems_uf_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uf_Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ShoppingListRecipeItems_uf_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uf_Recipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ShoppingListRecipeItems_uf_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "uf_ShoppingLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_StoreProductLocations",
                columns: table => new
                {
                    StoreId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    StoreLocationId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_StoreProductLocations", x => new { x.StoreId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_uf_StoreProductLocations_uf_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uf_Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_StoreProductLocations_uf_StoreLocations_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "uf_StoreLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_StoreProductLocations_uf_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "uf_Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_uf_CookbookInvites_CookbookId",
                table: "uf_CookbookInvites",
                column: "CookbookId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_CookbookInvites_DateCreatedUtc",
                table: "uf_CookbookInvites",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_CookbookInvites_DateLastModifiedUtc",
                table: "uf_CookbookInvites",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_CookbookMembers_MemberId",
                table: "uf_CookbookMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_CookbookRecipes_HouseholdId",
                table: "uf_CookbookRecipes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_CookbookRecipes_MemberId",
                table: "uf_CookbookRecipes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_CookbookRecipes_RecipeId",
                table: "uf_CookbookRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Cookbooks_DateCreatedUtc",
                table: "uf_Cookbooks",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Cookbooks_DateLastModifiedUtc",
                table: "uf_Cookbooks",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Cookbooks_MemberId",
                table: "uf_Cookbooks",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_HouseholdInvites_DateCreatedUtc",
                table: "uf_HouseholdInvites",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_HouseholdInvites_DateLastModifiedUtc",
                table: "uf_HouseholdInvites",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_HouseholdInvites_HouseholdId",
                table: "uf_HouseholdInvites",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_HouseholdMembers_MemberId",
                table: "uf_HouseholdMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Households_DateCreatedUtc",
                table: "uf_Households",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Households_DateLastModifiedUtc",
                table: "uf_Households",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Households_MemberId",
                table: "uf_Households",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanRecipes_DateCreatedUtc",
                table: "uf_MealPrepPlanRecipes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanRecipes_DateLastModifiedUtc",
                table: "uf_MealPrepPlanRecipes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanRecipes_DatePlanned_SlotId",
                table: "uf_MealPrepPlanRecipes",
                columns: new[] { "DatePlanned", "SlotId" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanRecipes_HouseholdId",
                table: "uf_MealPrepPlanRecipes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanRecipes_RecipeId",
                table: "uf_MealPrepPlanRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanRecipes_SlotId",
                table: "uf_MealPrepPlanRecipes",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanSlots_DateCreatedUtc",
                table: "uf_MealPrepPlanSlots",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanSlots_DateLastModifiedUtc",
                table: "uf_MealPrepPlanSlots",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanSlots_HouseholdId",
                table: "uf_MealPrepPlanSlots",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanSlots_SortOrder",
                table: "uf_MealPrepPlanSlots",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MealPrepPlanSlots_Title",
                table: "uf_MealPrepPlanSlots",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductBundleItems_ProductId",
                table: "uf_ProductBundleItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductBundles_DateCreatedUtc",
                table: "uf_ProductBundles",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductBundles_DateLastModifiedUtc",
                table: "uf_ProductBundles",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductBundles_HouseholdId",
                table: "uf_ProductBundles",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductBundles_HouseholdId_Title",
                table: "uf_ProductBundles",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductCategories_DateCreatedUtc",
                table: "uf_ProductCategories",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductCategories_DateLastModifiedUtc",
                table: "uf_ProductCategories",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductCategories_HouseholdId",
                table: "uf_ProductCategories",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductCategories_Title",
                table: "uf_ProductCategories",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductImages_DateCreatedUtc",
                table: "uf_ProductImages",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductImages_DateLastModifiedUtc",
                table: "uf_ProductImages",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductImages_HouseholdId",
                table: "uf_ProductImages",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductImages_ProductId",
                table: "uf_ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Products_DateCreatedUtc",
                table: "uf_Products",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Products_DateLastModifiedUtc",
                table: "uf_Products",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Products_HouseholdId",
                table: "uf_Products",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Products_HouseholdId_Title",
                table: "uf_Products",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_Products_ProductCategoryId",
                table: "uf_Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductSubstitutions_HouseholdId_IngredientKey",
                table: "uf_ProductSubstitutions",
                columns: new[] { "HouseholdId", "IngredientKey" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProductSubstitutions_ProductId",
                table: "uf_ProductSubstitutions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeImages_DateCreatedUtc",
                table: "uf_RecipeImages",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeImages_DateLastModifiedUtc",
                table: "uf_RecipeImages",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeImages_HouseholdId",
                table: "uf_RecipeImages",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeImages_RecipeId",
                table: "uf_RecipeImages",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredientGroups_DateCreatedUtc",
                table: "uf_RecipeIngredientGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredientGroups_DateLastModifiedUtc",
                table: "uf_RecipeIngredientGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredientGroups_HouseholdId",
                table: "uf_RecipeIngredientGroups",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredientGroups_RecipeId_SortOrder",
                table: "uf_RecipeIngredientGroups",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredients_DateCreatedUtc",
                table: "uf_RecipeIngredients",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredients_DateLastModifiedUtc",
                table: "uf_RecipeIngredients",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredients_HouseholdId",
                table: "uf_RecipeIngredients",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredients_HouseholdId_Key",
                table: "uf_RecipeIngredients",
                columns: new[] { "HouseholdId", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredients_ListGroupId",
                table: "uf_RecipeIngredients",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeIngredients_RecipeId_SortOrder",
                table: "uf_RecipeIngredients",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeNotes_DateCreatedUtc",
                table: "uf_RecipeNotes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeNotes_DateLastModifiedUtc",
                table: "uf_RecipeNotes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeNotes_HouseholdId",
                table: "uf_RecipeNotes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeNotes_RecipeId_SortOrder",
                table: "uf_RecipeNotes",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_Recipes_DateCreatedUtc",
                table: "uf_Recipes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Recipes_DateLastModifiedUtc",
                table: "uf_Recipes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Recipes_HouseholdId",
                table: "uf_Recipes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Recipes_HouseholdId_Title",
                table: "uf_Recipes",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeSteps_DateCreatedUtc",
                table: "uf_RecipeSteps",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeSteps_DateLastModifiedUtc",
                table: "uf_RecipeSteps",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeSteps_HouseholdId",
                table: "uf_RecipeSteps",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeSteps_RecipeId_SortOrder",
                table: "uf_RecipeSteps",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_RecipeTags_TagId",
                table: "uf_RecipeTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ShoppingListItems_ProductId",
                table: "uf_ShoppingListItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ShoppingListRecipeItems_ProductId",
                table: "uf_ShoppingListRecipeItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ShoppingListRecipeItems_RecipeId",
                table: "uf_ShoppingListRecipeItems",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ShoppingLists_DateCreatedUtc",
                table: "uf_ShoppingLists",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ShoppingLists_DateLastModifiedUtc",
                table: "uf_ShoppingLists",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ShoppingLists_HouseholdId",
                table: "uf_ShoppingLists",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ShoppingLists_HouseholdId_Title",
                table: "uf_ShoppingLists",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_ShoppingLists_StoreId",
                table: "uf_ShoppingLists",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_StoreLocations_DateCreatedUtc",
                table: "uf_StoreLocations",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_StoreLocations_DateLastModifiedUtc",
                table: "uf_StoreLocations",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_StoreLocations_HouseholdId",
                table: "uf_StoreLocations",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_StoreLocations_HouseholdId_StoreId_SortOrder",
                table: "uf_StoreLocations",
                columns: new[] { "HouseholdId", "StoreId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_StoreLocations_StoreId",
                table: "uf_StoreLocations",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_StoreProductLocations_ProductId",
                table: "uf_StoreProductLocations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_StoreProductLocations_StoreId_StoreLocationId_SortOrder",
                table: "uf_StoreProductLocations",
                columns: new[] { "StoreId", "StoreLocationId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_StoreProductLocations_StoreLocationId",
                table: "uf_StoreProductLocations",
                column: "StoreLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Stores_DateCreatedUtc",
                table: "uf_Stores",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Stores_DateLastModifiedUtc",
                table: "uf_Stores",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Stores_HouseholdId",
                table: "uf_Stores",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Stores_HouseholdId_Title",
                table: "uf_Stores",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_Tags_DateCreatedUtc",
                table: "uf_Tags",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Tags_DateLastModifiedUtc",
                table: "uf_Tags",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Tags_HouseholdId",
                table: "uf_Tags",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Tags_HouseholdId_Key",
                table: "uf_Tags",
                columns: new[] { "HouseholdId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uf_Tags_Title",
                table: "uf_Tags",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uf_CookbookInvites");

            migrationBuilder.DropTable(
                name: "uf_CookbookMembers");

            migrationBuilder.DropTable(
                name: "uf_CookbookRecipes");

            migrationBuilder.DropTable(
                name: "uf_HouseholdInvites");

            migrationBuilder.DropTable(
                name: "uf_HouseholdMembers");

            migrationBuilder.DropTable(
                name: "uf_MealPrepPlanRecipes");

            migrationBuilder.DropTable(
                name: "uf_ProductBundleItems");

            migrationBuilder.DropTable(
                name: "uf_ProductImages");

            migrationBuilder.DropTable(
                name: "uf_ProductSubstitutions");

            migrationBuilder.DropTable(
                name: "uf_RecipeImages");

            migrationBuilder.DropTable(
                name: "uf_RecipeIngredients");

            migrationBuilder.DropTable(
                name: "uf_RecipeNotes");

            migrationBuilder.DropTable(
                name: "uf_RecipeSteps");

            migrationBuilder.DropTable(
                name: "uf_RecipeTags");

            migrationBuilder.DropTable(
                name: "uf_ShoppingListItems");

            migrationBuilder.DropTable(
                name: "uf_ShoppingListRecipeItems");

            migrationBuilder.DropTable(
                name: "uf_StoreProductLocations");

            migrationBuilder.DropTable(
                name: "uf_Cookbooks");

            migrationBuilder.DropTable(
                name: "uf_MealPrepPlanSlots");

            migrationBuilder.DropTable(
                name: "uf_ProductBundles");

            migrationBuilder.DropTable(
                name: "uf_RecipeIngredientGroups");

            migrationBuilder.DropTable(
                name: "uf_Tags");

            migrationBuilder.DropTable(
                name: "uf_ShoppingLists");

            migrationBuilder.DropTable(
                name: "uf_Products");

            migrationBuilder.DropTable(
                name: "uf_StoreLocations");

            migrationBuilder.DropTable(
                name: "uf_Recipes");

            migrationBuilder.DropTable(
                name: "uf_ProductCategories");

            migrationBuilder.DropTable(
                name: "uf_Stores");

            migrationBuilder.DropTable(
                name: "uf_Households");
        }
    }
}
