using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Planos.Domain;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Planos.Commands;

public sealed record ConfiguracaoPlataformaDto(
    decimal TaxaVendaPercentual,
    decimal LimiarFreteGratis,
    decimal DescontoAVistaPercentual,
    decimal TetoFaturamentoDispensaMensalidade,
    decimal MensalidadeAtelie,
    decimal MensalidadeLoja);

public class ObterConfiguracaoPlataforma(CriaLabDbContext db)
{
    public async Task<ConfiguracaoPlataformaDto> HandleAsync(CancellationToken ct)
    {
        var config = await db.Configuracoes.FirstAsync(ct);
        var atelie = await db.Planos.FirstAsync(p => p.Codigo == "Atelie", ct);
        var loja = await db.Planos.FirstAsync(p => p.Codigo == "Loja", ct);
        return new ConfiguracaoPlataformaDto(
            config.TaxaVendaPercentual,
            config.LimiarFreteGratis,
            config.DescontoAVistaPercentual,
            config.TetoFaturamentoDispensaMensalidade,
            atelie.Mensalidade,
            loja.Mensalidade);
    }
}

public class AlterarConfiguracaoPlataforma(CriaLabDbContext db)
{
    public async Task<ConfiguracaoPlataformaDto> HandleAsync(ConfiguracaoPlataformaDto pedido, CancellationToken ct)
    {
        Validar(pedido.TaxaVendaPercentual, "Taxa de venda");
        Validar(pedido.DescontoAVistaPercentual, "Desconto à vista");
        if (pedido.LimiarFreteGratis < 0 || pedido.TetoFaturamentoDispensaMensalidade < 0 || pedido.MensalidadeAtelie < 0 || pedido.MensalidadeLoja < 0)
            throw new RegraNegocioException("Os valores não podem ser negativos.");

        var config = await db.Configuracoes.FirstAsync(ct);
        config.TaxaVendaPercentual = pedido.TaxaVendaPercentual;
        config.LimiarFreteGratis = pedido.LimiarFreteGratis;
        config.DescontoAVistaPercentual = pedido.DescontoAVistaPercentual;
        config.TetoFaturamentoDispensaMensalidade = pedido.TetoFaturamentoDispensaMensalidade;

        var atelie = await db.Planos.FirstAsync(p => p.Codigo == "Atelie", ct);
        var loja = await db.Planos.FirstAsync(p => p.Codigo == "Loja", ct);
        atelie.Mensalidade = pedido.MensalidadeAtelie;
        loja.Mensalidade = pedido.MensalidadeLoja;
        await db.SaveChangesAsync(ct);
        return pedido;
    }

    private static void Validar(decimal percentual, string nome)
    {
        if (percentual < 0 || percentual > 100)
            throw new RegraNegocioException($"{nome} precisa ficar entre 0 e 100.");
    }
}
