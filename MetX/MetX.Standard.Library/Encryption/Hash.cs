using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using MetX.Standard.Strings;

namespace MetX.Standard.Library.Encryption;

public static class Hash
{
    private static HashAlgorithm _shaProvider;
    
    /// <summary>
    /// Calculates the SHA has for an array of bytes
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string Calculate(byte[] bytes)
    {
        _shaProvider ??= SHA512.Create();

        return _shaProvider.ComputeHash(bytes).AsStringFromObject();
    }

    /// <summary>
    /// Calculates the SHA has for an enumeration of bytes
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string Calculate(IEnumerable<byte> bytes)
    {
        _shaProvider ??= SHA512.Create();
        return _shaProvider.ComputeHash(bytes.ToArray()).AsStringFromArray();
    }

    public static bool Compare(string hash, byte[] bytes)
    {
        _shaProvider ??= SHA512.Create();
        var arrayHash = Calculate(bytes);
        return hash == arrayHash;
    }

    public static bool Compare(string hash, IEnumerable<byte> bytes)
    {
        _shaProvider ??= SHA512.Create();
        var arrayHash = Calculate(bytes);
        return hash == arrayHash;
    }
}