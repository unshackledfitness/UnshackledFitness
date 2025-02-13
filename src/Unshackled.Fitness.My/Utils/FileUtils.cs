namespace Unshackled.Studio.Core.Server.Utils;

public static class FileUtils
{
	public static void EnsureDataSourceDirectoryExists(string connString)
	{
		if (string.IsNullOrEmpty(connString))
			return;

		int idxEquals = connString.IndexOf('=');

		if (idxEquals == -1)
			throw new Exception("Invalid data source path string.");

		string path = connString.Substring(idxEquals + 1);
		if (!path.StartsWith("/"))
			path = $"/{path}";

		if (Path.DirectorySeparatorChar != '/')
		{
			path = path.Replace('/', Path.DirectorySeparatorChar);
		}

		string absPath = $"{Directory.GetCurrentDirectory()}{path}";

		int idxLastSlash = absPath.LastIndexOf(Path.DirectorySeparatorChar);
		if (idxLastSlash == -1)
			throw new Exception("Invalid path string.");

		string dirPath = absPath.Substring(0, idxLastSlash);
		Directory.CreateDirectory(dirPath);
	}
}
