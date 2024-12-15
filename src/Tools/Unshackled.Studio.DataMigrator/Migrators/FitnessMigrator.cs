using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;
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
				dbLegacy = SetupMsSQL(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupMySQL(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MsSqlToPostgreSql:
				dbLegacy = SetupMsSQL(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupPostgreSQL(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MsSqlToSqlite:
				dbLegacy = SetupMsSQL(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupSqlite(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.MySqlToMsSql:
				dbLegacy = SetupMySQL(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupMsSQL(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.MySqlToPostgreSql:
				dbLegacy = SetupMySQL(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupPostgreSQL(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MySqlToSqlite:
				dbLegacy = SetupMySQL(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupSqlite(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.PostgreSqlToMsSql:
				dbLegacy = SetupPostgreSQL(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupMsSQL(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.PostgreSqlToMySql:
				dbLegacy = SetupPostgreSQL(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupMySQL(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.PostgreSqlToSqlite:
				dbLegacy = SetupPostgreSQL(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupSqlite(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.SqliteToMsSql:
				dbLegacy = SetupSqlite(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupMsSQL(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.SqliteToMySql:
				dbLegacy = SetupSqlite(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupMySQL(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.SqliteToPostgreSql:
				dbLegacy = SetupSqlite(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = SetupPostgreSQL(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
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

	private MsSqlServerDbContext SetupMsSQL(string connString, string tablePrefix)
	{
		var ob = new DbContextOptionsBuilder<MsSqlServerDbContext>();
		ConnectionStrings connStrings = new() { DefaultDatabase = connString };
		DbConfiguration dbConfig = new()
		{
			DatabaseType = DbConfiguration.MSSQL,
			TablePrefix = tablePrefix
		};
		return new MsSqlServerDbContext(ob.Options, connStrings, dbConfig);
	}

	private MySqlServerDbContext SetupMySQL(string connString, string tablePrefix)
	{
		var ob = new DbContextOptionsBuilder<MySqlServerDbContext>();
		ConnectionStrings connStrings = new() { DefaultDatabase = connString };
		DbConfiguration dbConfig = new()
		{
			DatabaseType = DbConfiguration.MYSQL,
			TablePrefix = tablePrefix
		};
		return new MySqlServerDbContext(ob.Options, connStrings, dbConfig);
	}

	private PostgreSqlServerDbContext SetupPostgreSQL(string connString, string tablePrefix)
	{
		var ob = new DbContextOptionsBuilder<PostgreSqlServerDbContext>();
		ConnectionStrings connStrings = new() { DefaultDatabase = connString };
		DbConfiguration dbConfig = new()
		{
			DatabaseType = DbConfiguration.POSTGRESQL,
			TablePrefix = tablePrefix
		};
		return new PostgreSqlServerDbContext(ob.Options, connStrings, dbConfig);
	}

	private SqliteDbContext SetupSqlite(string connString, string tablePrefix)
	{
		var ob = new DbContextOptionsBuilder<SqliteDbContext>();
		ConnectionStrings connStrings = new() { DefaultDatabase = connString };
		DbConfiguration dbConfig = new()
		{
			DatabaseType = DbConfiguration.SQLITE,
			TablePrefix = tablePrefix
		};
		return new SqliteDbContext(ob.Options, connStrings, dbConfig);
	}
}
