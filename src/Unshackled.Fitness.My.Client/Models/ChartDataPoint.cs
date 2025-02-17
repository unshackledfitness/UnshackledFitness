namespace Unshackled.Fitness.My.Client.Models;

public class ChartDataPoint<T> where T : struct
{
	public string X { get; set; } = string.Empty;
	public T Y { get; set; }
}
