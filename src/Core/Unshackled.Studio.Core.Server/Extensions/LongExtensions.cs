using HashidsNet;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Studio.Core.Server.Extensions;

public static class LongExtensions
{
	public static string Encode(this long value)
	{
		var hashids = new Hashids(HashIdSettings.Salt, HashIdSettings.MinLength, HashIdSettings.Alphabet);
		return hashids.EncodeLong(value);
	}
}