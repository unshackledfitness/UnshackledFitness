using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.ActivityTemplates;

public class FormPropertiesBase : BaseFormComponent<FormTemplateModel, FormTemplateModel.Validator>
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
					Model.EventType = actType.DefaultEventType;
					Model.TargetCadenceUnit = actType.DefaultCadenceUnits;
					Model.TargetDistanceUnit = actType.DefaultDistanceUnits;
				}
			}
		}
	}
}