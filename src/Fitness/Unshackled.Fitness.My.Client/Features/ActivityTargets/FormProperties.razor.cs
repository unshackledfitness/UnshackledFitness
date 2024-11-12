using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Features.ActivityTargets.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.ActivityTargets;

public class FormPropertiesBase : BaseFormComponent<FormTargetModel, FormTargetModel.Validator>
{
	[Parameter] public List<ActivityTypeListModel> ActivityTypes { get; set; } = [];

	protected void HandleActivityTypeChange(string typeSid)
	{
		if (Model.ActivityTypeSid != typeSid)
		{
			Model.ActivityTypeSid = typeSid;

			// Only change units when adding
			if (string.IsNullOrEmpty(Model.Sid))
			{
				var actType = ActivityTypes.Where(x => x.Sid == typeSid).FirstOrDefault();
				if (actType != null)
				{
					Model.TargetCadenceUnit = actType.DefaultCadenceUnits;
					Model.TargetDistanceUnit = actType.DefaultDistanceUnits;
				}
			}
		}
	}
}