namespace Unshackled.Fitness.DataMigrator;

internal static class Msg
{
	public static void WriteHeader(string message)
	{
		Console.WriteLine("----------------------------------------------");
		Console.WriteLine(message);
		Console.WriteLine("----------------------------------------------");
	}

	public static void WriteOngoing(string message)
	{
		Console.Write($"{message}...");
	}

	public static void WriteDot()
	{
		Console.Write(".");
	}

	public static void WriteComplete()
	{
		Console.WriteLine("complete.");
		Console.WriteLine();
	}

	public static void WriteComplete(string message)
	{
		Console.WriteLine($"{message}.");
		Console.WriteLine();
	}
}
