using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Client.Utils;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public class DrawerBulkAddIngredientsBase : BaseFormComponent<FormBulkAddIngredientModel, FormBulkAddIngredientModel.Validator>
{
	protected List<FormIngredientModel> PreviewIngredients { get; set; } = [];

	protected void HandlePreview(string bulkText)
	{
		PreviewIngredients.Clear();
		if (!string.IsNullOrEmpty(bulkText))
		{
			var reader = new StringReader(bulkText);
			string? line;
			while (null != (line = reader.ReadLine()))
			{
				if (!string.IsNullOrEmpty(line.Trim()))
				{
					var result = IngredientUtils.Parse(line);
					PreviewIngredients.Add(new()
					{
						Amount = result.Amount,
						AmountUnit = result.AmountUnit,
						AmountLabel = result.AmountLabel,
						AmountText = result.AmountText,
						Key = result.Title.NormalizeKey(),
						PrepNote = result.PrepNote,
						SortOrder = PreviewIngredients.Count,
						Title = result.Title
					});
				}
			}
		}
	}
}