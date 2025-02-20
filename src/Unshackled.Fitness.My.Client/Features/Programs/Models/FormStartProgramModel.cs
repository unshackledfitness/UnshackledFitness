namespace Unshackled.Fitness.My.Client.Features.Programs.Models;

public class FormStartProgramModel
{
	public string Sid { get; set; } = string.Empty;
	public DateTimeOffset DateStart { get; set; }
	public int StartingTemplateIndex { get; set; }
}
