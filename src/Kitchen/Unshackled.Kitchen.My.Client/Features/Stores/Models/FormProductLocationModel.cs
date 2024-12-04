using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Stores.Models;

public class FormProductLocationModel : ISortable, ICloneable
{
	public string StoreSid { get; set; } = string.Empty;
	public string StoreLocationSid { get; set; } = string.Empty;
	public string ProductSid { get; set; } = string.Empty;
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int SortOrder { get; set; }

	public object Clone()
	{
		return new FormProductLocationModel
		{
			StoreSid = StoreSid,
			StoreLocationSid = StoreLocationSid,
			ProductSid = ProductSid,
			Brand = Brand,
			Title = Title,
			Description = Description,
			SortOrder = SortOrder
		};
	}
}
