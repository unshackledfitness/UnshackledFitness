using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Unshackled.Fitness.Core.Configuration;

namespace Unshackled.Fitness.Core.Data;

public class SqliteDbContext : BaseDbContext
{
	public SqliteDbContext(DbContextOptions<SqliteDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<string>().UseCollation("NOCASE");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		if (!string.IsNullOrEmpty(ConnectionStrings.DefaultDatabase))
		{
			string prefix = DbConfig.TablePrefix.EndsWith("_") ? DbConfig.TablePrefix : $"{DbConfig.TablePrefix}_";
			// connect to MS Sql Server database
			options.UseSqlite(ConnectionStrings.DefaultDatabase, o =>
			{
				o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
				o.MigrationsHistoryTable($"{prefix}_EFMigrationsHistory");
			});
		}
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
		{
			// SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
			// here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
			// To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
			// use the DateTimeOffsetToBinaryConverter
			// Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
			// This only supports millisecond precision, but should be sufficient for most use cases.
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
																			   || p.PropertyType == typeof(DateTimeOffset?));
				foreach (var property in properties)
				{
					builder
						.Entity(entityType.Name)
						.Property(property.Name)
						.HasConversion(new DateTimeOffsetToBinaryConverter());
				}
			}
		}
	}

	public static SqliteDbContext Create(string connString, string tablePrefix)
	{
		var ob = new DbContextOptionsBuilder<SqliteDbContext>();
		ConnectionStrings connStrings = new() { DefaultDatabase = connString };
		DbConfiguration dbConfig = new()
		{
			DatabaseType = "sqlite",
			TablePrefix = tablePrefix
		};
		return new SqliteDbContext(ob.Options, connStrings, dbConfig);
	}
}
