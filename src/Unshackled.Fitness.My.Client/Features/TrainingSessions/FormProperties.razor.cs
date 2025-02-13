using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions;

public class FormPropertiesBase : BaseFormComponent<FormSessionModel, FormSessionModel.Validator>
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