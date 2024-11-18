namespace Unshackled.Food.My.Client.Features.Stores.Models;

public class UpdateSortsModel
{
	public string StoreSid { get; set; } = string.Empty;
	public List<FormStoreLocationModel> Locations { get; set; } = [];
}
