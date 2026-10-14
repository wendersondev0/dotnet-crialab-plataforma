namespace CriaLab.Api.Core.Comum;

public abstract class Entidade
{
    public Guid Id { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    public DateTime? ExcluidoEm { get; set; }
}
