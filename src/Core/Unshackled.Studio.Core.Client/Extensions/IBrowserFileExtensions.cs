using Microsoft.AspNetCore.Components.Forms;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Studio.Core.Client.Extensions;

public static class IFormFileExtensions
{
	// For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
	// and the official specifications for the file types you wish to add.
	private static readonly Dictionary<string, List<byte[]>> fileSignature = new()
	{
		{ ".jpeg", new List<byte[]>
			{
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
			}
		},
		{ ".jpg", new List<byte[]>
			{
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
			}
		}
	};

	public static bool IsValidFileExtensionAndSignature(this string fileName, Stream data, string[] permittedExtensions)
	{
		if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
			return false;

		string ext = Path.GetExtension(fileName).ToLowerInvariant();

		if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
			return false;

		data.Position = 0;

		using var reader = new BinaryReader(data);
		// Uncomment the following code block if you must permit
		// files whose signature isn't provided in the _fileSignature
		// dictionary. We recommend that you add file signatures
		// for files (when possible) for all file types you intend
		// to allow on the system and perform the file signature
		// check.
		/*
		if (!_fileSignature.ContainsKey(ext))
		{
			return true;
		}
		*/

		// File signature check
		// --------------------
		// With the file signatures provided in the _fileSignature
		// dictionary, the following code tests the input content's
		// file signature.
		var signatures = fileSignature[ext];
		byte[] headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

		return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
	}

	public static async Task<CommandResult> ProcessFormFile(this IBrowserFile formFile, string[] permittedExtensions, long sizeLimit)
	{
		if (formFile.Size > sizeLimit)
		{
			long megabyteSizeLimit = sizeLimit / 1048576;
			return new CommandResult(false, $"File exceeds {megabyteSizeLimit:N1} MB.");
		}

		try
		{
			using (var memoryStream = new MemoryStream())
			{
				await formFile.OpenReadStream(sizeLimit).CopyToAsync(memoryStream);

				// Check the content length in case the file's only
				// content was a BOM and the content is actually
				// empty after removing the BOM.
				if (memoryStream.Length == 0)
					return new CommandResult(false, $"File is empty.");

				if (!formFile.Name.IsValidFileExtensionAndSignature(memoryStream, permittedExtensions))
					return new CommandResult(false, $"File type isn't permitted or the file's signature doesn't match the file's extension.");
				else
					return new CommandResult(true, "File is valid");
			}
		}
		catch (Exception ex)
		{
			return new CommandResult(false, $"Upload failed. Please contact us for support. Error: {ex.Message}");
		}
	}
}
