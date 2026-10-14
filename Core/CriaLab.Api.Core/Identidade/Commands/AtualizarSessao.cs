using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Identidade.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Identidade.Commands;

public class AtualizarSessao(CriaLabDbContext db, IEmissorAcesso emissor)
{
    public async Task<SessaoDto> HandleAsync(string refreshToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new RegraNegocioException("Sessão inválida. Entre novamente.", 401);

        var hash = TokenOpaco.Hash(refreshToken);
        var atual = await db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash, ct);
        if (atual is null || atual.ExpiraEm < DateTime.UtcNow)
            throw new RegraNegocioException("Sessão inválida. Entre novamente.", 401);

        if (atual.RevogadoEm is not null)
        {
            var ativos = await db.RefreshTokens.Where(t => t.UsuarioId == atual.UsuarioId && t.RevogadoEm == null).ToListAsync(ct);
            foreach (var token in ativos)
                token.RevogadoEm = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
            throw new RegraNegocioException("Sessão inválida. Entre novamente.", 401);
        }

        var usuario = await db.Usuarios.Include(u => u.Papeis).FirstAsync(u => u.Id == atual.UsuarioId, ct);
        atual.RevogadoEm = DateTime.UtcNow;
        var sessao = await Sessao.Emitir(db, emissor, usuario, ct);
        atual.SubstituidoPor = await db.RefreshTokens
            .Where(t => t.UsuarioId == usuario.Id && t.RevogadoEm == null)
            .OrderByDescending(t => t.CriadoEm)
            .Select(t => (Guid?)t.Id)
            .FirstOrDefaultAsync(ct);
        await db.SaveChangesAsync(ct);
        return sessao;
    }
}
