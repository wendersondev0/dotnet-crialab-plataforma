using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Lojas.Domain;

public static class SlugLoja
{
    public static readonly string[] Reservados =
    [
        "painel", "conta", "carrinho", "checkout", "busca", "produto",
        "arquivos-3d", "revendedor", "contato", "planos", "admin", "api", "crialabdm"
    ];

    public static string Normalizar(string valor)
    {
        var semAcento = RemoverAcento(valor.Trim().ToLowerInvariant());
        var convertido = new string(semAcento.Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray());
        while (convertido.Contains("--", StringComparison.Ordinal))
            convertido = convertido.Replace("--", "-", StringComparison.Ordinal);
        return convertido.Trim('-');
    }

    public static void Validar(string slug)
    {
        if (slug.Length < 3 || slug.Length > 60 || Reservados.Contains(slug) || !Regex.IsMatch(slug, "^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            throw new RegraNegocioException("Escolha outro endereço para a loja.");
    }

    private static string RemoverAcento(string texto)
    {
        var form = texto.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in form)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
