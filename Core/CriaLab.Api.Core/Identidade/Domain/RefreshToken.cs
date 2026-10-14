using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Identidade.Domain;

public class RefreshToken : Entidade
{
    public Guid UsuarioId { get; set; }
    public string TokenHash { get; set; } = "";
    public DateTime ExpiraEm { get; set; }
    public DateTime? RevogadoEm { get; set; }
    public Guid? SubstituidoPor { get; set; }
}
