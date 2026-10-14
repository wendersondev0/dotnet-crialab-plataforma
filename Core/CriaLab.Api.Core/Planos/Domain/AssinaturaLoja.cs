using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Planos.Domain;

public class AssinaturaLoja : Entidade
{
    public Guid LojaId { get; set; }
    public Guid PlanoId { get; set; }
    public DateTime InicioEm { get; set; }
}
