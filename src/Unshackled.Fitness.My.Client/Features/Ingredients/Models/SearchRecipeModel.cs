﻿using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Models;

public class SearchRecipeModel : SearchModel
{
	public string IngredientKey { get; set; } = string.Empty;
}
