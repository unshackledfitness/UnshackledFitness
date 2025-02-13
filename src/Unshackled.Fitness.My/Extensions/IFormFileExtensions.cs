using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Extensions;

public static class IFormFileExtensions
{
	public static async Task<CommandResult> ProcessFormFile(this IFormFile formFile, string[] permittedExtensions, long sizeLimit)
	{
		// Check the file length. This check doesn't catch files that only have 
		// a BOM as their content.
		if (formFile.Length == 0)
			return new CommandResult(false, $"File is empty.");

		if (formFile.Length > sizeLimit)
		{
			long megabyteSizeLimit = sizeLimit / 1048576;
			return new CommandResult(false, $"File exceeds {megabyteSizeLimit:N1} MB.");
		}

		try
		{
			using var memoryStream = new MemoryStream();
			await formFile.CopyToAsync(memoryStream);

			// Check the content length in case the file's only
			// content was a BOM and the content is actually
			// empty after removing the BOM.
			if (memoryStream.Length == 0)
				return new CommandResult(false, $"File is empty.");

			if (!formFile.FileName.IsValidFileExtensionAndSignature(memoryStream, permittedExtensions))
				return new CommandResult(false, $"File type isn't permitted or the file's signature doesn't match the file's extension.");
			else
				return new CommandResult(true, "File is valid");
		}
		catch (Exception ex)
		{
			return new CommandResult(false, $"Upload failed. Please contact us for support. Error: {ex.Message}");
		}
	}
}
