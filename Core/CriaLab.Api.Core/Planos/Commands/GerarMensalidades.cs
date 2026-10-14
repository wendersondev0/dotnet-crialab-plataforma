using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Planos.Domain;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Planos.Commands;

public class GerarMensalidades(CriaLabDbContext db)
{
    public async Task<int> HandleAsync(DateTime agora, CancellationToken ct)
    {
        var competencia = new DateOnly(agora.Year, agora.Month, 1);
        var config = await db.Configuracoes.FirstOrDefaultAsync(ct);
        var teto = config?.TetoFaturamentoDispensaMensalidade ?? 0m;
        var lojas = await db.Lojas.Include(l => l.Plano).Where(l => l.Ativa && !l.IsentaMensalidade).ToListAsync(ct);
        var criadas = 0;

        var pendentes = await db.Faturas.Where(f => f.Status == StatusMensalidade.Pendente && f.Competencia < competencia).ToListAsync(ct);
        foreach (var pendente in pendentes)
            pendente.Status = StatusMensalidade.Atrasado;

        foreach (var loja in lojas)
        {
            var faturamento = 0m;
            var status = StatusMensalidade.Calcular(faturamento, teto);
            var existente = await db.Faturas.FirstOrDefaultAsync(f => f.LojaId == loja.Id && f.Competencia == competencia, ct);
            if (existente is null)
            {
                db.Faturas.Add(new FaturaMensalidade
                {
                    LojaId = loja.Id,
                    Competencia = competencia,
                    Valor = loja.Plano.Mensalidade,
                    Status = status
                });
                criadas++;
            }
            else if (existente.Status is StatusMensalidade.Pendente or StatusMensalidade.Dispensado)
            {
                existente.Valor = loja.Plano.Mensalidade;
                existente.Status = status;
            }
        }

        await db.SaveChangesAsync(ct);
        return criadas;
    }
}
