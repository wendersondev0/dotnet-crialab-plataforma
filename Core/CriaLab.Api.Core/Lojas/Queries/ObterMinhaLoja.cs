using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Lojas.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Lojas.Queries;

public class ObterMinhaLoja(CriaLabDbContext db)
{
    public async Task<MinhaLojaDto?> HandleAsync(Guid usuarioId, CancellationToken ct)
    {
        var membro = await db.LojaMembros.AsNoTracking().FirstOrDefaultAsync(m => m.UsuarioId == usuarioId, ct);
        if (membro is null)
            return null;

        var loja = await db.Lojas.AsNoTracking().Include(l => l.Plano).FirstAsync(l => l.Id == membro.LojaId, ct);
        var faturas = await db.Faturas.AsNoTracking()
            .Where(f => f.LojaId == loja.Id)
            .OrderByDescending(f => f.Competencia)
            .Select(f => new FaturaDto(f.Id, f.Competencia, f.Valor, f.Status))
            .ToListAsync(ct);
        var convites = membro.Dono
            ? await db.Convites.AsNoTracking()
                .Where(c => c.LojaId == loja.Id)
                .OrderByDescending(c => c.CriadoEm)
                .Select(c => new ConviteDto(c.Id, c.Email, c.AceitoEm != null, new PermissoesColaborador(c.Pedidos, c.Estoque, c.Orcamento, c.Financeiro, c.Patrimonio)))
                .ToListAsync(ct)
            : [];

        return new MinhaLojaDto(
            loja.Id,
            loja.Nome,
            loja.Slug,
            loja.Plano.Nome,
            loja.Plano.Codigo,
            loja.Plano.Mensalidade,
            loja.Plano.PublicaVitrine,
            loja.Plano.VendeArquivo3d,
            membro.Dono,
            new PermissoesColaborador(membro.Pedidos, membro.Estoque, membro.Orcamento, membro.Financeiro, membro.Patrimonio),
            faturas,
            convites);
    }
}
