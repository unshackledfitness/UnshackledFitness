using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class FormPropertiesBase : BaseFormComponent<FormRecipeModel, FormRecipeModel.Validator>
{
	[Parameter] public List<RecipeTagSelectItem> RecipeTags { get; set; } = [];
	protected string? TimeCook { get; set; } = string.Empty;
	protected string? TimePrep { get; set; } = string.Empty;

	protected void HandlePrepTimeTextChange(string? value)
	{
		if (TimePrep != value)
		{
			Model.PrepTime = ParseTime(value);
			if (!Model.PrepTime.HasValue || Model.PrepTime == TimeSpan.Zero)
			{
				TimePrep = string.Empty;
			}
			else
			{
				TimePrep = Model.PrepTime.Value.ToString(@"hh\:mm");
			}
		}
	}

	protected void HandleCookTimeTextChange(string? value)
	{
		if (TimeCook != value)
		{
			Model.CookTime = ParseTime(value);
			if (!Model.CookTime.HasValue || Model.CookTime == TimeSpan.Zero)
			{
				TimeCook = string.Empty;
			}
			else
			{
				TimeCook = Model.CookTime.Value.ToString(@"hh\:mm");
			}
		}
	}

	private TimeSpan ParseTime(string? value)
	{
		if (string.IsNullOrEmpty(value)) return TimeSpan.Zero;

		var split = value.Split(':');
		if (split.Length > 1)
		{
			int.TryParse(split[0], out int hours);
			int.TryParse(split[1], out int minutes);

			return new TimeSpan(hours, minutes, 0);
		}
		else if (split.Length == 1)
		{
			int.TryParse(split[0], out int minutes);
			return new TimeSpan(0, minutes, 0);
		}
		return TimeSpan.Zero;
	}
}