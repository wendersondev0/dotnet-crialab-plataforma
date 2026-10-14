using CriaLab.Api.Core.Comum;

namespace CriaLab.Api.Core.Planos.Domain;

public class ConfiguracaoPlataforma : Entidade
{
    public decimal TaxaVendaPercentual { get; set; }
    public decimal LimiarFreteGratis { get; set; }
    public decimal DescontoAVistaPercentual { get; set; }
    public decimal TetoFaturamentoDispensaMensalidade { get; set; }
}
