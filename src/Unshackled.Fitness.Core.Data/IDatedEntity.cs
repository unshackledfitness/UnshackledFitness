namespace Unshackled.Fitness.Core.Data;

public interface IDatedEntity
{
	DateTime DateCreatedUtc { get; set; }
	DateTime? DateLastModifiedUtc { get; set; }
}
