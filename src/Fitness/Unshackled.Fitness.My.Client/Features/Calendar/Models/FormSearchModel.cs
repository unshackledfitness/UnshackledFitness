using Unshackled.Fitness.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Calendar.Models;

public class FormSearchModel : SearchModel
{
	public DateTime? EndDate { get; set; }
	public int NumberOfMonths { get; set; }
}
