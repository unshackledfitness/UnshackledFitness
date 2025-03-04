namespace Unshackled.Fitness.Core.Data;

public interface IDatedEntity
{
	DateTimeOffset DateCreatedUtc { get; set; }
	DateTimeOffset? DateLastModifiedUtc { get; set; }
}
