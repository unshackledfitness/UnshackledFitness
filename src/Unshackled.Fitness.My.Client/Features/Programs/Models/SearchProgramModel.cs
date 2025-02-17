using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Programs.Models;

public class SearchProgramModel : SearchModel
{
	public string? Title { get; set; }
	public ProgramTypes ProgramType { get; set; } = ProgramTypes.Any;
}