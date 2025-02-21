using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Unshackled.Fitness.Core.Data.Migrations.PostgreSQL
{
    /// <inheritdoc />
    public partial class v330 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:case_insensitive_collation", "en-u-ks-primary,en-u-ks-primary,icu,False");

            migrationBuilder.CreateTable(
                name: "uf_DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    Xml = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uf_Members",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, collation: "case_insensitive_collation"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AppTheme = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uf_Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, collation: "case_insensitive_collation"),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, collation: "case_insensitive_collation"),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uf_Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, collation: "case_insensitive_collation"),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, collation: "case_insensitive_collation"),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, collation: "case_insensitive_collation"),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, collation: "case_insensitive_collation"),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uf_ActivityTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Color = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, collation: "case_insensitive_collation"),
                    DefaultEventType = table.Column<int>(type: "integer", nullable: false),
                    DefaultDistanceUnits = table.Column<int>(type: "integer", nullable: false),
                    DefaultElevationUnits = table.Column<int>(type: "integer", nullable: false),
                    DefaultSpeedUnits = table.Column<int>(type: "integer", nullable: false),
                    DefaultCadenceUnits = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ActivityTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_ActivityTypes_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Cookbooks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
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
                name: "uf_Exercises",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Muscles = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    Equipment = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    DefaultSetMetricType = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    Notes = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    DefaultSetType = table.Column<int>(type: "integer", nullable: false),
                    IsTrackingSplit = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Exercises_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Households",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
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
                name: "uf_MemberMeta",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MemberId = table.Column<long>(type: "bigint", maxLength: 450, nullable: false),
                    MetaKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    MetaValue = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_MemberMeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_MemberMeta_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_MetricDefinitionGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_MetricDefinitionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_MetricDefinitionGroups_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_MetricPresets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Settings = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_MetricPresets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_MetricPresets_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Programs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    ProgramType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    LengthWeeks = table.Column<int>(type: "integer", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateLastWorkoutUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NextTemplateIndex = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Programs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Programs_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_TrainingPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    ProgramType = table.Column<int>(type: "integer", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateLastActivityUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    LengthWeeks = table.Column<int>(type: "integer", nullable: false),
                    NextSessionIndex = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_TrainingPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_TrainingPlans_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Workouts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    DateStarted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateStartedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateCompletedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MusclesTargeted = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    ExerciseCount = table.Column<int>(type: "integer", nullable: false),
                    SetCount = table.Column<int>(type: "integer", nullable: false),
                    RepCount = table.Column<int>(type: "integer", nullable: false),
                    VolumeKg = table.Column<decimal>(type: "numeric(12,3)", precision: 12, scale: 3, nullable: false),
                    VolumeLb = table.Column<decimal>(type: "numeric(12,3)", precision: 12, scale: 3, nullable: false),
                    WorkoutTemplateId = table.Column<long>(type: "bigint", nullable: true),
                    RecordRepsCount = table.Column<int>(type: "integer", nullable: false),
                    RecordSecondsCount = table.Column<int>(type: "integer", nullable: false),
                    RecordSecondsAtWeightCount = table.Column<int>(type: "integer", nullable: false),
                    RecordTargetVolumeCount = table.Column<int>(type: "integer", nullable: false),
                    RecordTargetWeightCount = table.Column<int>(type: "integer", nullable: false),
                    RecordVolumeCount = table.Column<int>(type: "integer", nullable: false),
                    RecordWeightCount = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Workouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Workouts_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_WorkoutTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    ExerciseCount = table.Column<int>(type: "integer", nullable: false),
                    MusclesTargeted = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    SetCount = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_WorkoutTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplates_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    ClaimType = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    ClaimValue = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_RoleClaims_uf_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "uf_Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    ClaimType = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    ClaimValue = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_UserClaims_uf_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "uf_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    ProviderKey = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    UserId = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_uf_UserLogins_uf_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "uf_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    RoleId = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_uf_UserRoles_uf_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "uf_Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_UserRoles_uf_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "uf_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    LoginProvider = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    Name = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    Value = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_uf_UserTokens_uf_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "uf_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Activities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityTypeId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingSessionId = table.Column<long>(type: "bigint", nullable: true),
                    AverageCadence = table.Column<double>(type: "double precision", nullable: true),
                    AverageCadenceUnit = table.Column<int>(type: "integer", nullable: false),
                    AverageHeartRateBpm = table.Column<int>(type: "integer", nullable: true),
                    AveragePace = table.Column<int>(type: "integer", nullable: true),
                    AveragePower = table.Column<double>(type: "double precision", nullable: true),
                    AverageSpeed = table.Column<double>(type: "double precision", nullable: true),
                    AverageSpeedN = table.Column<double>(type: "double precision", nullable: true),
                    AverageSpeedUnit = table.Column<int>(type: "integer", nullable: false),
                    DateEvent = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateEventUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    MaximumAltitude = table.Column<double>(type: "double precision", nullable: true),
                    MaximumAltitudeN = table.Column<double>(type: "double precision", nullable: true),
                    MaximumAltitudeUnit = table.Column<int>(type: "integer", nullable: false),
                    MaximumCadence = table.Column<double>(type: "double precision", nullable: true),
                    MaximumCadenceUnit = table.Column<int>(type: "integer", nullable: false),
                    MaximumHeartRateBpm = table.Column<int>(type: "integer", nullable: true),
                    MaximumPace = table.Column<int>(type: "integer", nullable: true),
                    MaximumPower = table.Column<double>(type: "double precision", nullable: true),
                    MaximumSpeed = table.Column<double>(type: "double precision", nullable: true),
                    MaximumSpeedN = table.Column<double>(type: "double precision", nullable: true),
                    MaximumSpeedUnit = table.Column<int>(type: "integer", nullable: false),
                    MinimumAltitude = table.Column<double>(type: "double precision", nullable: true),
                    MinimumAltitudeN = table.Column<double>(type: "double precision", nullable: true),
                    MinimumAltitudeUnit = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    TargetCadence = table.Column<double>(type: "double precision", nullable: true),
                    TargetCadenceUnit = table.Column<int>(type: "integer", nullable: false),
                    TargetCalories = table.Column<int>(type: "integer", nullable: true),
                    TargetDistance = table.Column<double>(type: "double precision", nullable: true),
                    TargetDistanceN = table.Column<double>(type: "double precision", nullable: true),
                    TargetDistanceUnit = table.Column<int>(type: "integer", nullable: false),
                    TargetHeartRateBpm = table.Column<int>(type: "integer", nullable: true),
                    TargetPace = table.Column<int>(type: "integer", nullable: true),
                    TargetPower = table.Column<double>(type: "double precision", nullable: true),
                    TargetTimeSeconds = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    TotalAscent = table.Column<double>(type: "double precision", nullable: true),
                    TotalAscentN = table.Column<double>(type: "double precision", nullable: true),
                    TotalAscentUnit = table.Column<int>(type: "integer", nullable: false),
                    TotalCalories = table.Column<int>(type: "integer", nullable: true),
                    TotalDescent = table.Column<double>(type: "double precision", nullable: true),
                    TotalDescentN = table.Column<double>(type: "double precision", nullable: true),
                    TotalDescentUnit = table.Column<int>(type: "integer", nullable: false),
                    TotalDistance = table.Column<double>(type: "double precision", nullable: true),
                    TotalDistanceUnit = table.Column<int>(type: "integer", nullable: false),
                    TotalDistanceN = table.Column<double>(type: "double precision", nullable: true),
                    TotalTimeSeconds = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Activities_uf_ActivityTypes_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "uf_ActivityTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_Activities_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_TrainingSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    TargetCadence = table.Column<double>(type: "double precision", nullable: true),
                    TargetCadenceUnit = table.Column<int>(type: "integer", nullable: false),
                    TargetCalories = table.Column<int>(type: "integer", nullable: true),
                    TargetDistance = table.Column<double>(type: "double precision", nullable: true),
                    TargetDistanceN = table.Column<double>(type: "double precision", nullable: true),
                    TargetDistanceUnit = table.Column<int>(type: "integer", nullable: false),
                    TargetHeartRateBpm = table.Column<int>(type: "integer", nullable: true),
                    TargetPace = table.Column<int>(type: "integer", nullable: true),
                    TargetPower = table.Column<double>(type: "double precision", nullable: true),
                    TargetTimeSeconds = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_TrainingSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_TrainingSessions_uf_ActivityTypes_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "uf_ActivityTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_TrainingSessions_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_CookbookInvites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CookbookId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Permissions = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
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
                    CookbookId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionLevel = table.Column<int>(type: "integer", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Permissions = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionLevel = table.Column<int>(type: "integer", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_insensitive_collation"),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    CookTimeMinutes = table.Column<int>(type: "integer", nullable: false),
                    PrepTimeMinutes = table.Column<int>(type: "integer", nullable: false),
                    TotalServings = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                name: "uf_MetricDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_insensitive_collation"),
                    SubTitle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, collation: "case_insensitive_collation"),
                    MetricType = table.Column<int>(type: "integer", nullable: false),
                    ListGroupId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    HighlightColor = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, collation: "case_insensitive_collation"),
                    MaxValue = table.Column<decimal>(type: "numeric(2,0)", precision: 2, scale: 0, nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    IsOnDashboard = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_MetricDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_MetricDefinitions_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_MetricDefinitions_uf_MetricDefinitionGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "uf_MetricDefinitionGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_WorkoutSetGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_WorkoutSetGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_WorkoutSetGroups_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutSetGroups_uf_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "uf_Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_WorkoutTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_WorkoutTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTasks_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTasks_uf_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "uf_Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ProgramTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<long>(type: "bigint", nullable: false),
                    WorkoutTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    WeekNumber = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_ProgramTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_ProgramTemplates_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ProgramTemplates_uf_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "uf_Programs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_ProgramTemplates_uf_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "uf_WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_WorkoutTemplateSetGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_WorkoutTemplateSetGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateSetGroups_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateSetGroups_uf_WorkoutTemplates_WorkoutTemp~",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "uf_WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_WorkoutTemplateTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_WorkoutTemplateTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateTasks_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateTasks_uf_WorkoutTemplates_WorkoutTemplate~",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "uf_WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_TrainingPlanSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingPlanId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingSessionId = table.Column<long>(type: "bigint", nullable: false),
                    WeekNumber = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_TrainingPlanSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_TrainingPlanSessions_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_TrainingPlanSessions_uf_TrainingPlans_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "uf_TrainingPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_TrainingPlanSessions_uf_TrainingSessions_TrainingSession~",
                        column: x => x.TrainingSessionId,
                        principalTable: "uf_TrainingSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Brand = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "case_insensitive_collation"),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "case_insensitive_collation"),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    IsAutoSkipped = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    HasNutritionInfo = table.Column<bool>(type: "boolean", nullable: false),
                    ServingSize = table.Column<decimal>(type: "numeric(8,3)", precision: 8, scale: 3, nullable: false),
                    ServingSizeN = table.Column<decimal>(type: "numeric(13,6)", precision: 13, scale: 6, nullable: false),
                    ServingSizeUnit = table.Column<int>(type: "integer", nullable: false),
                    ServingSizeUnitLabel = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, collation: "case_insensitive_collation"),
                    ServingSizeMetric = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    ServingSizeMetricN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    ServingSizeMetricUnit = table.Column<int>(type: "integer", nullable: false),
                    ServingsPerContainer = table.Column<decimal>(type: "numeric(8,3)", precision: 8, scale: 3, nullable: false),
                    Calories = table.Column<int>(type: "integer", nullable: false),
                    CaloriesFromFat = table.Column<int>(type: "integer", nullable: false),
                    TotalFat = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    TotalFatUnit = table.Column<int>(type: "integer", nullable: false),
                    TotalFatN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    SaturatedFat = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    SaturatedFatUnit = table.Column<int>(type: "integer", nullable: false),
                    SaturatedFatN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    TransFat = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    TransFatUnit = table.Column<int>(type: "integer", nullable: false),
                    TransFatN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    PolyunsaturatedFat = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    PolyunsaturatedFatUnit = table.Column<int>(type: "integer", nullable: false),
                    PolyunsaturatedFatN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    MonounsaturatedFat = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    MonounsaturatedFatUnit = table.Column<int>(type: "integer", nullable: false),
                    MonounsaturatedFatN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Cholesterol = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    CholesterolUnit = table.Column<int>(type: "integer", nullable: false),
                    CholesterolN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    TotalCarbohydrates = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    TotalCarbohydratesUnit = table.Column<int>(type: "integer", nullable: false),
                    TotalCarbohydratesN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    DietaryFiber = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    DietaryFiberUnit = table.Column<int>(type: "integer", nullable: false),
                    DietaryFiberN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    SolubleFiber = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    SolubleFiberUnit = table.Column<int>(type: "integer", nullable: false),
                    SolubleFiberN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    InsolubleFiber = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    InsolubleFiberUnit = table.Column<int>(type: "integer", nullable: false),
                    InsolubleFiberN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    TotalSugars = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    TotalSugarsUnit = table.Column<int>(type: "integer", nullable: false),
                    TotalSugarsN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    AddedSugars = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    AddedSugarsUnit = table.Column<int>(type: "integer", nullable: false),
                    AddedSugarsN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    SugarAlcohols = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    SugarAlcoholsUnit = table.Column<int>(type: "integer", nullable: false),
                    SugarAlcoholsN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Protein = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    ProteinUnit = table.Column<int>(type: "integer", nullable: false),
                    ProteinN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Biotin = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    BiotinUnit = table.Column<int>(type: "integer", nullable: false),
                    BiotinN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Choline = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    CholineUnit = table.Column<int>(type: "integer", nullable: false),
                    CholineN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Folate = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    FolateUnit = table.Column<int>(type: "integer", nullable: false),
                    FolateN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Niacin = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    NiacinUnit = table.Column<int>(type: "integer", nullable: false),
                    NiacinN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    PantothenicAcid = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    PantothenicAcidUnit = table.Column<int>(type: "integer", nullable: false),
                    PantothenicAcidN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Riboflavin = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    RiboflavinUnit = table.Column<int>(type: "integer", nullable: false),
                    RiboflavinN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Thiamin = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    ThiaminUnit = table.Column<int>(type: "integer", nullable: false),
                    ThiaminN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    VitaminA = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    VitaminAUnit = table.Column<int>(type: "integer", nullable: false),
                    VitaminAN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    VitaminB6 = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    VitaminB6Unit = table.Column<int>(type: "integer", nullable: false),
                    VitaminB6N = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    VitaminB12 = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    VitaminB12Unit = table.Column<int>(type: "integer", nullable: false),
                    VitaminB12N = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    VitaminC = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    VitaminCUnit = table.Column<int>(type: "integer", nullable: false),
                    VitaminCN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    VitaminD = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    VitaminDUnit = table.Column<int>(type: "integer", nullable: false),
                    VitaminDN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    VitaminE = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    VitaminEUnit = table.Column<int>(type: "integer", nullable: false),
                    VitaminEN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    VitaminK = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    VitaminKUnit = table.Column<int>(type: "integer", nullable: false),
                    VitaminKN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Calcium = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    CalciumUnit = table.Column<int>(type: "integer", nullable: false),
                    CalciumN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Chloride = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    ChlorideUnit = table.Column<int>(type: "integer", nullable: false),
                    ChlorideN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Chromium = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    ChromiumUnit = table.Column<int>(type: "integer", nullable: false),
                    ChromiumN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Copper = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    CopperUnit = table.Column<int>(type: "integer", nullable: false),
                    CopperN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Iodine = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    IodineUnit = table.Column<int>(type: "integer", nullable: false),
                    IodineN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Iron = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    IronUnit = table.Column<int>(type: "integer", nullable: false),
                    IronN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Magnesium = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    MagnesiumUnit = table.Column<int>(type: "integer", nullable: false),
                    MagnesiumN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Manganese = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    ManganeseUnit = table.Column<int>(type: "integer", nullable: false),
                    ManganeseN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Molybdenum = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    MolybdenumUnit = table.Column<int>(type: "integer", nullable: false),
                    MolybdenumN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Phosphorus = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    PhosphorusUnit = table.Column<int>(type: "integer", nullable: false),
                    PhosphorusN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Potassium = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    PotassiumUnit = table.Column<int>(type: "integer", nullable: false),
                    PotassiumN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Selenium = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    SeleniumUnit = table.Column<int>(type: "integer", nullable: false),
                    SeleniumN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Sodium = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    SodiumUnit = table.Column<int>(type: "integer", nullable: false),
                    SodiumN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    Zinc = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    ZincUnit = table.Column<int>(type: "integer", nullable: false),
                    ZincN = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    CookbookId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DatePlanned = table.Column<DateOnly>(type: "date", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    SlotId = table.Column<long>(type: "bigint", nullable: true),
                    Scale = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    Container = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_insensitive_collation"),
                    RelativePath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    MimeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_insensitive_collation"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "case_insensitive_collation"),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false)
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
                name: "uf_Metrics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MetricDefinitionId = table.Column<long>(type: "bigint", nullable: false),
                    DateRecorded = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RecordedValue = table.Column<decimal>(type: "numeric(15,3)", precision: 15, scale: 3, nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Metrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_Metrics_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_Metrics_uf_MetricDefinitions_MetricDefinitionId",
                        column: x => x.MetricDefinitionId,
                        principalTable: "uf_MetricDefinitions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_WorkoutSets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutId = table.Column<long>(type: "bigint", nullable: false),
                    ExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    SetMetricType = table.Column<int>(type: "integer", nullable: false),
                    ListGroupId = table.Column<long>(type: "bigint", nullable: false),
                    IsTrackingSplit = table.Column<bool>(type: "boolean", nullable: false),
                    IntensityTarget = table.Column<int>(type: "integer", nullable: false),
                    RepMode = table.Column<int>(type: "integer", nullable: false),
                    RepsTarget = table.Column<int>(type: "integer", nullable: false),
                    Reps = table.Column<int>(type: "integer", nullable: false),
                    RepsLeft = table.Column<int>(type: "integer", nullable: false),
                    RepsRight = table.Column<int>(type: "integer", nullable: false),
                    SecondsTarget = table.Column<int>(type: "integer", nullable: false),
                    Seconds = table.Column<int>(type: "integer", nullable: false),
                    SecondsLeft = table.Column<int>(type: "integer", nullable: false),
                    SecondsRight = table.Column<int>(type: "integer", nullable: false),
                    SetType = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    WeightLb = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    VolumeLb = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: false),
                    VolumeKg = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: false),
                    DateRecorded = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateRecordedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsBestSetByReps = table.Column<bool>(type: "boolean", nullable: false),
                    IsBestSetBySeconds = table.Column<bool>(type: "boolean", nullable: false),
                    IsBestSetByWeight = table.Column<bool>(type: "boolean", nullable: false),
                    IsBestSetByVolume = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecordReps = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecordSeconds = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecordSecondsAtWeight = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecordTargetVolume = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecordTargetWeight = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecordVolume = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecordWeight = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_WorkoutSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_WorkoutSets_uf_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "uf_Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutSets_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutSets_uf_WorkoutSetGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "uf_WorkoutSetGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutSets_uf_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "uf_Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_WorkoutTemplateSets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    ListGroupId = table.Column<long>(type: "bigint", nullable: false),
                    ExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    SetMetricType = table.Column<int>(type: "integer", nullable: false),
                    SetType = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    RepMode = table.Column<int>(type: "integer", nullable: false),
                    RepsTarget = table.Column<int>(type: "integer", nullable: false),
                    SecondsTarget = table.Column<int>(type: "integer", nullable: false),
                    IntensityTarget = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_WorkoutTemplateSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateSets_uf_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "uf_Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateSets_uf_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "uf_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateSets_uf_WorkoutTemplateSetGroups_ListGrou~",
                        column: x => x.ListGroupId,
                        principalTable: "uf_WorkoutTemplateSetGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateSets_uf_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "uf_WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_ProductBundleItems",
                columns: table => new
                {
                    ProductBundleId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Container = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_insensitive_collation"),
                    RelativePath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    MimeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_insensitive_collation"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    IngredientKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "case_insensitive_collation"),
                    ListGroupId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(8,3)", precision: 8, scale: 3, nullable: false),
                    AmountN = table.Column<decimal>(type: "numeric(15,3)", precision: 15, scale: 3, nullable: false),
                    AmountUnit = table.Column<int>(type: "integer", nullable: false),
                    AmountLabel = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, collation: "case_insensitive_collation"),
                    AmountText = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false, collation: "case_insensitive_collation"),
                    PrepNote = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "case_insensitive_collation"),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
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
                    ShoppingListId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    IsInCart = table.Column<bool>(type: "boolean", nullable: false)
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
                    ShoppingListId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    InstanceId = table.Column<int>(type: "integer", nullable: false),
                    IngredientKey = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    IngredientAmount = table.Column<decimal>(type: "numeric(8,3)", precision: 8, scale: 3, nullable: false),
                    IngredientAmountUnitLabel = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive_collation"),
                    PortionUsed = table.Column<decimal>(type: "numeric(15,10)", precision: 15, scale: 10, nullable: false),
                    IngredientAmountUnitType = table.Column<int>(type: "integer", nullable: false),
                    ServingSizeUnitType = table.Column<int>(type: "integer", nullable: false),
                    IsUnitMismatch = table.Column<bool>(type: "boolean", nullable: false)
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
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    StoreLocationId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_uf_Activities_ActivityTypeId",
                table: "uf_Activities",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Activities_DateCreatedUtc",
                table: "uf_Activities",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Activities_DateLastModifiedUtc",
                table: "uf_Activities",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Activities_MemberId",
                table: "uf_Activities",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Activities_MemberId_DateEventUtc",
                table: "uf_Activities",
                columns: new[] { "MemberId", "DateEventUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_ActivityTypes_DateCreatedUtc",
                table: "uf_ActivityTypes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ActivityTypes_DateLastModifiedUtc",
                table: "uf_ActivityTypes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ActivityTypes_MemberId",
                table: "uf_ActivityTypes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ActivityTypes_MemberId_Title",
                table: "uf_ActivityTypes",
                columns: new[] { "MemberId", "Title" });

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
                name: "IX_uf_Exercises_DateCreatedUtc",
                table: "uf_Exercises",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Exercises_DateLastModifiedUtc",
                table: "uf_Exercises",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Exercises_MemberId",
                table: "uf_Exercises",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Exercises_MemberId_Title_IsArchived",
                table: "uf_Exercises",
                columns: new[] { "MemberId", "Title", "IsArchived" });

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
                name: "IX_uf_MemberMeta_MemberId_MetaKey",
                table: "uf_MemberMeta",
                columns: new[] { "MemberId", "MetaKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uf_Members_DateCreatedUtc",
                table: "uf_Members",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Members_DateLastModifiedUtc",
                table: "uf_Members",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Members_Email",
                table: "uf_Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitionGroups_DateCreatedUtc",
                table: "uf_MetricDefinitionGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitionGroups_DateLastModifiedUtc",
                table: "uf_MetricDefinitionGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitionGroups_MemberId",
                table: "uf_MetricDefinitionGroups",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitionGroups_MemberId_SortOrder",
                table: "uf_MetricDefinitionGroups",
                columns: new[] { "MemberId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitions_DateCreatedUtc",
                table: "uf_MetricDefinitions",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitions_DateLastModifiedUtc",
                table: "uf_MetricDefinitions",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitions_ListGroupId",
                table: "uf_MetricDefinitions",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitions_MemberId",
                table: "uf_MetricDefinitions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricDefinitions_MemberId_ListGroupId_SortOrder",
                table: "uf_MetricDefinitions",
                columns: new[] { "MemberId", "ListGroupId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricPresets_DateCreatedUtc",
                table: "uf_MetricPresets",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricPresets_DateLastModifiedUtc",
                table: "uf_MetricPresets",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricPresets_MemberId",
                table: "uf_MetricPresets",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_MetricPresets_MemberId_Title",
                table: "uf_MetricPresets",
                columns: new[] { "MemberId", "Title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uf_Metrics_DateCreatedUtc",
                table: "uf_Metrics",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Metrics_DateLastModifiedUtc",
                table: "uf_Metrics",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Metrics_MemberId",
                table: "uf_Metrics",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Metrics_MemberId_MetricDefinitionId_DateRecorded",
                table: "uf_Metrics",
                columns: new[] { "MemberId", "MetricDefinitionId", "DateRecorded" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_Metrics_MetricDefinitionId",
                table: "uf_Metrics",
                column: "MetricDefinitionId");

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
                name: "IX_uf_Programs_DateCreatedUtc",
                table: "uf_Programs",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Programs_DateLastModifiedUtc",
                table: "uf_Programs",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Programs_MemberId",
                table: "uf_Programs",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Programs_MemberId_Title",
                table: "uf_Programs",
                columns: new[] { "MemberId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProgramTemplates_DateCreatedUtc",
                table: "uf_ProgramTemplates",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProgramTemplates_DateLastModifiedUtc",
                table: "uf_ProgramTemplates",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProgramTemplates_MemberId",
                table: "uf_ProgramTemplates",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProgramTemplates_ProgramId_WeekNumber_DayNumber",
                table: "uf_ProgramTemplates",
                columns: new[] { "ProgramId", "WeekNumber", "DayNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_ProgramTemplates_WorkoutTemplateId",
                table: "uf_ProgramTemplates",
                column: "WorkoutTemplateId");

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
                name: "IX_uf_RoleClaims_RoleId",
                table: "uf_RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "uf_Roles",
                column: "NormalizedName",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlans_DateCreatedUtc",
                table: "uf_TrainingPlans",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlans_DateLastModifiedUtc",
                table: "uf_TrainingPlans",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlans_MemberId",
                table: "uf_TrainingPlans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlans_MemberId_DateStarted",
                table: "uf_TrainingPlans",
                columns: new[] { "MemberId", "DateStarted" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlans_MemberId_Title",
                table: "uf_TrainingPlans",
                columns: new[] { "MemberId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlanSessions_DateCreatedUtc",
                table: "uf_TrainingPlanSessions",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlanSessions_DateLastModifiedUtc",
                table: "uf_TrainingPlanSessions",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlanSessions_MemberId",
                table: "uf_TrainingPlanSessions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlanSessions_MemberId_TrainingPlanId_WeekNumber_~",
                table: "uf_TrainingPlanSessions",
                columns: new[] { "MemberId", "TrainingPlanId", "WeekNumber", "DayNumber", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlanSessions_TrainingPlanId",
                table: "uf_TrainingPlanSessions",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingPlanSessions_TrainingSessionId",
                table: "uf_TrainingPlanSessions",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingSessions_ActivityTypeId",
                table: "uf_TrainingSessions",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingSessions_DateCreatedUtc",
                table: "uf_TrainingSessions",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingSessions_DateLastModifiedUtc",
                table: "uf_TrainingSessions",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingSessions_MemberId",
                table: "uf_TrainingSessions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingSessions_MemberId_ActivityTypeId_Title",
                table: "uf_TrainingSessions",
                columns: new[] { "MemberId", "ActivityTypeId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_TrainingSessions_MemberId_Title",
                table: "uf_TrainingSessions",
                columns: new[] { "MemberId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_UserClaims_UserId",
                table: "uf_UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_UserLogins_UserId",
                table: "uf_UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_UserRoles_RoleId",
                table: "uf_UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "uf_Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "uf_Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uf_Workouts_DateCreatedUtc",
                table: "uf_Workouts",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Workouts_DateLastModifiedUtc",
                table: "uf_Workouts",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Workouts_MemberId",
                table: "uf_Workouts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_Workouts_MemberId_DateStartedUtc",
                table: "uf_Workouts",
                columns: new[] { "MemberId", "DateStartedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSetGroups_DateCreatedUtc",
                table: "uf_WorkoutSetGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSetGroups_DateLastModifiedUtc",
                table: "uf_WorkoutSetGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSetGroups_MemberId",
                table: "uf_WorkoutSetGroups",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSetGroups_WorkoutId_SortOrder",
                table: "uf_WorkoutSetGroups",
                columns: new[] { "WorkoutId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSets_DateCreatedUtc",
                table: "uf_WorkoutSets",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSets_DateLastModifiedUtc",
                table: "uf_WorkoutSets",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSets_ExerciseId",
                table: "uf_WorkoutSets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSets_ListGroupId",
                table: "uf_WorkoutSets",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSets_MemberId",
                table: "uf_WorkoutSets",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutSets_WorkoutId_SortOrder",
                table: "uf_WorkoutSets",
                columns: new[] { "WorkoutId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTasks_DateCreatedUtc",
                table: "uf_WorkoutTasks",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTasks_DateLastModifiedUtc",
                table: "uf_WorkoutTasks",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTasks_MemberId",
                table: "uf_WorkoutTasks",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTasks_WorkoutId_Type_SortOrder",
                table: "uf_WorkoutTasks",
                columns: new[] { "WorkoutId", "Type", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplates_DateCreatedUtc",
                table: "uf_WorkoutTemplates",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplates_DateLastModifiedUtc",
                table: "uf_WorkoutTemplates",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplates_MemberId",
                table: "uf_WorkoutTemplates",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplates_MemberId_Title",
                table: "uf_WorkoutTemplates",
                columns: new[] { "MemberId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSetGroups_DateCreatedUtc",
                table: "uf_WorkoutTemplateSetGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSetGroups_DateLastModifiedUtc",
                table: "uf_WorkoutTemplateSetGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSetGroups_MemberId",
                table: "uf_WorkoutTemplateSetGroups",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSetGroups_WorkoutTemplateId_SortOrder",
                table: "uf_WorkoutTemplateSetGroups",
                columns: new[] { "WorkoutTemplateId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSets_DateCreatedUtc",
                table: "uf_WorkoutTemplateSets",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSets_DateLastModifiedUtc",
                table: "uf_WorkoutTemplateSets",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSets_ExerciseId",
                table: "uf_WorkoutTemplateSets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSets_ListGroupId",
                table: "uf_WorkoutTemplateSets",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSets_MemberId",
                table: "uf_WorkoutTemplateSets",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateSets_WorkoutTemplateId_SortOrder",
                table: "uf_WorkoutTemplateSets",
                columns: new[] { "WorkoutTemplateId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateTasks_DateCreatedUtc",
                table: "uf_WorkoutTemplateTasks",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateTasks_DateLastModifiedUtc",
                table: "uf_WorkoutTemplateTasks",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateTasks_MemberId",
                table: "uf_WorkoutTemplateTasks",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_uf_WorkoutTemplateTasks_WorkoutTemplateId_Type_SortOrder",
                table: "uf_WorkoutTemplateTasks",
                columns: new[] { "WorkoutTemplateId", "Type", "SortOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uf_Activities");

            migrationBuilder.DropTable(
                name: "uf_CookbookInvites");

            migrationBuilder.DropTable(
                name: "uf_CookbookMembers");

            migrationBuilder.DropTable(
                name: "uf_CookbookRecipes");

            migrationBuilder.DropTable(
                name: "uf_DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "uf_HouseholdInvites");

            migrationBuilder.DropTable(
                name: "uf_HouseholdMembers");

            migrationBuilder.DropTable(
                name: "uf_MealPrepPlanRecipes");

            migrationBuilder.DropTable(
                name: "uf_MemberMeta");

            migrationBuilder.DropTable(
                name: "uf_MetricPresets");

            migrationBuilder.DropTable(
                name: "uf_Metrics");

            migrationBuilder.DropTable(
                name: "uf_ProductBundleItems");

            migrationBuilder.DropTable(
                name: "uf_ProductImages");

            migrationBuilder.DropTable(
                name: "uf_ProductSubstitutions");

            migrationBuilder.DropTable(
                name: "uf_ProgramTemplates");

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
                name: "uf_RoleClaims");

            migrationBuilder.DropTable(
                name: "uf_ShoppingListItems");

            migrationBuilder.DropTable(
                name: "uf_ShoppingListRecipeItems");

            migrationBuilder.DropTable(
                name: "uf_StoreProductLocations");

            migrationBuilder.DropTable(
                name: "uf_TrainingPlanSessions");

            migrationBuilder.DropTable(
                name: "uf_UserClaims");

            migrationBuilder.DropTable(
                name: "uf_UserLogins");

            migrationBuilder.DropTable(
                name: "uf_UserRoles");

            migrationBuilder.DropTable(
                name: "uf_UserTokens");

            migrationBuilder.DropTable(
                name: "uf_WorkoutSets");

            migrationBuilder.DropTable(
                name: "uf_WorkoutTasks");

            migrationBuilder.DropTable(
                name: "uf_WorkoutTemplateSets");

            migrationBuilder.DropTable(
                name: "uf_WorkoutTemplateTasks");

            migrationBuilder.DropTable(
                name: "uf_Cookbooks");

            migrationBuilder.DropTable(
                name: "uf_MealPrepPlanSlots");

            migrationBuilder.DropTable(
                name: "uf_MetricDefinitions");

            migrationBuilder.DropTable(
                name: "uf_ProductBundles");

            migrationBuilder.DropTable(
                name: "uf_Programs");

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
                name: "uf_TrainingPlans");

            migrationBuilder.DropTable(
                name: "uf_TrainingSessions");

            migrationBuilder.DropTable(
                name: "uf_Roles");

            migrationBuilder.DropTable(
                name: "uf_Users");

            migrationBuilder.DropTable(
                name: "uf_WorkoutSetGroups");

            migrationBuilder.DropTable(
                name: "uf_Exercises");

            migrationBuilder.DropTable(
                name: "uf_WorkoutTemplateSetGroups");

            migrationBuilder.DropTable(
                name: "uf_MetricDefinitionGroups");

            migrationBuilder.DropTable(
                name: "uf_Recipes");

            migrationBuilder.DropTable(
                name: "uf_ProductCategories");

            migrationBuilder.DropTable(
                name: "uf_Stores");

            migrationBuilder.DropTable(
                name: "uf_ActivityTypes");

            migrationBuilder.DropTable(
                name: "uf_Workouts");

            migrationBuilder.DropTable(
                name: "uf_WorkoutTemplates");

            migrationBuilder.DropTable(
                name: "uf_Households");

            migrationBuilder.DropTable(
                name: "uf_Members");

			migrationBuilder.AlterDatabase()
				.OldAnnotation("Npgsql:CollationDefinition:case_insensitive_collation", "en-u-ks-primary,en-u-ks-primary,icu,False");
		}
    }
}
