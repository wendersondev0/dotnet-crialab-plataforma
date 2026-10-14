using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Lojas.Domain;

public class LojaMembro : Entidade
{
    public Guid LojaId { get; set; }
    public Guid UsuarioId { get; set; }
    public bool Dono { get; set; }
    public bool Pedidos { get; set; }
    public bool Estoque { get; set; }
    public bool Orcamento { get; set; }
    public bool Financeiro { get; set; }
    public bool Patrimonio { get; set; }
}
