using Microsoft.Extensions.Configuration;
using Unshackled.Fitness.Core.Data;
using Unshackled.Studio.DataMigrator.Configuration;
using Unshackled.Studio.DataMigrator.Enums;
using Unshackled.Studio.DataMigrator.Migrators;

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

Console.WriteLine();
Console.WriteLine("Choose which app to migrate.");
int optionsCount = 2;
int selected = 0;
bool isDone = false;
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

		Console.WriteLine($"{i} - {((Apps)i).Title()}");
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

Apps targetApp = (Apps)selected;

Console.WriteLine();
Console.WriteLine("Choose your migration direction.");
optionsCount = 12;
selected = 0;
isDone = false;
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
Console.WriteLine($"Make sure the target database tables in the {targetApp.Title()} {targetMigration.Target()} do not exist before continuing.");
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

switch (targetApp)
{
	case Apps.Fitness:
		FitnessMigrator fm = new(migrateConfig, targetMigration);
		await fm.Migrate();
		break;
	case Apps.Kitchen:
		KitchenMigrator km = new(migrateConfig, targetMigration);
		await km.Migrate();
		break;
	default:
		break;
}