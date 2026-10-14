namespace CriaLab.Api.Core.Identidade.Domain;

public static class EmailUsuario
{
    public static string Normalizar(string email) => email.Trim().ToLowerInvariant();

    public static bool Valido(string email)
    {
        var arroba = email.IndexOf('@');
        return arroba > 0 && arroba < email.Length - 3 && !email.Contains(' ') && email.Contains('.');
    }
}
