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
                name: "ActivityTypes",
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
                    table.PrimaryKey("PK_ActivityTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityTypes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
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
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExportFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Container = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    RelativePath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DateExpirationUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLastModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportFiles_Members_MemberId",
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
                name: "MetricDefinitionGroups",
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
                    table.PrimaryKey("PK_MetricDefinitionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetricDefinitionGroups_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MetricPresets",
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
                    table.PrimaryKey("PK_MetricPresets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetricPresets_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Programs",
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
                    table.PrimaryKey("PK_Programs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programs_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlans",
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
                    table.PrimaryKey("PK_TrainingPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
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
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workouts_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTemplates",
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
                    table.PrimaryKey("PK_WorkoutTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutTemplates_Members_MemberId",
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
                name: "Activities",
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
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityTypes_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activities_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingSessions",
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
                    table.PrimaryKey("PK_TrainingSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSessions_ActivityTypes_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingSessions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MetricDefinitions",
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
                    table.PrimaryKey("PK_MetricDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetricDefinitions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MetricDefinitions_MetricDefinitionGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "MetricDefinitionGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSetGroups",
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
                    table.PrimaryKey("PK_WorkoutSetGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSetGroups_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutSetGroups_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTasks",
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
                    table.PrimaryKey("PK_WorkoutTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutTasks_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutTasks_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProgramTemplates",
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
                    table.PrimaryKey("PK_ProgramTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramTemplates_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProgramTemplates_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProgramTemplates_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTemplateSetGroups",
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
                    table.PrimaryKey("PK_WorkoutTemplateSetGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutTemplateSetGroups_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutTemplateSetGroups_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTemplateTasks",
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
                    table.PrimaryKey("PK_WorkoutTemplateTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutTemplateTasks_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutTemplateTasks_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlanSessions",
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
                    table.PrimaryKey("PK_TrainingPlanSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlanSessions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingPlanSessions_TrainingPlans_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingPlanSessions_TrainingSessions_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "TrainingSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
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
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metrics_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Metrics_MetricDefinitions_MetricDefinitionId",
                        column: x => x.MetricDefinitionId,
                        principalTable: "MetricDefinitions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSets",
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
                    table.PrimaryKey("PK_WorkoutSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutSets_WorkoutSetGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "WorkoutSetGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTemplateSets",
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
                    table.PrimaryKey("PK_WorkoutTemplateSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutTemplateSets_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutTemplateSets_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutTemplateSets_WorkoutTemplateSetGroups_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "WorkoutTemplateSetGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutTemplateSets_WorkoutTemplates_WorkoutTemplateId",
                        column: x => x.WorkoutTemplateId,
                        principalTable: "WorkoutTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityTypeId",
                table: "Activities",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DateCreatedUtc",
                table: "Activities",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DateLastModifiedUtc",
                table: "Activities",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MemberId",
                table: "Activities",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MemberId_DateEventUtc",
                table: "Activities",
                columns: new[] { "MemberId", "DateEventUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypes_DateCreatedUtc",
                table: "ActivityTypes",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypes_DateLastModifiedUtc",
                table: "ActivityTypes",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypes_MemberId",
                table: "ActivityTypes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypes_MemberId_Title",
                table: "ActivityTypes",
                columns: new[] { "MemberId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_DateCreatedUtc",
                table: "Exercises",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_DateLastModifiedUtc",
                table: "Exercises",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MemberId",
                table: "Exercises",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MemberId_Title_IsArchived",
                table: "Exercises",
                columns: new[] { "MemberId", "Title", "IsArchived" });

            migrationBuilder.CreateIndex(
                name: "IX_ExportFiles_DateCreatedUtc",
                table: "ExportFiles",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ExportFiles_DateLastModifiedUtc",
                table: "ExportFiles",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ExportFiles_MemberId",
                table: "ExportFiles",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportFiles_MemberId_Container_RelativePath",
                table: "ExportFiles",
                columns: new[] { "MemberId", "Container", "RelativePath" });

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
                name: "IX_MetricDefinitionGroups_DateCreatedUtc",
                table: "MetricDefinitionGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitionGroups_DateLastModifiedUtc",
                table: "MetricDefinitionGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitionGroups_MemberId",
                table: "MetricDefinitionGroups",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitionGroups_MemberId_SortOrder",
                table: "MetricDefinitionGroups",
                columns: new[] { "MemberId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitions_DateCreatedUtc",
                table: "MetricDefinitions",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitions_DateLastModifiedUtc",
                table: "MetricDefinitions",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitions_ListGroupId",
                table: "MetricDefinitions",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitions_MemberId",
                table: "MetricDefinitions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitions_MemberId_ListGroupId_SortOrder",
                table: "MetricDefinitions",
                columns: new[] { "MemberId", "ListGroupId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_MetricPresets_DateCreatedUtc",
                table: "MetricPresets",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_MetricPresets_DateLastModifiedUtc",
                table: "MetricPresets",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_MetricPresets_MemberId",
                table: "MetricPresets",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MetricPresets_MemberId_Title",
                table: "MetricPresets",
                columns: new[] { "MemberId", "Title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_DateCreatedUtc",
                table: "Metrics",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_DateLastModifiedUtc",
                table: "Metrics",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_MemberId",
                table: "Metrics",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_MemberId_MetricDefinitionId_DateRecorded",
                table: "Metrics",
                columns: new[] { "MemberId", "MetricDefinitionId", "DateRecorded" });

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_MetricDefinitionId",
                table: "Metrics",
                column: "MetricDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_DateCreatedUtc",
                table: "Programs",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_DateLastModifiedUtc",
                table: "Programs",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_MemberId",
                table: "Programs",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_MemberId_Title",
                table: "Programs",
                columns: new[] { "MemberId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramTemplates_DateCreatedUtc",
                table: "ProgramTemplates",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramTemplates_DateLastModifiedUtc",
                table: "ProgramTemplates",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramTemplates_MemberId",
                table: "ProgramTemplates",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramTemplates_ProgramId_WeekNumber_DayNumber",
                table: "ProgramTemplates",
                columns: new[] { "ProgramId", "WeekNumber", "DayNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramTemplates_WorkoutTemplateId",
                table: "ProgramTemplates",
                column: "WorkoutTemplateId");

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
                name: "IX_TrainingPlans_DateCreatedUtc",
                table: "TrainingPlans",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_DateLastModifiedUtc",
                table: "TrainingPlans",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_MemberId",
                table: "TrainingPlans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_MemberId_DateStarted",
                table: "TrainingPlans",
                columns: new[] { "MemberId", "DateStarted" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_MemberId_Title",
                table: "TrainingPlans",
                columns: new[] { "MemberId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanSessions_DateCreatedUtc",
                table: "TrainingPlanSessions",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanSessions_DateLastModifiedUtc",
                table: "TrainingPlanSessions",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanSessions_MemberId",
                table: "TrainingPlanSessions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanSessions_MemberId_TrainingPlanId_WeekNumber_DayNumber_SortOrder",
                table: "TrainingPlanSessions",
                columns: new[] { "MemberId", "TrainingPlanId", "WeekNumber", "DayNumber", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanSessions_TrainingPlanId",
                table: "TrainingPlanSessions",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanSessions_TrainingSessionId",
                table: "TrainingPlanSessions",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_ActivityTypeId",
                table: "TrainingSessions",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_DateCreatedUtc",
                table: "TrainingSessions",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_DateLastModifiedUtc",
                table: "TrainingSessions",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_MemberId",
                table: "TrainingSessions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_MemberId_ActivityTypeId_Title",
                table: "TrainingSessions",
                columns: new[] { "MemberId", "ActivityTypeId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_MemberId_Title",
                table: "TrainingSessions",
                columns: new[] { "MemberId", "Title" });

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

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_DateCreatedUtc",
                table: "Workouts",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_DateLastModifiedUtc",
                table: "Workouts",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_MemberId",
                table: "Workouts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_MemberId_DateStartedUtc",
                table: "Workouts",
                columns: new[] { "MemberId", "DateStartedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSetGroups_DateCreatedUtc",
                table: "WorkoutSetGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSetGroups_DateLastModifiedUtc",
                table: "WorkoutSetGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSetGroups_MemberId",
                table: "WorkoutSetGroups",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSetGroups_WorkoutId_SortOrder",
                table: "WorkoutSetGroups",
                columns: new[] { "WorkoutId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_DateCreatedUtc",
                table: "WorkoutSets",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_DateLastModifiedUtc",
                table: "WorkoutSets",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_ExerciseId",
                table: "WorkoutSets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_ListGroupId",
                table: "WorkoutSets",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_MemberId",
                table: "WorkoutSets",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_WorkoutId_SortOrder",
                table: "WorkoutSets",
                columns: new[] { "WorkoutId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTasks_DateCreatedUtc",
                table: "WorkoutTasks",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTasks_DateLastModifiedUtc",
                table: "WorkoutTasks",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTasks_MemberId",
                table: "WorkoutTasks",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTasks_WorkoutId_Type_SortOrder",
                table: "WorkoutTasks",
                columns: new[] { "WorkoutId", "Type", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_DateCreatedUtc",
                table: "WorkoutTemplates",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_DateLastModifiedUtc",
                table: "WorkoutTemplates",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_MemberId",
                table: "WorkoutTemplates",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_MemberId_Title",
                table: "WorkoutTemplates",
                columns: new[] { "MemberId", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSetGroups_DateCreatedUtc",
                table: "WorkoutTemplateSetGroups",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSetGroups_DateLastModifiedUtc",
                table: "WorkoutTemplateSetGroups",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSetGroups_MemberId",
                table: "WorkoutTemplateSetGroups",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSetGroups_WorkoutTemplateId_SortOrder",
                table: "WorkoutTemplateSetGroups",
                columns: new[] { "WorkoutTemplateId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSets_DateCreatedUtc",
                table: "WorkoutTemplateSets",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSets_DateLastModifiedUtc",
                table: "WorkoutTemplateSets",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSets_ExerciseId",
                table: "WorkoutTemplateSets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSets_ListGroupId",
                table: "WorkoutTemplateSets",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSets_MemberId",
                table: "WorkoutTemplateSets",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateSets_WorkoutTemplateId_SortOrder",
                table: "WorkoutTemplateSets",
                columns: new[] { "WorkoutTemplateId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateTasks_DateCreatedUtc",
                table: "WorkoutTemplateTasks",
                column: "DateCreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateTasks_DateLastModifiedUtc",
                table: "WorkoutTemplateTasks",
                column: "DateLastModifiedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateTasks_MemberId",
                table: "WorkoutTemplateTasks",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateTasks_WorkoutTemplateId_Type_SortOrder",
                table: "WorkoutTemplateTasks",
                columns: new[] { "WorkoutTemplateId", "Type", "SortOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ExportFiles");

            migrationBuilder.DropTable(
                name: "MemberMeta");

            migrationBuilder.DropTable(
                name: "MetricPresets");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "ProgramTemplates");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "TrainingPlanSessions");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WorkoutSets");

            migrationBuilder.DropTable(
                name: "WorkoutTasks");

            migrationBuilder.DropTable(
                name: "WorkoutTemplateSets");

            migrationBuilder.DropTable(
                name: "WorkoutTemplateTasks");

            migrationBuilder.DropTable(
                name: "MetricDefinitions");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "TrainingPlans");

            migrationBuilder.DropTable(
                name: "TrainingSessions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkoutSetGroups");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "WorkoutTemplateSetGroups");

            migrationBuilder.DropTable(
                name: "MetricDefinitionGroups");

            migrationBuilder.DropTable(
                name: "ActivityTypes");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "WorkoutTemplates");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
