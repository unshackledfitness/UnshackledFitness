using System.Text.Json.Serialization;
using FluentValidation;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class FormRecipeModel
{
	public string Sid { get; set; } = string.Empty;
	public string HouseholdSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int CookTimeMinutes { get; set; }
	public int PrepTimeMinutes { get; set; }
	public int TotalServings { get; set; }
	public List<string> TagSids { get; set; } = [];

	[JsonIgnore]
	public TimeSpan? PrepTime
	{
		get
		{
			return PrepTimeMinutes > 0 ? TimeSpan.FromMinutes(PrepTimeMinutes) : null;
		}
		set
		{
			PrepTimeMinutes = value.HasValue ? (int)Math.Ceiling(value.Value.TotalMinutes) : 0;
		}
	}

	[JsonIgnore]
	public TimeSpan? CookTime
	{
		get
		{
			return CookTimeMinutes > 0 ? TimeSpan.FromMinutes(CookTimeMinutes) : null;
		}
		set
		{
			CookTimeMinutes = value.HasValue ? (int)Math.Ceiling(value.Value.TotalMinutes) : 0;
		}
	}

	public class Validator : BaseValidator<FormRecipeModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
