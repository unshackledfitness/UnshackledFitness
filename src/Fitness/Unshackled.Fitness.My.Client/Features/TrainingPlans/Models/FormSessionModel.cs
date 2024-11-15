using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class FormSessionModel : BaseMemberObject, ICloneable, ISortable
{
	public string TrainingPlanSid { get; set; } = string.Empty;
	public string TrainingSessionSid { get; set; } = string.Empty;
	public string TrainingSessionName { get; set; } = string.Empty;
	public int WeekNumber { get; set; }
	public int DayNumber { get; set; }
	public int SortOrder { get; set; }
	public bool IsNew { get; set; }

	public object Clone()
	{
		return new FormSessionModel
		{
			DateCreatedUtc = DateCreatedUtc,
			DateLastModifiedUtc = DateLastModifiedUtc,
			DayNumber = DayNumber,
			IsNew = IsNew,
			MemberSid = MemberSid,
			Sid = Sid,
			SortOrder = SortOrder,
			TrainingPlanSid = TrainingPlanSid,
			TrainingSessionName = TrainingSessionName,
			TrainingSessionSid = TrainingSessionSid,
			WeekNumber = WeekNumber,
		};
	}
}
