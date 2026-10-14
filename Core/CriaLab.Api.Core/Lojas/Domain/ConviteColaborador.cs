using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Lojas.Domain;

public class ConviteColaborador : Entidade
{
    public Guid LojaId { get; set; }
    public string Email { get; set; } = "";
    public string TokenHash { get; set; } = "";
    public DateTime ExpiraEm { get; set; }
    public DateTime? AceitoEm { get; set; }
    public bool Pedidos { get; set; }
    public bool Estoque { get; set; }
    public bool Orcamento { get; set; }
    public bool Financeiro { get; set; }
    public bool Patrimonio { get; set; }
}
