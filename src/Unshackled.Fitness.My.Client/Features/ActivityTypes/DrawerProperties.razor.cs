using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;
using Unshackled.Studio.Core.Client.Components;

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