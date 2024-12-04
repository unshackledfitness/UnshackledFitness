namespace Unshackled.Kitchen.My.Client.Features.Dashboard.Models;

public class DistinctProductCategoryComparer : IEqualityComparer<PinnedProductModel>
{
	public bool Equals(PinnedProductModel? x, PinnedProductModel? y)
	{
		if (x == null && y == null)
			return true;

		if (x == null && y != null)
			return false;

		if (x != null && y == null)
			return false;

		return x!.ProductCategorySid == y!.ProductCategorySid;
	}

	public int GetHashCode(PinnedProductModel obj)
	{
		return obj.ProductCategorySid?.GetHashCode() ?? 0;
	}
}
