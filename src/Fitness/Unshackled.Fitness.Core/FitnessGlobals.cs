namespace Unshackled.Fitness.Core;

public class FitnessGlobals
{
	public const int MaxSelectionLimit = 25;
	public const int MaxSetReps = 1000;
	public const decimal MaxSetWeight = 9999.999M;

	public static class LocalStorageKeys
	{
		public const string TrackTrainingSessionSid = "trackTrainingSessionSid";
	}

	public static class MetaKeys
	{
		public const string AppSettings = "AppSettings";
	}
}
