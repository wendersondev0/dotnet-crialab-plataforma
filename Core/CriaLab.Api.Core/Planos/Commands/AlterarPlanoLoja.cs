using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Planos.Commands;

public class AlterarPlanoLoja(CriaLabDbContext db, GerarMensalidades mensalidades)
{
    public async Task HandleAsync(Guid usuarioId, string codigo, CancellationToken ct)
    {
        var membro = await db.LojaMembros.FirstOrDefaultAsync(m => m.UsuarioId == usuarioId && m.Dono, ct);
        if (membro is null)
            throw new RegraNegocioException("Só o dono altera o plano da loja.", 403);

        var loja = await db.Lojas.Include(l => l.Plano).FirstAsync(l => l.Id == membro.LojaId, ct);
        if (codigo != "Loja")
            throw new RegraNegocioException("A troca disponível é subir para o plano Loja.");
        if (loja.Plano.Codigo == "Loja")
            return;

        var destino = await db.Planos.FirstAsync(p => p.Codigo == "Loja" && p.Ativo, ct);
        loja.PlanoId = destino.Id;
        var assinatura = await db.Assinaturas.OrderByDescending(a => a.InicioEm).FirstOrDefaultAsync(a => a.LojaId == loja.Id, ct);
        if (assinatura is null)
            db.Assinaturas.Add(new Domain.AssinaturaLoja { LojaId = loja.Id, PlanoId = destino.Id, InicioEm = DateTime.UtcNow });
        else
            assinatura.PlanoId = destino.Id;

        await db.SaveChangesAsync(ct);
        await mensalidades.HandleAsync(DateTime.UtcNow, ct);
    }
}
