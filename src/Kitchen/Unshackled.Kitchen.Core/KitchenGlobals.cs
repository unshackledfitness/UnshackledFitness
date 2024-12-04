namespace Unshackled.Kitchen.Core;

public class KitchenGlobals
{
	public const int MaxSelectionLimit = 25;
	public const int MaxSetReps = 1000;
	public const decimal MaxSetWeight = 9999.999M;
	public const string PermissionError = "You don't have permission to perform that action.";
	public const string UncategorizedKey = "_uncategorized";

	public class LocalStorageKeys
	{
		public const string IngredientTitles = "IngredientTitles";
	}

	public static class MetaKeys
	{
		public const string AppSettings = "AppSettings";
		public const string ActiveCookbookId = "ActiveCookbookId";
		public const string ActiveHouseholdId = "ActiveHouseholdId";
	}
}
