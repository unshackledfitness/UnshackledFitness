using Microsoft.EntityFrameworkCore;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Kitchen.Core.Data;

public class PostgreSqlServerDbContext : KitchenDbContext
{
	public PostgreSqlServerDbContext(DbContextOptions<PostgreSqlServerDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		base.ConfigureConventions(configurationBuilder);
		configurationBuilder.Properties<string>().UseCollation("case_insensitive_collation");
		configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
		configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp without time zone");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		if (!string.IsNullOrEmpty(ConnectionStrings.DefaultDatabase))
		{
			string prefix = DbConfig.TablePrefix.EndsWith("_") ? DbConfig.TablePrefix : $"{DbConfig.TablePrefix}_";
			// connect to PostgreSql database
			options.UseNpgsql(ConnectionStrings.DefaultDatabase, o =>
			{
				o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
				o.MigrationsHistoryTable($"{prefix}_EFMigrationsHistory");
			});
		}
	}

	public static PostgreSqlServerDbContext Create(string connString, string tablePrefix)
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
}