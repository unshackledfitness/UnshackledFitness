namespace Unshackled.Fitness.Core.Models;

public class MakeItRecipeStepModel
{
	public string Sid { get; set; } = string.Empty;
	public string Instructions { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public bool IsSelected { get; set; } = false;
}
