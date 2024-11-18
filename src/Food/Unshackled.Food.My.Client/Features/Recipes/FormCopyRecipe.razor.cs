using MediatR;
using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class FormCopyRecipeBase : BaseFormComponent<FormCopyRecipeModel, FormCopyRecipeModel.Validator>
{
	[Inject] protected IMediator Mediator { get; set; } = default!;

	[Parameter] public List<HouseholdListModel> Households { get; set; } = new();
}