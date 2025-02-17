namespace Unshackled.Fitness.My.Client.Models;

public class Fraction
{
	public int Numerator { get; private set; }
	public int Denominator { get; private set; }

	public Fraction(int numerator, int denominator)
	{
		Numerator = numerator;
		Denominator = denominator;
	}
}