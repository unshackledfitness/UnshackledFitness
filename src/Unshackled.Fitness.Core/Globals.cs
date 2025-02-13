namespace Unshackled.Fitness.Core;

public class Globals
{
	public const string AccountUrlPrefix = "/account";
	public const int DefaultCacheDurationMinutes = 30;
	public const string LoginCallbackAction = "login-callback";
	public const string LinkLoginCallbackAction = "link-login-callback";
	public const int MaxImageWidth = 800;
	public const int MaxSelectionLimit = 25;
	public const int MaxSetReps = 1000;
	public const decimal MaxSetWeight = 9999.999M;
	public const string PermissionError = "You don't have permission to perform that action.";
	public const string UncategorizedKey = "_uncategorized";
	public const string UnexpectedError = "An unexpected error occurred.";

	public static class ApiConstants
	{
		public const string LocalApi = "LocalApi";
	}

	public static class LocalStorageKeys
	{
		public const string IngredientTitles = "IngredientTitles";
		public const string MakeItIndex = "MakeItIndex";
		public const string MakeItRecipes = "MakeItRecipes";
		public const string TrackTrainingSessionSid = "trackTrainingSessionSid";
	}

	public static class MetaKeys
	{
		public const string AppSettings = "AppSettings";
		public const string ActiveCookbookId = "ActiveCookbookId";
		public const string ActiveHouseholdId = "ActiveHouseholdId";
	}

	public static class MiddlewareItemKeys
	{
		public const string Member = "member";
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
