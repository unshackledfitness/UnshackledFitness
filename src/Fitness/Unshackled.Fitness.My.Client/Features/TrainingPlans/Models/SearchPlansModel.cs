using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class SearchPlansModel : SearchModel
{
	public string? Title { get; set; }
	public ProgramTypes ProgramType { get; set; } = ProgramTypes.Any;
}