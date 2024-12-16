using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unshackled.Kitchen.Core.Data.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class v300 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "uk_Members",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AppTheme = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uk_Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uk_Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uk_Cookbooks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_Cookbooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_Cookbooks_uk_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uk_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_Households",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_Households", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_Households_uk_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uk_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_MemberMeta",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<long>(type: "INTEGER", maxLength: 450, nullable: false),
                    MetaKey = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    MetaValue = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_MemberMeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_MemberMeta_uk_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uk_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_RoleClaims_uk_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "uk_Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_UserClaims_uk_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "uk_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_uk_UserLogins_uk_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "uk_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_uk_UserRoles_uk_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "uk_Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_UserRoles_uk_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "uk_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_uk_UserTokens_uk_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "uk_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_CookbookInvites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CookbookId = table.Column<long>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Permissions = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_CookbookInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_CookbookInvites_uk_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "uk_Cookbooks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_CookbookMembers",
                columns: table => new
                {
                    CookbookId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false),
                    PermissionLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_CookbookMembers", x => new { x.CookbookId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_uk_CookbookMembers_uk_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "uk_Cookbooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_CookbookMembers_uk_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uk_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_HouseholdInvites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Permissions = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_HouseholdInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_HouseholdInvites_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_HouseholdMembers",
                columns: table => new
                {
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false),
                    PermissionLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_HouseholdMembers", x => new { x.HouseholdId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_uk_HouseholdMembers_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_HouseholdMembers_uk_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uk_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_ProductBundles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_ProductBundles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_ProductBundles_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_ProductCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_ProductCategories_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_Recipes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CookTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    PrepTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalServings = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_Recipes_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_Stores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_Stores_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_Tags_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Brand = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ProductCategoryId = table.Column<long>(type: "INTEGER", nullable: true),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPinned = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasNutritionInfo = table.Column<bool>(type: "INTEGER", nullable: false),
                    ServingSize = table.Column<decimal>(type: "TEXT", precision: 8, scale: 3, nullable: false),
                    ServingSizeN = table.Column<decimal>(type: "TEXT", precision: 13, scale: 6, nullable: false),
                    ServingSizeUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ServingSizeUnitLabel = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
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
                    table.PrimaryKey("PK_uk_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_Products_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_Products_uk_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "uk_ProductCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_CookbookRecipes",
                columns: table => new
                {
                    CookbookId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_CookbookRecipes", x => new { x.CookbookId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_uk_CookbookRecipes_uk_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "uk_Cookbooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_CookbookRecipes_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_CookbookRecipes_uk_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uk_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_CookbookRecipes_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_RecipeIngredientGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_RecipeIngredientGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_RecipeIngredientGroups_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeIngredientGroups_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_RecipeNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_RecipeNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_RecipeNotes_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeNotes_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_RecipeSteps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Instructions = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_RecipeSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_RecipeSteps_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeSteps_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    StoreId = table.Column<long>(type: "INTEGER", nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_ShoppingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_ShoppingLists_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_ShoppingLists_uk_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "uk_Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_StoreLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StoreId = table.Column<long>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_StoreLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_StoreLocations_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_StoreLocations_uk_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "uk_Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_RecipeTags",
                columns: table => new
                {
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    TagId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_RecipeTags", x => new { x.RecipeId, x.TagId });
                    table.ForeignKey(
                        name: "FK_uk_RecipeTags_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeTags_uk_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "uk_Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_ProductBundleItems",
                columns: table => new
                {
                    ProductBundleId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_ProductBundleItems", x => new { x.ProductBundleId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_uk_ProductBundleItems_uk_ProductBundles_ProductBundleId",
                        column: x => x.ProductBundleId,
                        principalTable: "uk_ProductBundles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_ProductBundleItems_uk_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uk_Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_ProductSubstitutions",
                columns: table => new
                {
                    IngredientKey = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_ProductSubstitutions", x => new { x.IngredientKey, x.ProductId });
                    table.ForeignKey(
                        name: "FK_uk_ProductSubstitutions_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_ProductSubstitutions_uk_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uk_Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_RecipeIngredients",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ListGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 8, scale: 3, nullable: false),
                    AmountN = table.Column<decimal>(type: "TEXT", precision: 15, scale: 3, nullable: false),
                    AmountUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountLabel = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    AmountText = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    PrepNote = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uk_RecipeIngredients_uk_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "uk_Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeIngredients_uk_RecipeIngredientGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "uk_RecipeIngredientGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_RecipeIngredients_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_ShoppingListItems",
                columns: table => new
                {
                    ShoppingListId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInCart = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_ShoppingListItems", x => new { x.ShoppingListId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_uk_ShoppingListItems_uk_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uk_Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_ShoppingListItems_uk_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "uk_ShoppingLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_ShoppingListRecipeItems",
                columns: table => new
                {
                    ShoppingListId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    IngredientKey = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 8, scale: 3, nullable: false),
                    UnitLabel = table.Column<string>(type: "TEXT", nullable: false),
                    PortionUsed = table.Column<decimal>(type: "TEXT", precision: 15, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_ShoppingListRecipeItems", x => new { x.ShoppingListId, x.ProductId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_uk_ShoppingListRecipeItems_uk_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uk_Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_ShoppingListRecipeItems_uk_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "uk_Recipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_ShoppingListRecipeItems_uk_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "uk_ShoppingLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_StoreProductLocations",
                columns: table => new
                {
                    StoreId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    StoreLocationId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uk_StoreProductLocations", x => new { x.StoreId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_uk_StoreProductLocations_uk_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "uk_Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_StoreProductLocations_uk_StoreLocations_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "uk_StoreLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uk_StoreProductLocations_uk_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "uk_Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uk_RecipeStepIngredients",
                columns: table => new
                {
                    RecipeStepId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeIngredientId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false)
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
                name: "IX_uk_CookbookInvites_CookbookId",
                table: "uk_CookbookInvites",
                column: "CookbookId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_CookbookInvites_DateCreatedUtc",
                table: "uk_CookbookInvites",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_CookbookInvites_DateLastModifiedUtc",
                table: "uk_CookbookInvites",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_CookbookMembers_MemberId",
                table: "uk_CookbookMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_CookbookRecipes_HouseholdId",
                table: "uk_CookbookRecipes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_CookbookRecipes_MemberId",
                table: "uk_CookbookRecipes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_CookbookRecipes_RecipeId",
                table: "uk_CookbookRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Cookbooks_DateCreatedUtc",
                table: "uk_Cookbooks",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Cookbooks_DateLastModifiedUtc",
                table: "uk_Cookbooks",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Cookbooks_MemberId",
                table: "uk_Cookbooks",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_HouseholdInvites_DateCreatedUtc",
                table: "uk_HouseholdInvites",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_HouseholdInvites_DateLastModifiedUtc",
                table: "uk_HouseholdInvites",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_HouseholdInvites_HouseholdId",
                table: "uk_HouseholdInvites",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_HouseholdMembers_MemberId",
                table: "uk_HouseholdMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Households_DateCreatedUtc",
                table: "uk_Households",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Households_DateLastModifiedUtc",
                table: "uk_Households",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Households_MemberId",
                table: "uk_Households",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_MemberMeta_MemberId_MetaKey",
                table: "uk_MemberMeta",
                columns: new[] { "MemberId", "MetaKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uk_Members_DateCreatedUtc",
                table: "uk_Members",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Members_DateLastModifiedUtc",
                table: "uk_Members",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Members_Email",
                table: "uk_Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductBundleItems_ProductId",
                table: "uk_ProductBundleItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductBundles_DateCreatedUtc",
                table: "uk_ProductBundles",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductBundles_DateLastModifiedUtc",
                table: "uk_ProductBundles",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductBundles_HouseholdId",
                table: "uk_ProductBundles",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductBundles_HouseholdId_Title",
                table: "uk_ProductBundles",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductCategories_DateCreatedUtc",
                table: "uk_ProductCategories",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductCategories_DateLastModifiedUtc",
                table: "uk_ProductCategories",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductCategories_HouseholdId",
                table: "uk_ProductCategories",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductCategories_Title",
                table: "uk_ProductCategories",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Products_DateCreatedUtc",
                table: "uk_Products",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Products_DateLastModifiedUtc",
                table: "uk_Products",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Products_HouseholdId",
                table: "uk_Products",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Products_HouseholdId_Title",
                table: "uk_Products",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_Products_ProductCategoryId",
                table: "uk_Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductSubstitutions_HouseholdId_IngredientKey",
                table: "uk_ProductSubstitutions",
                columns: new[] { "HouseholdId", "IngredientKey" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_ProductSubstitutions_ProductId",
                table: "uk_ProductSubstitutions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredientGroups_DateCreatedUtc",
                table: "uk_RecipeIngredientGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredientGroups_DateLastModifiedUtc",
                table: "uk_RecipeIngredientGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredientGroups_HouseholdId",
                table: "uk_RecipeIngredientGroups",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredientGroups_RecipeId_SortOrder",
                table: "uk_RecipeIngredientGroups",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredients_DateCreatedUtc",
                table: "uk_RecipeIngredients",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredients_DateLastModifiedUtc",
                table: "uk_RecipeIngredients",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredients_HouseholdId",
                table: "uk_RecipeIngredients",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredients_HouseholdId_Key",
                table: "uk_RecipeIngredients",
                columns: new[] { "HouseholdId", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredients_ListGroupId",
                table: "uk_RecipeIngredients",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeIngredients_RecipeId_SortOrder",
                table: "uk_RecipeIngredients",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeNotes_DateCreatedUtc",
                table: "uk_RecipeNotes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeNotes_DateLastModifiedUtc",
                table: "uk_RecipeNotes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeNotes_HouseholdId",
                table: "uk_RecipeNotes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeNotes_RecipeId_SortOrder",
                table: "uk_RecipeNotes",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_Recipes_DateCreatedUtc",
                table: "uk_Recipes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Recipes_DateLastModifiedUtc",
                table: "uk_Recipes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Recipes_HouseholdId",
                table: "uk_Recipes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Recipes_HouseholdId_Title",
                table: "uk_Recipes",
                columns: new[] { "HouseholdId", "Title" });

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

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeSteps_DateCreatedUtc",
                table: "uk_RecipeSteps",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeSteps_DateLastModifiedUtc",
                table: "uk_RecipeSteps",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeSteps_HouseholdId",
                table: "uk_RecipeSteps",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeSteps_RecipeId_SortOrder",
                table: "uk_RecipeSteps",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_RecipeTags_TagId",
                table: "uk_RecipeTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_RoleClaims_RoleId",
                table: "uk_RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "uk_Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uk_ShoppingListItems_ProductId",
                table: "uk_ShoppingListItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ShoppingListRecipeItems_ProductId",
                table: "uk_ShoppingListRecipeItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ShoppingListRecipeItems_RecipeId",
                table: "uk_ShoppingListRecipeItems",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ShoppingLists_DateCreatedUtc",
                table: "uk_ShoppingLists",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ShoppingLists_DateLastModifiedUtc",
                table: "uk_ShoppingLists",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ShoppingLists_HouseholdId",
                table: "uk_ShoppingLists",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_ShoppingLists_HouseholdId_Title",
                table: "uk_ShoppingLists",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_ShoppingLists_StoreId",
                table: "uk_ShoppingLists",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_StoreLocations_DateCreatedUtc",
                table: "uk_StoreLocations",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_StoreLocations_DateLastModifiedUtc",
                table: "uk_StoreLocations",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_StoreLocations_HouseholdId",
                table: "uk_StoreLocations",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_StoreLocations_HouseholdId_StoreId_SortOrder",
                table: "uk_StoreLocations",
                columns: new[] { "HouseholdId", "StoreId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_StoreLocations_StoreId",
                table: "uk_StoreLocations",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_StoreProductLocations_ProductId",
                table: "uk_StoreProductLocations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_StoreProductLocations_StoreId_StoreLocationId_SortOrder",
                table: "uk_StoreProductLocations",
                columns: new[] { "StoreId", "StoreLocationId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_StoreProductLocations_StoreLocationId",
                table: "uk_StoreProductLocations",
                column: "StoreLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Stores_DateCreatedUtc",
                table: "uk_Stores",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Stores_DateLastModifiedUtc",
                table: "uk_Stores",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Stores_HouseholdId",
                table: "uk_Stores",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Stores_HouseholdId_Title",
                table: "uk_Stores",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uk_Tags_DateCreatedUtc",
                table: "uk_Tags",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Tags_DateLastModifiedUtc",
                table: "uk_Tags",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Tags_HouseholdId",
                table: "uk_Tags",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_Tags_HouseholdId_Key",
                table: "uk_Tags",
                columns: new[] { "HouseholdId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uk_Tags_Title",
                table: "uk_Tags",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_uk_UserClaims_UserId",
                table: "uk_UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_UserLogins_UserId",
                table: "uk_UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_uk_UserRoles_RoleId",
                table: "uk_UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "uk_Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "uk_Users",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uk_CookbookInvites");

            migrationBuilder.DropTable(
                name: "uk_CookbookMembers");

            migrationBuilder.DropTable(
                name: "uk_CookbookRecipes");

            migrationBuilder.DropTable(
                name: "uk_HouseholdInvites");

            migrationBuilder.DropTable(
                name: "uk_HouseholdMembers");

            migrationBuilder.DropTable(
                name: "uk_MemberMeta");

            migrationBuilder.DropTable(
                name: "uk_ProductBundleItems");

            migrationBuilder.DropTable(
                name: "uk_ProductSubstitutions");

            migrationBuilder.DropTable(
                name: "uk_RecipeNotes");

            migrationBuilder.DropTable(
                name: "uk_RecipeStepIngredients");

            migrationBuilder.DropTable(
                name: "uk_RecipeTags");

            migrationBuilder.DropTable(
                name: "uk_RoleClaims");

            migrationBuilder.DropTable(
                name: "uk_ShoppingListItems");

            migrationBuilder.DropTable(
                name: "uk_ShoppingListRecipeItems");

            migrationBuilder.DropTable(
                name: "uk_StoreProductLocations");

            migrationBuilder.DropTable(
                name: "uk_UserClaims");

            migrationBuilder.DropTable(
                name: "uk_UserLogins");

            migrationBuilder.DropTable(
                name: "uk_UserRoles");

            migrationBuilder.DropTable(
                name: "uk_UserTokens");

            migrationBuilder.DropTable(
                name: "uk_Cookbooks");

            migrationBuilder.DropTable(
                name: "uk_ProductBundles");

            migrationBuilder.DropTable(
                name: "uk_RecipeIngredients");

            migrationBuilder.DropTable(
                name: "uk_RecipeSteps");

            migrationBuilder.DropTable(
                name: "uk_Tags");

            migrationBuilder.DropTable(
                name: "uk_ShoppingLists");

            migrationBuilder.DropTable(
                name: "uk_Products");

            migrationBuilder.DropTable(
                name: "uk_StoreLocations");

            migrationBuilder.DropTable(
                name: "uk_Roles");

            migrationBuilder.DropTable(
                name: "uk_Users");

            migrationBuilder.DropTable(
                name: "uk_RecipeIngredientGroups");

            migrationBuilder.DropTable(
                name: "uk_ProductCategories");

            migrationBuilder.DropTable(
                name: "uk_Stores");

            migrationBuilder.DropTable(
                name: "uk_Recipes");

            migrationBuilder.DropTable(
                name: "uk_Households");

            migrationBuilder.DropTable(
                name: "uk_Members");
        }
    }
}
