namespace Unshackled.Studio.DataMigrator.Enums;

internal enum Apps
{
	Fitness = 0,
	Kitchen = 1
}

internal static class AppsExtensions
{
	public static string Title(this Apps app)
	{
		return app switch
		{
			Apps.Fitness => "Fitness",
			Apps.Kitchen => "Kitchen",
			_ => string.Empty
		};
	}
}
