using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Utils;

namespace Unshackled.Fitness.My.Services;

public static class DbStartupService
{
	public static async Task<bool> IsReady(DbConfiguration dbConfig, ConnectionStrings connectionStrings, IServiceProvider services)
	{
		bool isReady = false;

		int retries = 1;
		while (retries < 7)
		{
			Console.WriteLine("Connecting to db. Trial: {0}", retries);
			switch (dbConfig.DatabaseType?.ToLower())
			{
				case DbConfiguration.MSSQL:
					isReady = await services.GetRequiredService<MsSqlServerDbContext>()
						.Database.CanConnectAsync();
					break;
				case DbConfiguration.MYSQL:
					isReady = await services.GetRequiredService<MySqlServerDbContext>()
						.Database.CanConnectAsync();
					break;
				case DbConfiguration.POSTGRESQL:
					isReady = await services.GetRequiredService<PostgreSqlServerDbContext>()
						.Database.CanConnectAsync();
					break;
				case DbConfiguration.SQLITE:
					FileUtils.EnsureDataSourceDirectoryExists(connectionStrings.DefaultDatabase);
					isReady = await services.GetRequiredService<SqliteDbContext>()
						.Database.CanConnectAsync();
					break;
				default:
					break;
			}

			if (!isReady)
			{
				Thread.Sleep((int)Math.Pow(2, retries) * 1000);
				retries++;
			}
			else
			{
				break;
			}
		}

		return isReady;
	}

	public static async Task ApplyMigrations(DbConfiguration dbConfig, ConnectionStrings connectionStrings, IServiceProvider services)
	{
		switch (dbConfig.DatabaseType?.ToLower())
		{
			case DbConfiguration.MSSQL:
				await services.GetRequiredService<MsSqlServerDbContext>()
					.Database.MigrateAsync();
				break;
			case DbConfiguration.MYSQL:
				await services.GetRequiredService<MySqlServerDbContext>()
					.Database.MigrateAsync();
				break;
			case DbConfiguration.POSTGRESQL:
				await services.GetRequiredService<PostgreSqlServerDbContext>()
					.Database.MigrateAsync();
				break;
			case DbConfiguration.SQLITE:
				FileUtils.EnsureDataSourceDirectoryExists(connectionStrings.DefaultDatabase);
				await services.GetRequiredService<SqliteDbContext>()
					.Database.MigrateAsync();
				break;
			default:
				break;
		}
	}
}
