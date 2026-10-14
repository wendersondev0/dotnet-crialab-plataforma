using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Identidade.Domain;

public class Usuario : Entidade
{
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string SenhaHash { get; set; } = "";
    public DateTime? EmailConfirmadoEm { get; set; }
    public string? TokenConfirmacaoHash { get; set; }
    public DateTime? TokenConfirmacaoExpiraEm { get; set; }
    public string? TokenRecuperacaoHash { get; set; }
    public DateTime? TokenRecuperacaoExpiraEm { get; set; }
    public List<UsuarioPapel> Papeis { get; set; } = [];
}
