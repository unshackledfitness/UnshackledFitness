﻿using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class ShoppingListModel : BaseObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}
