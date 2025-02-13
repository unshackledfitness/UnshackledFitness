using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Studio.DataMigrator.Configuration;
using Unshackled.Studio.DataMigrator.Enums;

namespace Unshackled.Studio.DataMigrator.Migrators;
internal class FitnessMigrator : BaseMigrator<FitnessDbContext>
{
	public FitnessMigrator(MigrationConfiguration mConfig, MigrationTypes migrationType)
	{
		isIdentityInsertRequired = false;
		switch (migrationType)
		{
			case MigrationTypes.MsSqlToMySql:
				dbLegacy = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MsSqlToPostgreSql:
				dbLegacy = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MsSqlToSqlite:
				dbLegacy = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.MySqlToMsSql:
				dbLegacy = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.MySqlToPostgreSql:
				dbLegacy = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MySqlToSqlite:
				dbLegacy = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.PostgreSqlToMsSql:
				dbLegacy = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.PostgreSqlToMySql:
				dbLegacy = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.PostgreSqlToSqlite:
				dbLegacy = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.SqliteToMsSql:
				dbLegacy = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.SqliteToMySql:
				dbLegacy = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.SqliteToPostgreSql:
				dbLegacy = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
				break;
			default:
				dbLegacy = default!;
				dbNew = default!;
				break;
		}
	}

	public async Task Migrate()
	{
		await dbNew.Database.MigrateAsync();

		await MigrateUsers();
		await MigrateUserClaims();
		await MigrateUserLogins();
		await MigrateUserTokens();
		await MigrateRoles();
		await MigrateRoleClaims();
		await MigrateUserRoles();
		await MigrateDbSet(dbLegacy.Members, dbNew.Members, "Members");
		await MigrateMemberMeta();
		await MigrateDbSet(dbLegacy.Exercises, dbNew.Exercises, "Exercises");
		await MigrateDbSet(dbLegacy.WorkoutTemplates, dbNew.WorkoutTemplates, "Workout Templates");
		await MigrateDbSet(dbLegacy.WorkoutTemplateSetGroups, dbNew.WorkoutTemplateSetGroups, "Workout Template Set Groups");
		await MigrateDbSet(dbLegacy.WorkoutTemplateSets, dbNew.WorkoutTemplateSets, "Workout Template Sets");
		await MigrateDbSet(dbLegacy.WorkoutTemplateTasks, dbNew.WorkoutTemplateTasks, "Workout Template Tasks");
		await MigrateDbSet(dbLegacy.Programs, dbNew.Programs, "Programs");
		await MigrateDbSet(dbLegacy.ProgramTemplates, dbNew.ProgramTemplates, "Program Templates");
		await MigrateDbSet(dbLegacy.Workouts, dbNew.Workouts, "Workouts");
		await MigrateDbSet(dbLegacy.WorkoutSetGroups, dbNew.WorkoutSetGroups, "Workout Set Groups");
		await MigrateDbSet(dbLegacy.WorkoutSets, dbNew.WorkoutSets, "Workout Sets");
		await MigrateDbSet(dbLegacy.WorkoutTasks, dbNew.WorkoutTasks, "Workout Tasks");
		await MigrateDbSet(dbLegacy.ActivityTypes, dbNew.ActivityTypes, "Activity Types"); ;
		await MigrateDbSet(dbLegacy.TrainingSessions, dbNew.TrainingSessions, "Training Sessions");
		await MigrateDbSet(dbLegacy.TrainingPlans, dbNew.TrainingPlans, "Training Plans");
		await MigrateDbSet(dbLegacy.TrainingPlanSessions, dbNew.TrainingPlanSessions, "Training Plan Sessions");
		await MigrateDbSet(dbLegacy.Activities, dbNew.Activities, "Activities");
		await MigrateDbSet(dbLegacy.MetricDefinitionGroups, dbNew.MetricDefinitionGroups, "Metric Definition Groups");
		await MigrateDbSet(dbLegacy.MetricDefinitions, dbNew.MetricDefinitions, "Metric Definitions");
		await MigrateDbSet(dbLegacy.Metrics, dbNew.Metrics, "Metrics");
		await MigrateDbSet(dbLegacy.MetricPresets, dbNew.MetricPresets, "Metric Presets");
	}
}
