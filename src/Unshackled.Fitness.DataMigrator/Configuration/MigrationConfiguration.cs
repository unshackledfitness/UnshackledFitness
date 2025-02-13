namespace Unshackled.Fitness.DataMigrator.Configuration;

internal class MigrationConfiguration
{
	public string SqliteDatabase { get; set; } = string.Empty;
	public string MsSqlDatabase { get; set; } = string.Empty;
	public string MySqlDatabase { get; set; } = string.Empty;
	public string PostgreSqlDatabase { get; set; } = string.Empty;
	public string SourceTablePrefix {  get; set; } = string.Empty;
	public string DestinationTablePrefix { get; set; } = string.Empty;
}
