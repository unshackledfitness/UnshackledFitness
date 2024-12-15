using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;
using Unshackled.Studio.Core.Data.Entities;
using Unshackled.Studio.DataMigrator.Configuration;
using Unshackled.Studio.DataMigrator.Enums;
using Unshackled.Studio.DataMigrator.Extensions;

namespace Unshackled.Studio.DataMigrator.Migrators;
internal class FitnessMigrator
{
	private const int pageSize = 50;

	private readonly FitnessDbContext dbLegacy;
	private readonly FitnessDbContext dbNew;
	private readonly bool isIdentityInsertRequired;

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

	private async Task MigrateDbSet<TEntity>(DbSet<TEntity> dbSource, DbSet<TEntity> dbDest, string title)
		where TEntity : BaseEntity
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbSource.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbSource
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbDest.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<TEntity>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private async Task MigrateMemberMeta()
	{
		string title = "Member Meta";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.MemberMeta.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.MemberMeta
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.MemberMeta.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<MemberMetaEntity>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private async Task MigrateRoleClaims()
	{
		string title = "Role Claims";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.RoleClaims.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.RoleClaims
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.RoleClaims.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityRoleClaim<string>>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private async Task MigrateRoles()
	{
		string title = "Roles";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.Roles.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.Roles
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.Roles.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityRole>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private async Task MigrateUserRoles()
	{
		string title = "User Roles";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserRoles.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserRoles
					.AsNoTracking()
					.OrderBy(x => x.UserId)
						.ThenBy(x => x.RoleId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserRoles.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private async Task MigrateUserClaims()
	{
		string title = "User Claims";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserClaims.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserClaims
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserClaims.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityUserClaim<string>>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private async Task MigrateUserLogins()
	{
		string title = "Users";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserLogins.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserLogins
					.AsNoTracking()
					.OrderBy(x => x.LoginProvider)
						.ThenBy(x => x.ProviderKey)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserLogins.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private async Task MigrateUsers()
	{
		string title = "Users";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.Users.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.Users
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.Users.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private async Task MigrateUserTokens()
	{
		string title = "User Tokens";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserTokens.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserTokens
					.AsNoTracking()
					.OrderBy(x => x.UserId)
						.ThenBy(x => x.LoginProvider)
							.ThenBy(x => x.Name)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserTokens.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	private void PrepareSqlitePath(string connString)
	{
		Msg.WriteOngoing("Preparing directories");

		string path = connString
			.Replace("Data Source=", string.Empty);

		if (Path.DirectorySeparatorChar != '/')
		{
			path = path.Replace('/', Path.DirectorySeparatorChar);
		}

		int idxLastSlash = path.LastIndexOf(Path.DirectorySeparatorChar);
		string dirPath = path.Substring(0, idxLastSlash + 1);

		Directory.CreateDirectory(dirPath);

		if (File.Exists(path))
		{
			File.Delete(path);
		}
		Msg.WriteComplete();
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
