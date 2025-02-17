using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Services;

public class LocalFileStorageService : IFileStorageService
{
	private string GetFullPath(string container, string relativePath)
	{
		if (Path.DirectorySeparatorChar != '/')
		{
			relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
		}
		string currentDir = Directory.GetCurrentDirectory();
		return Path.Combine(currentDir, "wwwroot", container, relativePath);
	}

	public async Task<bool> CopyFile(string sourceContainer, string sourceFilePath, string destContainer, string destFilePath, CancellationToken cancellationToken)
	{
		string sourcePath = GetFullPath(sourceContainer, sourceFilePath);
		string destPath = GetFullPath(destContainer, destFilePath);

		// Source blob doesn't exist
		if (!File.Exists(sourcePath)) 
			return false;

		string? dirPath = Path.GetDirectoryName(destPath);
		if (string.IsNullOrEmpty(dirPath))
			return false;

		try
		{
			Directory.CreateDirectory(dirPath);
			// Do Copy
			File.Copy(sourcePath, destPath, true);

			return await ValueTask.FromResult(true);
		}
		catch
		{
			return await ValueTask.FromResult(false);
		}
	}

	public async Task<bool> DeleteDirectory(string container, string relativePath, CancellationToken cancellationToken)
	{
		string fullPath = GetFullPath(container, relativePath);
		if (!fullPath.EndsWith(Path.DirectorySeparatorChar))
			fullPath += Path.DirectorySeparatorChar;

		try
		{
			if (Directory.Exists(fullPath))
			{
				Directory.Delete(fullPath, true);
			}
			return await ValueTask.FromResult(true);
		}
		catch 
		{
			return await ValueTask.FromResult(false);
		}
	}

	public async Task<bool> DeleteFile(string container, string relativePath, CancellationToken cancellationToken)
	{
		string fullPath = GetFullPath(container, relativePath);
		try
		{
			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
			}
			return await ValueTask.FromResult(true);
		}
		catch
		{
			return await ValueTask.FromResult(false);
		}
	}

	public async Task<CommandResult> SaveFile(string container, string relativePath, string contentType, byte[] data, CancellationToken cancellationToken)
	{
		string filePath = GetFullPath(container, relativePath);

		string? dirPath = Path.GetDirectoryName(filePath);
		if (string.IsNullOrEmpty(dirPath))
			return new CommandResult(false, "Invalid file path.");

		try
		{
			Directory.CreateDirectory(dirPath);
			await File.WriteAllBytesAsync(filePath, data, cancellationToken);
			return new CommandResult(true, "File saved.");
		}
		catch (Exception ex)
		{
			return new CommandResult(false, ex.Message);
		}
	}

	public async Task<CommandResult> SaveFile(string container, string relativePath, string contentType, Stream data, CancellationToken cancellationToken)
	{
		string filePath = GetFullPath(container, relativePath);

		string? dirPath = Path.GetDirectoryName(filePath);
		if (string.IsNullOrEmpty(dirPath))
			return new CommandResult(false, "Invalid file path.");

		try
		{
			Directory.CreateDirectory(dirPath);
			using var stream = new FileStream(filePath, FileMode.Create);
			data.Position = 0;
			await data.CopyToAsync(stream, cancellationToken);
			return new CommandResult(true, "File saved.");
		}
		catch (Exception ex)
		{
			return new CommandResult(false, ex.Message);
		}
	}
}
