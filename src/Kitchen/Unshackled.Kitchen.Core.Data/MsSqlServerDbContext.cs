using Microsoft.EntityFrameworkCore;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Kitchen.Core.Data;

public class MsSqlServerDbContext : KitchenDbContext
{
	public MsSqlServerDbContext(DbContextOptions<MsSqlServerDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		if (!string.IsNullOrEmpty(ConnectionStrings.DefaultDatabase))
		{
			string prefix = DbConfig.TablePrefix.EndsWith("_") ? DbConfig.TablePrefix : $"{DbConfig.TablePrefix}_";
			// connect to MS Sql Server database
			options.UseSqlServer(ConnectionStrings.DefaultDatabase, o =>
			{
				o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
				o.MigrationsHistoryTable($"{prefix}_EFMigrationsHistory");
			});
		}
	}

	public static MsSqlServerDbContext Create(string connString, string tablePrefix)
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
}