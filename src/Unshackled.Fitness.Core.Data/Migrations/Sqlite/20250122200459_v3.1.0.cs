using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unshackled.Fitness.Core.Data.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class v310 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "uf_WorkoutTemplateTasks",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_WorkoutTemplateSetGroups",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_WorkoutTemplates",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MusclesTargeted",
                table: "uf_WorkoutTemplates",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "uf_WorkoutTemplates",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "uf_WorkoutTasks",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_WorkoutSets",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_WorkoutSetGroups",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_Workouts",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_Workouts",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MusclesTargeted",
                table: "uf_Workouts",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "uf_UserTokens",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "uf_UserTokens",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "uf_UserTokens",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "uf_UserTokens",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "uf_Users",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "uf_Users",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "uf_Users",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "uf_Users",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "uf_Users",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "uf_Users",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "uf_Users",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "uf_Users",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "uf_Users",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "uf_UserRoles",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "uf_UserRoles",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "uf_UserLogins",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "uf_UserLogins",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "uf_UserLogins",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "uf_UserLogins",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "uf_UserClaims",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "uf_UserClaims",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "uf_UserClaims",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_TrainingSessions",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_TrainingSessions",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_TrainingPlans",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "uf_TrainingPlans",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "uf_Roles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "uf_Roles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "uf_Roles",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "uf_Roles",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "uf_RoleClaims",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "uf_RoleClaims",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "uf_RoleClaims",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_Programs",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "uf_Programs",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_MetricPresets",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Settings",
                table: "uf_MetricPresets",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_MetricDefinitions",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "uf_MetricDefinitions",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HighlightColor",
                table: "uf_MetricDefinitions",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_MetricDefinitionGroups",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "uf_Members",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "MetaValue",
                table: "uf_MemberMeta",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "MetaKey",
                table: "uf_MemberMeta",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_Exercises",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_Exercises",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Muscles",
                table: "uf_Exercises",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Equipment",
                table: "uf_Exercises",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "uf_Exercises",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_ActivityTypes",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "uf_ActivityTypes",
                type: "TEXT",
                maxLength: 10,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_Activities",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_Activities",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "uf_WorkoutTemplateTasks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_WorkoutTemplateSetGroups",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_WorkoutTemplates",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "MusclesTargeted",
                table: "uf_WorkoutTemplates",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "uf_WorkoutTemplates",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "uf_WorkoutTasks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_WorkoutSets",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_WorkoutSetGroups",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_Workouts",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_Workouts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "MusclesTargeted",
                table: "uf_Workouts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "uf_UserTokens",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "uf_UserTokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "uf_UserTokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "uf_UserTokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "uf_Users",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "uf_Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "uf_Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "uf_Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "uf_Users",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "uf_Users",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "uf_Users",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "uf_Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "uf_Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "uf_UserRoles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "uf_UserRoles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "uf_UserLogins",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "uf_UserLogins",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "uf_UserLogins",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "uf_UserLogins",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "uf_UserClaims",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "uf_UserClaims",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "uf_UserClaims",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_TrainingSessions",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_TrainingSessions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_TrainingPlans",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "uf_TrainingPlans",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "uf_Roles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "uf_Roles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "uf_Roles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "uf_Roles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "uf_RoleClaims",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "uf_RoleClaims",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "uf_RoleClaims",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_Programs",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "uf_Programs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_MetricPresets",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Settings",
                table: "uf_MetricPresets",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_MetricDefinitions",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "uf_MetricDefinitions",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "HighlightColor",
                table: "uf_MetricDefinitions",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 10,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_MetricDefinitionGroups",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "uf_Members",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "MetaValue",
                table: "uf_MemberMeta",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "MetaKey",
                table: "uf_MemberMeta",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_Exercises",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_Exercises",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Muscles",
                table: "uf_Exercises",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Equipment",
                table: "uf_Exercises",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "uf_Exercises",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_ActivityTypes",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "uf_ActivityTypes",
                type: "TEXT",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 10,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "uf_Activities",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "uf_Activities",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");
        }
    }
}
