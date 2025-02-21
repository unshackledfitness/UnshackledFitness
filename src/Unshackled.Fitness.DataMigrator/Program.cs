using Microsoft.Extensions.Configuration;
using Unshackled.Fitness.DataMigrator.Configuration;
using Unshackled.Fitness.DataMigrator.Enums;
using Unshackled.Fitness.DataMigrator.Migrators;

IConfigurationRoot config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

MigrationConfiguration? migrateConfig = config.GetSection("ConnectionStrings").Get<MigrationConfiguration>();

if (migrateConfig == null)
{
	Console.WriteLine();
	Console.WriteLine("Migration configuration is missing.");
	Console.WriteLine("-------------------------------");
	Console.WriteLine("Press any key to quit.");
	Console.ReadKey();
	return;
}

int optionsCount = (int)MigrationTypes.SqliteToPostgreSql + 1;
int selected = 0;
bool isDone = false;

Console.WriteLine();
Console.WriteLine("Choose your migration direction.");
while (!isDone)
{
	for (int i = 0; i < optionsCount; i++)
	{
		if (selected == i)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("> ");
		}
		else
		{
			Console.Write("  ");
		}

		Console.WriteLine($"{i} - {((MigrationTypes)i).Title()}");
		Console.ResetColor();
	}

	switch (Console.ReadKey(true).Key)
	{
		case ConsoleKey.UpArrow:
			selected = Math.Max(0, selected - 1);
			break;
		case ConsoleKey.DownArrow:
			selected = Math.Min(optionsCount - 1, selected + 1);
			break;
		case ConsoleKey.Enter:
			isDone = true;
			break;
	}

	if (!isDone)
		Console.CursorTop = Console.CursorTop - optionsCount;
}
MigrationTypes targetMigration = (MigrationTypes)selected;

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Make sure the target database tables in the Fitness {targetMigration.Target()} do not exist before continuing.");
Console.ResetColor();

Console.WriteLine();
bool shouldStart = false;
while (!shouldStart)
{
	Console.Write("Continue with migration? (y/n) ");
	switch (Console.ReadKey(true).Key)
	{
		case ConsoleKey.Y:
			shouldStart = true;
			break;
		default:
			Environment.Exit(0);
			break;
	}
}
Console.WriteLine();

FitnessMigrator fm = new(migrateConfig, targetMigration);
await fm.Migrate();