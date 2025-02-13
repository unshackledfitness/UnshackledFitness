using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTypes;

public class DrawerPropertiesBase : BaseFormComponent<FormActivityTypeModel, FormActivityTypeModel.Validator>
{
	[Parameter] public string Title { get; set; } = string.Empty;
	[Parameter] public string SubmitLabel { get; set; } = "Submit";

	protected MudColor? EditingColor { get; set; }

	protected void HandleEditingColorChanged(MudColor color)
	{
		EditingColor = color;
		Model.Color = color.Value;
	}
}