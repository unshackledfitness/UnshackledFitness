namespace Unshackled.Fitness.DataMigrator.Enums;

internal enum MigrationTypes
{
	MsSqlToMySql = 1,
	MsSqlToPostgreSql = 2,
	MySqlToMsSql = 3,
	MySqlToPostgreSql = 4,
	PostgreSqlToMsSql = 5,
	PostgreSqlToMySql = 6,
	SqliteToMsSql = 7,
	SqliteToMySql = 8,
	SqliteToPostgreSql = 9
}

internal static class MigrationTypesExtensions
{
	public static string Target(this MigrationTypes type)
	{
		return type switch
		{
			MigrationTypes.MsSqlToMySql => "MySQL Server",
			MigrationTypes.MsSqlToPostgreSql => "PostgreSQL Server",
			MigrationTypes.MySqlToMsSql => "MS SQL Server",
			MigrationTypes.MySqlToPostgreSql => "PostgreSQL Server",
			MigrationTypes.PostgreSqlToMsSql => "MS SQL Server",
			MigrationTypes.PostgreSqlToMySql => "MySQL Server",
			MigrationTypes.SqliteToMsSql => "MS SQL Server",
			MigrationTypes.SqliteToMySql => "MySQL Server",
			MigrationTypes.SqliteToPostgreSql => "PostgreSQL Server",
			_ => string.Empty
		};
	}

	public static string Title(this MigrationTypes type)
	{
		return type switch
		{
			MigrationTypes.MsSqlToMySql => "MS SQL to MySQL",
			MigrationTypes.MsSqlToPostgreSql => "MS SQL to PostgreSQL",
			MigrationTypes.MySqlToMsSql => "MySQL to MS SQL",
			MigrationTypes.MySqlToPostgreSql => "MySQL to PostgreSQL",
			MigrationTypes.PostgreSqlToMsSql => "PostgreSQL to MS SQL",
			MigrationTypes.PostgreSqlToMySql => "PostgreSQL to MySQL",
			MigrationTypes.SqliteToMsSql => "Sqlite to MS SQL",
			MigrationTypes.SqliteToMySql => "Sqlite to MySQL",
			MigrationTypes.SqliteToPostgreSql => "Sqlite to PostgreSQL",
			_ => string.Empty
		};
	}
}

