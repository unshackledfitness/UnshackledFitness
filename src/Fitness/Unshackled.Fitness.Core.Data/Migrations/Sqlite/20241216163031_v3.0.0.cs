using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unshackled.Fitness.Core.Data.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class v300 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "uf_Members",
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
                    table.PrimaryKey("PK_uf_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uf_Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uf_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uf_Users",
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
                    table.PrimaryKey("PK_uf_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uf_ActivityTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    DefaultEventType = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultDistanceUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultElevationUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultSpeedUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultCadenceUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                name: "uf_Exercises",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Muscles = table.Column<string>(type: "TEXT", nullable: false),
                    Equipment = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultSetMetricType = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultSetType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTrackingSplit = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                name: "uf_MemberMeta",
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Settings = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ProgramType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    LengthWeeks = table.Column<int>(type: "INTEGER", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastWorkoutUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NextTemplateIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ProgramType = table.Column<int>(type: "INTEGER", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastActivityUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    LengthWeeks = table.Column<int>(type: "INTEGER", nullable: false),
                    NextSessionIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DateStarted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateStartedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateCompletedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MusclesTargeted = table.Column<string>(type: "TEXT", nullable: true),
                    ExerciseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SetCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RepCount = table.Column<int>(type: "INTEGER", nullable: false),
                    VolumeKg = table.Column<decimal>(type: "TEXT", precision: 12, scale: 3, nullable: false),
                    VolumeLb = table.Column<decimal>(type: "TEXT", precision: 12, scale: 3, nullable: false),
                    WorkoutTemplateId = table.Column<long>(type: "INTEGER", nullable: true),
                    RecordSecondsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RecordSecondsAtWeightCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RecordTargetVolumeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RecordTargetWeightCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RecordVolumeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RecordWeightCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ExerciseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MusclesTargeted = table.Column<string>(type: "TEXT", nullable: true),
                    SetCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
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
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
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
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    TrainingSessionId = table.Column<long>(type: "INTEGER", nullable: true),
                    AverageCadence = table.Column<double>(type: "REAL", nullable: true),
                    AverageCadenceUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageHeartRateBpm = table.Column<int>(type: "INTEGER", nullable: true),
                    AveragePace = table.Column<int>(type: "INTEGER", nullable: true),
                    AveragePower = table.Column<double>(type: "REAL", nullable: true),
                    AverageSpeed = table.Column<double>(type: "REAL", nullable: true),
                    AverageSpeedN = table.Column<double>(type: "REAL", nullable: true),
                    AverageSpeedUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    DateEvent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEventUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false),
                    MaximumAltitude = table.Column<double>(type: "REAL", nullable: true),
                    MaximumAltitudeN = table.Column<double>(type: "REAL", nullable: true),
                    MaximumAltitudeUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    MaximumCadence = table.Column<double>(type: "REAL", nullable: true),
                    MaximumCadenceUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    MaximumHeartRateBpm = table.Column<int>(type: "INTEGER", nullable: true),
                    MaximumPace = table.Column<int>(type: "INTEGER", nullable: true),
                    MaximumPower = table.Column<double>(type: "REAL", nullable: true),
                    MaximumSpeed = table.Column<double>(type: "REAL", nullable: true),
                    MaximumSpeedN = table.Column<double>(type: "REAL", nullable: true),
                    MaximumSpeedUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    MinimumAltitude = table.Column<double>(type: "REAL", nullable: true),
                    MinimumAltitudeN = table.Column<double>(type: "REAL", nullable: true),
                    MinimumAltitudeUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetCadence = table.Column<double>(type: "REAL", nullable: true),
                    TargetCadenceUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetCalories = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetDistance = table.Column<double>(type: "REAL", nullable: true),
                    TargetDistanceN = table.Column<double>(type: "REAL", nullable: true),
                    TargetDistanceUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetHeartRateBpm = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetPace = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetPower = table.Column<double>(type: "REAL", nullable: true),
                    TargetTimeSeconds = table.Column<int>(type: "INTEGER", nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    TotalAscent = table.Column<double>(type: "REAL", nullable: true),
                    TotalAscentN = table.Column<double>(type: "REAL", nullable: true),
                    TotalAscentUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalCalories = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalDescent = table.Column<double>(type: "REAL", nullable: true),
                    TotalDescentN = table.Column<double>(type: "REAL", nullable: true),
                    TotalDescentUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalDistance = table.Column<double>(type: "REAL", nullable: true),
                    TotalDistanceUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalDistanceN = table.Column<double>(type: "REAL", nullable: true),
                    TotalTimeSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetCadence = table.Column<double>(type: "REAL", nullable: true),
                    TargetCadenceUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetCalories = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetDistance = table.Column<double>(type: "REAL", nullable: true),
                    TargetDistanceN = table.Column<double>(type: "REAL", nullable: true),
                    TargetDistanceUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetHeartRateBpm = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetPace = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetPower = table.Column<double>(type: "REAL", nullable: true),
                    TargetTimeSeconds = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                name: "uf_MetricDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SubTitle = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    MetricType = table.Column<int>(type: "INTEGER", nullable: false),
                    ListGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    HighlightColor = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    MaxValue = table.Column<decimal>(type: "TEXT", precision: 2, scale: 0, nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsOnDashboard = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutId = table.Column<long>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Completed = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProgramId = table.Column<long>(type: "INTEGER", nullable: false),
                    WorkoutTemplateId = table.Column<long>(type: "INTEGER", nullable: false),
                    WeekNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    DayNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutTemplateId = table.Column<long>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                        name: "FK_uf_WorkoutTemplateSetGroups_uf_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "uf_WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_WorkoutTemplateTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutTemplateId = table.Column<long>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                        name: "FK_uf_WorkoutTemplateTasks_uf_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "uf_WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_TrainingPlanSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainingPlanId = table.Column<long>(type: "INTEGER", nullable: false),
                    TrainingSessionId = table.Column<long>(type: "INTEGER", nullable: false),
                    WeekNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    DayNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                        name: "FK_uf_TrainingPlanSessions_uf_TrainingSessions_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "uf_TrainingSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "uf_Metrics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricDefinitionId = table.Column<long>(type: "INTEGER", nullable: false),
                    DateRecorded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RecordedValue = table.Column<decimal>(type: "TEXT", precision: 15, scale: 3, nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutId = table.Column<long>(type: "INTEGER", nullable: false),
                    ExerciseId = table.Column<long>(type: "INTEGER", nullable: false),
                    SetMetricType = table.Column<int>(type: "INTEGER", nullable: false),
                    ListGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsTrackingSplit = table.Column<bool>(type: "INTEGER", nullable: false),
                    IntensityTarget = table.Column<int>(type: "INTEGER", nullable: false),
                    RepMode = table.Column<int>(type: "INTEGER", nullable: false),
                    RepsTarget = table.Column<int>(type: "INTEGER", nullable: false),
                    Reps = table.Column<int>(type: "INTEGER", nullable: false),
                    RepsLeft = table.Column<int>(type: "INTEGER", nullable: false),
                    RepsRight = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondsTarget = table.Column<int>(type: "INTEGER", nullable: false),
                    Seconds = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondsLeft = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondsRight = table.Column<int>(type: "INTEGER", nullable: false),
                    SetType = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightLb = table.Column<decimal>(type: "TEXT", precision: 7, scale: 3, nullable: false),
                    WeightKg = table.Column<decimal>(type: "TEXT", precision: 7, scale: 3, nullable: false),
                    VolumeLb = table.Column<decimal>(type: "TEXT", precision: 10, scale: 3, nullable: false),
                    VolumeKg = table.Column<decimal>(type: "TEXT", precision: 10, scale: 3, nullable: false),
                    DateRecorded = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateRecordedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsBestSetBySeconds = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBestSetByWeight = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBestSetByVolume = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecordSeconds = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecordSecondsAtWeight = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecordTargetVolume = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecordTargetWeight = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecordVolume = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecordWeight = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutTemplateId = table.Column<long>(type: "INTEGER", nullable: false),
                    ListGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    ExerciseId = table.Column<long>(type: "INTEGER", nullable: false),
                    SetMetricType = table.Column<int>(type: "INTEGER", nullable: false),
                    SetType = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    RepMode = table.Column<int>(type: "INTEGER", nullable: false),
                    RepsTarget = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondsTarget = table.Column<int>(type: "INTEGER", nullable: false),
                    IntensityTarget = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
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
                        name: "FK_uf_WorkoutTemplateSets_uf_WorkoutTemplateSetGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "uf_WorkoutTemplateSetGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_uf_WorkoutTemplateSets_uf_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "uf_WorkoutTemplates",
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
                name: "IX_uf_RoleClaims_RoleId",
                table: "uf_RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "uf_Roles",
                column: "NormalizedName",
                unique: true);

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
                name: "IX_uf_TrainingPlanSessions_MemberId_TrainingPlanId_WeekNumber_DayNumber_SortOrder",
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
                name: "uf_MemberMeta");

            migrationBuilder.DropTable(
                name: "uf_MetricPresets");

            migrationBuilder.DropTable(
                name: "uf_Metrics");

            migrationBuilder.DropTable(
                name: "uf_ProgramTemplates");

            migrationBuilder.DropTable(
                name: "uf_RoleClaims");

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
                name: "uf_MetricDefinitions");

            migrationBuilder.DropTable(
                name: "uf_Programs");

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
                name: "uf_ActivityTypes");

            migrationBuilder.DropTable(
                name: "uf_Workouts");

            migrationBuilder.DropTable(
                name: "uf_WorkoutTemplates");

            migrationBuilder.DropTable(
                name: "uf_Members");
        }
    }
}
