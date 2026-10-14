using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Lojas.Domain;

public class RedeSocialLoja : Entidade
{
    public Guid LojaId { get; set; }
    public string Rede { get; set; } = "";
    public string Url { get; set; } = "";
    public bool Visivel { get; set; } = true;
}
