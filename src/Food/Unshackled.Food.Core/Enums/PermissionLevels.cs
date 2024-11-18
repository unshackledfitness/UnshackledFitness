namespace Unshackled.Food.Core.Enums;

public enum PermissionLevels
{
	Read = 0,
	Write = 1,
	Admin = 2
}

public static class PermissionLevelsExtensions
{
	public static string Title(this PermissionLevels permission)
	{
		return permission switch
		{
			PermissionLevels.Read => "Read",
			PermissionLevels.Write => "Read/Write",
			PermissionLevels.Admin => "Administrator",
			_ => string.Empty,
		};
	}
}
