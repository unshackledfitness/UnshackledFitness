using MediatR;
using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Actions;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public class DrawerIngredientBase : BaseFormComponent<FormIngredientModel, FormIngredientModel.Validator>
{
	[Inject] protected IMediator Mediator { get; set; } = default!;
	[Parameter] public bool IsAdding { get; set; }
	[Parameter] public EventCallback OnDeleted { get; set; }

	protected string initialAmoutLabel = string.Empty;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		initialAmoutLabel = Model.AmountLabel;
	}

	protected void HandleAmountUnitChanged(MeasurementUnits unit)
	{
		Model.AmountUnit = unit;
		if (unit != MeasurementUnits.Item)
		{
			Model.AmountLabel = unit.Label();
		}
		else if(Model.AmountUnit == MeasurementUnits.Item)
		{
			Model.AmountLabel = initialAmoutLabel;
		}
		else
		{
			Model.AmountLabel = string.Empty;
		}
	}

	protected async Task HandleDeleteClicked()
	{
		await OnDeleted.InvokeAsync();
	}

	protected async Task<IEnumerable<string>> SearchIngredients(string value, CancellationToken cancellationToken)
	{
		var results = await Mediator.Send(new SearchIngredients.Query(value)) 
			?? new List<string>();

		return results;
	}
}