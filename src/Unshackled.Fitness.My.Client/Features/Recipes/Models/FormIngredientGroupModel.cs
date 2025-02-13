using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class FormIngredientGroupModel : ISortableGroupForm, ICloneable
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public bool IsNew { get; set; }
	public bool IsDeleted { get; set; }

	public object Clone()
	{
		return new FormIngredientGroupModel
		{
			IsDeleted = IsDeleted,
			IsNew = IsNew,
			Sid = Sid,
			SortOrder = SortOrder,
			Title = Title
		};
	}
}
