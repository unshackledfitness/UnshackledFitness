namespace Unshackled.Fitness.Core.Enums;

public enum CommonStoreLocations
{
	Bakery,
	Dairy,
	Deli,
	FreshProduce,
	MeatSeafood,
	Pharmacy,
	WineAlcohol
}

public static class CommonStoreLocationsExtensions
{
	public static string Title(this CommonStoreLocations location)
	{
		return location switch
		{
			CommonStoreLocations.Bakery => "Bakery",
			CommonStoreLocations.Dairy => "Dairy",
			CommonStoreLocations.Deli => "Deli",
			CommonStoreLocations.FreshProduce => "Fresh Produce",
			CommonStoreLocations.MeatSeafood => "Meat/Seafood",
			CommonStoreLocations.Pharmacy => "Pharmacy",
			CommonStoreLocations.WineAlcohol => "Wine & Alcohol",
			_ => string.Empty,
		};
	}
}