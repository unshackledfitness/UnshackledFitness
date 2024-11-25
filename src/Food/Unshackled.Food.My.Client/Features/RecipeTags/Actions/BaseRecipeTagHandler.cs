﻿using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.RecipeTags.Actions;

public abstract class BaseRecipeTagHandler : BaseHandler
{
	public BaseRecipeTagHandler(HttpClient httpClient) : base(httpClient, "recipe-tags") { }
}
