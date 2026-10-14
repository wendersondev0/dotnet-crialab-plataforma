using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Lojas.Commands;

public class AceitarConvite(CriaLabDbContext db)
{
    public async Task HandleAsync(Guid usuarioId, string token, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new RegraNegocioException("Convite inválido.");

        var convite = await db.Convites.FirstOrDefaultAsync(c => c.TokenHash == TokenOpaco.Hash(token), ct);
        if (convite is null || convite.AceitoEm is not null || convite.ExpiraEm < DateTime.UtcNow)
            throw new RegraNegocioException("Convite inválido ou vencido.");

        var usuario = await db.Usuarios.Include(u => u.Papeis).FirstAsync(u => u.Id == usuarioId, ct);
        if (usuario.Email != convite.Email)
            throw new RegraNegocioException("Entre com a conta do e-mail convidado.", 403);

        if (!await db.LojaMembros.AnyAsync(m => m.LojaId == convite.LojaId && m.UsuarioId == usuarioId, ct))
        {
            db.LojaMembros.Add(new Domain.LojaMembro
            {
                LojaId = convite.LojaId,
                UsuarioId = usuarioId,
                Dono = false,
                Pedidos = convite.Pedidos,
                Estoque = convite.Estoque,
                Orcamento = convite.Orcamento,
                Financeiro = convite.Financeiro,
                Patrimonio = convite.Patrimonio
            });
        }

        if (usuario.Papeis.All(p => p.Papel != Papeis.ColaboradorLoja))
            usuario.Papeis.Add(new UsuarioPapel { Papel = Papeis.ColaboradorLoja });

        convite.AceitoEm = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}
