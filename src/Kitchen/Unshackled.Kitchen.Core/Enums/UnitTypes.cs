namespace Unshackled.Kitchen.Core.Enums;

public enum UnitTypes
{
	Item,
	Volume,
	Weight
}


public static class UnitTypesExtensions
{
	public static string Label(this UnitTypes unit)
	{
		return unit switch
		{
			UnitTypes.Item => "item",
			UnitTypes.Volume => "volume",
			UnitTypes.Weight => "weight",
			_ => string.Empty,
		};
	}

	public static string Title(this UnitTypes unit)
	{
		return unit switch
		{
			UnitTypes.Item => "Item",
			UnitTypes.Volume => "Volume",
			UnitTypes.Weight => "Weight",
			_ => string.Empty,
		};
	}
}