namespace Unshackled.Fitness.Core.Enums;

public enum CadenceUnits
{
	RPM = 0,
	SPM = 1
}

public static class CadenceUnitsExtensions
{
	public static string Label(this CadenceUnits unit)
	{
		return unit switch
		{
			CadenceUnits.RPM => "rpm",
			CadenceUnits.SPM => "spm",
			_ => string.Empty,
		};
	}
}
