using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Planos.Domain;

public class FaturaMensalidade : Entidade
{
    public Guid LojaId { get; set; }
    public DateOnly Competencia { get; set; }
    public decimal Valor { get; set; }
    public string Status { get; set; } = StatusMensalidade.Pendente;
}
