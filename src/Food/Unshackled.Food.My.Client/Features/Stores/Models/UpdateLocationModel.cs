namespace Unshackled.Food.My.Client.Features.Stores.Models;

public class UpdateLocationsModel
{
	public List<FormStoreLocationModel> DeletedStoreLocations { get; set; } = new();
	public List<FormStoreLocationModel> StoreLocations { get; set; } = new();
}
