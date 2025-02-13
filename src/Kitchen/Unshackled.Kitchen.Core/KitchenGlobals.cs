namespace Unshackled.Kitchen.Core;

public class KitchenGlobals
{
	public const int MaxSelectionLimit = 25;
	public const int MaxImageWidth = 800;
	public const string PermissionError = "You don't have permission to perform that action.";
	public const string UncategorizedKey = "_uncategorized";

	public class LocalStorageKeys
	{
		public const string IngredientTitles = "IngredientTitles";
		public const string MakeItIndex = "MakeItIndex";
		public const string MakeItRecipes = "MakeItRecipes";
	}

	public static class MetaKeys
	{
		public const string AppSettings = "AppSettings";
		public const string ActiveCookbookId = "ActiveCookbookId";
		public const string ActiveHouseholdId = "ActiveHouseholdId";
	}

	public static class Paths
	{
		public static string HouseholdImageDir = "households/{0}";
		public static string ProductImageDir = "households/{0}/products/{1}";
		public static string ProductImageFile = "households/{0}/products/{1}/{2}";
		public static string RecipeImageDir = "households/{0}/recipes/{1}";
		public static string RecipeImageFile = "households/{0}/recipes/{1}/{2}";
	}
}
