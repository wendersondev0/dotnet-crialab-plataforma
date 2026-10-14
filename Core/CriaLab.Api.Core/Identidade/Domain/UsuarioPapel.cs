using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Identidade.Domain;

public class UsuarioPapel : Entidade
{
    public Guid UsuarioId { get; set; }
    public string Papel { get; set; } = "";
    public Usuario? Usuario { get; set; }
}
