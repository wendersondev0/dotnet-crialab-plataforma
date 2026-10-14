using System.Security.Cryptography;
using System.Text;

namespace CriaLab.Api.Core.Comum;

public static class TokenOpaco
{
    public static string Gerar() => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    public static string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}
