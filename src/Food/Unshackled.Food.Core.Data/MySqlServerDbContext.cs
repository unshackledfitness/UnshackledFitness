using Microsoft.EntityFrameworkCore;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Food.Core.Data;

public class MySqlServerDbContext : FoodDbContext
{
	public MySqlServerDbContext(DbContextOptions<MySqlServerDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		//if (!string.IsNullOrEmpty(ConnectionStrings.DefaultDatabase))
		//{
		//	string prefix = DbConfig.TablePrefix.EndsWith("_") ? DbConfig.TablePrefix : $"{DbConfig.TablePrefix}_";
		//	// connect to MySql database
		//	options.UseMySql(ConnectionStrings.DefaultDatabase, ServerVersion.AutoDetect(ConnectionStrings.DefaultDatabase), o =>
		//	{
		//		o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
		//		o.MigrationsHistoryTable($"{prefix}_EFMigrationsHistory");
		//	});
		//}
	}
}