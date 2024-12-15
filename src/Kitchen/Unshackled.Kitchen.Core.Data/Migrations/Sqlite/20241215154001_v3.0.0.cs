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
                name: "Members",
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
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cookbooks",
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
                    table.PrimaryKey("PK_Cookbooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cookbooks_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Households",
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
                    table.PrimaryKey("PK_Households", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Households_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemberMeta",
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
                    table.PrimaryKey("PK_MemberMeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberMeta_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
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
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
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
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CookbookInvites",
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
                    table.PrimaryKey("PK_CookbookInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CookbookInvites_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "Cookbooks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CookbookMembers",
                columns: table => new
                {
                    CookbookId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false),
                    PermissionLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookbookMembers", x => new { x.CookbookId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_CookbookMembers_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "Cookbooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CookbookMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HouseholdInvites",
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
                    table.PrimaryKey("PK_HouseholdInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseholdInvites_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HouseholdMembers",
                columns: table => new
                {
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false),
                    PermissionLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdMembers", x => new { x.HouseholdId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_HouseholdMembers_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HouseholdMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductBundles",
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
                    table.PrimaryKey("PK_ProductBundles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBundles_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
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
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
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
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
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
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
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
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
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
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CookbookRecipes",
                columns: table => new
                {
                    CookbookId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookbookRecipes", x => new { x.CookbookId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_CookbookRecipes_Cookbooks_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "Cookbooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CookbookRecipes_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CookbookRecipes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CookbookRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredientGroups",
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
                    table.PrimaryKey("PK_RecipeIngredientGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredientGroups_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeIngredientGroups_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeNotes",
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
                    table.PrimaryKey("PK_RecipeNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeNotes_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeNotes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
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
                    table.PrimaryKey("PK_RecipeSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
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
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingLists_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreLocations",
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
                    table.PrimaryKey("PK_StoreLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreLocations_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreLocations_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeTags",
                columns: table => new
                {
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false),
                    TagId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTags", x => new { x.RecipeId, x.TagId });
                    table.ForeignKey(
                        name: "FK_RecipeTags_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductBundleItems",
                columns: table => new
                {
                    ProductBundleId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBundleItems", x => new { x.ProductBundleId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductBundleItems_ProductBundles_ProductBundleId",
                        column: x => x.ProductBundleId,
                        principalTable: "ProductBundles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductBundleItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductSubstitutions",
                columns: table => new
                {
                    IngredientKey = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    HouseholdId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubstitutions", x => new { x.IngredientKey, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductSubstitutions_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductSubstitutions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
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
                    table.PrimaryKey("PK_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_RecipeIngredientGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "RecipeIngredientGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListItems",
                columns: table => new
                {
                    ShoppingListId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInCart = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItems", x => new { x.ShoppingListId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListRecipeItems",
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
                    table.PrimaryKey("PK_ShoppingListRecipeItems", x => new { x.ShoppingListId, x.ProductId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_ShoppingListRecipeItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingListRecipeItems_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingListRecipeItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreProductLocations",
                columns: table => new
                {
                    StoreId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    StoreLocationId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProductLocations", x => new { x.StoreId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_StoreProductLocations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreProductLocations_StoreLocations_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "StoreLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreProductLocations_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeStepIngredients",
                columns: table => new
                {
                    RecipeStepId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeIngredientId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeStepIngredients", x => new { x.RecipeStepId, x.RecipeIngredientId });
                    table.ForeignKey(
                        name: "FK_RecipeStepIngredients_RecipeIngredients_RecipeIngredientId",
                        column: x => x.RecipeIngredientId,
                        principalTable: "RecipeIngredients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeStepIngredients_RecipeSteps_RecipeStepId",
                        column: x => x.RecipeStepId,
                        principalTable: "RecipeSteps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeStepIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CookbookInvites_CookbookId",
                table: "CookbookInvites",
                column: "CookbookId");

            migrationBuilder.CreateIndex(
                name: "IX_CookbookInvites_DateCreatedUtc",
                table: "CookbookInvites",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CookbookInvites_DateLastModifiedUtc",
                table: "CookbookInvites",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CookbookMembers_MemberId",
                table: "CookbookMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CookbookRecipes_HouseholdId",
                table: "CookbookRecipes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_CookbookRecipes_MemberId",
                table: "CookbookRecipes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CookbookRecipes_RecipeId",
                table: "CookbookRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cookbooks_DateCreatedUtc",
                table: "Cookbooks",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Cookbooks_DateLastModifiedUtc",
                table: "Cookbooks",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Cookbooks_MemberId",
                table: "Cookbooks",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdInvites_DateCreatedUtc",
                table: "HouseholdInvites",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdInvites_DateLastModifiedUtc",
                table: "HouseholdInvites",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdInvites_HouseholdId",
                table: "HouseholdInvites",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdMembers_MemberId",
                table: "HouseholdMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Households_DateCreatedUtc",
                table: "Households",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Households_DateLastModifiedUtc",
                table: "Households",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Households_MemberId",
                table: "Households",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberMeta_MemberId_MetaKey",
                table: "MemberMeta",
                columns: new[] { "MemberId", "MetaKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_DateCreatedUtc",
                table: "Members",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Members_DateLastModifiedUtc",
                table: "Members",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBundleItems_ProductId",
                table: "ProductBundleItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBundles_DateCreatedUtc",
                table: "ProductBundles",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBundles_DateLastModifiedUtc",
                table: "ProductBundles",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBundles_HouseholdId",
                table: "ProductBundles",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBundles_HouseholdId_Title",
                table: "ProductBundles",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_DateCreatedUtc",
                table: "ProductCategories",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_DateLastModifiedUtc",
                table: "ProductCategories",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_HouseholdId",
                table: "ProductCategories",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Title",
                table: "ProductCategories",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DateCreatedUtc",
                table: "Products",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DateLastModifiedUtc",
                table: "Products",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Products_HouseholdId",
                table: "Products",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_HouseholdId_Title",
                table: "Products",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubstitutions_HouseholdId_IngredientKey",
                table: "ProductSubstitutions",
                columns: new[] { "HouseholdId", "IngredientKey" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubstitutions_ProductId",
                table: "ProductSubstitutions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredientGroups_DateCreatedUtc",
                table: "RecipeIngredientGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredientGroups_DateLastModifiedUtc",
                table: "RecipeIngredientGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredientGroups_HouseholdId",
                table: "RecipeIngredientGroups",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredientGroups_RecipeId_SortOrder",
                table: "RecipeIngredientGroups",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_DateCreatedUtc",
                table: "RecipeIngredients",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_DateLastModifiedUtc",
                table: "RecipeIngredients",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_HouseholdId",
                table: "RecipeIngredients",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_HouseholdId_Key",
                table: "RecipeIngredients",
                columns: new[] { "HouseholdId", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_ListGroupId",
                table: "RecipeIngredients",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId_SortOrder",
                table: "RecipeIngredients",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeNotes_DateCreatedUtc",
                table: "RecipeNotes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeNotes_DateLastModifiedUtc",
                table: "RecipeNotes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeNotes_HouseholdId",
                table: "RecipeNotes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeNotes_RecipeId_SortOrder",
                table: "RecipeNotes",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_DateCreatedUtc",
                table: "Recipes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_DateLastModifiedUtc",
                table: "Recipes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_HouseholdId",
                table: "Recipes",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_HouseholdId_Title",
                table: "Recipes",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeStepIngredients_RecipeId",
                table: "RecipeStepIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeStepIngredients_RecipeIngredientId",
                table: "RecipeStepIngredients",
                column: "RecipeIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeStepIngredients_RecipeStepId",
                table: "RecipeStepIngredients",
                column: "RecipeStepId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_DateCreatedUtc",
                table: "RecipeSteps",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_DateLastModifiedUtc",
                table: "RecipeSteps",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_HouseholdId",
                table: "RecipeSteps",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_RecipeId_SortOrder",
                table: "RecipeSteps",
                columns: new[] { "RecipeId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_TagId",
                table: "RecipeTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_ProductId",
                table: "ShoppingListItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListRecipeItems_ProductId",
                table: "ShoppingListRecipeItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListRecipeItems_RecipeId",
                table: "ShoppingListRecipeItems",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_DateCreatedUtc",
                table: "ShoppingLists",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_DateLastModifiedUtc",
                table: "ShoppingLists",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_HouseholdId",
                table: "ShoppingLists",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_HouseholdId_Title",
                table: "ShoppingLists",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_StoreId",
                table: "ShoppingLists",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_DateCreatedUtc",
                table: "StoreLocations",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_DateLastModifiedUtc",
                table: "StoreLocations",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_HouseholdId",
                table: "StoreLocations",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_HouseholdId_StoreId_SortOrder",
                table: "StoreLocations",
                columns: new[] { "HouseholdId", "StoreId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_StoreId",
                table: "StoreLocations",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductLocations_ProductId",
                table: "StoreProductLocations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductLocations_StoreId_StoreLocationId_SortOrder",
                table: "StoreProductLocations",
                columns: new[] { "StoreId", "StoreLocationId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductLocations_StoreLocationId",
                table: "StoreProductLocations",
                column: "StoreLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_DateCreatedUtc",
                table: "Stores",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_DateLastModifiedUtc",
                table: "Stores",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_HouseholdId",
                table: "Stores",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_HouseholdId_Title",
                table: "Stores",
                columns: new[] { "HouseholdId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_DateCreatedUtc",
                table: "Tags",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_DateLastModifiedUtc",
                table: "Tags",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_HouseholdId",
                table: "Tags",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_HouseholdId_Key",
                table: "Tags",
                columns: new[] { "HouseholdId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Title",
                table: "Tags",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CookbookInvites");

            migrationBuilder.DropTable(
                name: "CookbookMembers");

            migrationBuilder.DropTable(
                name: "CookbookRecipes");

            migrationBuilder.DropTable(
                name: "HouseholdInvites");

            migrationBuilder.DropTable(
                name: "HouseholdMembers");

            migrationBuilder.DropTable(
                name: "MemberMeta");

            migrationBuilder.DropTable(
                name: "ProductBundleItems");

            migrationBuilder.DropTable(
                name: "ProductSubstitutions");

            migrationBuilder.DropTable(
                name: "RecipeNotes");

            migrationBuilder.DropTable(
                name: "RecipeStepIngredients");

            migrationBuilder.DropTable(
                name: "RecipeTags");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "ShoppingListRecipeItems");

            migrationBuilder.DropTable(
                name: "StoreProductLocations");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Cookbooks");

            migrationBuilder.DropTable(
                name: "ProductBundles");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "RecipeSteps");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "StoreLocations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RecipeIngredientGroups");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Households");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
