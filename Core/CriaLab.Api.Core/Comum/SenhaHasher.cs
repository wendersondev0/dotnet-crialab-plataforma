using System.Security.Cryptography;

namespace CriaLab.Api.Core.Comum;

public static class SenhaHasher
{
    private const int Iteracoes = 100_000;
    private const int SaltBytes = 16;
    private const int HashBytes = 32;

    public static string Gerar(string senha)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltBytes);
        var hash = Rfc2898DeriveBytes.Pbkdf2(senha, salt, Iteracoes, HashAlgorithmName.SHA256, HashBytes);
        return $"pbkdf2${Iteracoes}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public static bool Conferir(string senha, string armazenada)
    {
        var partes = armazenada.Split('$');
        if (partes.Length != 4 || partes[0] != "pbkdf2" || !int.TryParse(partes[1], out var iteracoes))
            return false;

        var salt = Convert.FromBase64String(partes[2]);
        var esperado = Convert.FromBase64String(partes[3]);
        var atual = Rfc2898DeriveBytes.Pbkdf2(senha, salt, iteracoes, HashAlgorithmName.SHA256, esperado.Length);
        return CryptographicOperations.FixedTimeEquals(atual, esperado);
    }
}
