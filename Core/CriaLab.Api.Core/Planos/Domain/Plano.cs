using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Planos.Domain;

public class Plano : Entidade
{
    public string Codigo { get; set; } = "";
    public string Nome { get; set; } = "";
    public decimal Mensalidade { get; set; }
    public bool PublicaVitrine { get; set; }
    public bool VendeArquivo3d { get; set; }
    public bool Ativo { get; set; } = true;

    public static Plano NovoAtelie(decimal mensalidade) => new()
    {
        Codigo = "Atelie",
        Nome = "Ateliê",
        Mensalidade = mensalidade,
        PublicaVitrine = false,
        VendeArquivo3d = false,
        Ativo = true
    };

    public static Plano NovoLoja(decimal mensalidade) => new()
    {
        Codigo = "Loja",
        Nome = "Loja",
        Mensalidade = mensalidade,
        PublicaVitrine = true,
        VendeArquivo3d = true,
        Ativo = true
    };
}
