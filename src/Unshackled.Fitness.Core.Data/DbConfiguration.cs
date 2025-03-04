namespace Unshackled.Fitness.Core.Data;

public class DbConfiguration
{
	public const string MSSQL = "mssql";
	public const string MYSQL = "mysql";
	public const string POSTGRESQL = "postgresql";
	public const string SQLITE = "sqlite";

	public string DatabaseType { get; set; } = MSSQL;
	public string TablePrefix { get; set; } = string.Empty;
}
