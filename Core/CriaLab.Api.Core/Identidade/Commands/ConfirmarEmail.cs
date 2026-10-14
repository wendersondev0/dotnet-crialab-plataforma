using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Identidade.Commands;

public class ConfirmarEmail(CriaLabDbContext db)
{
    public async Task HandleAsync(string token, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new RegraNegocioException("Token de confirmação inválido.");

        var hash = TokenOpaco.Hash(token);
        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.TokenConfirmacaoHash == hash, ct);
        if (usuario is null || usuario.TokenConfirmacaoExpiraEm < DateTime.UtcNow)
            throw new RegraNegocioException("Token de confirmação inválido ou vencido.");

        usuario.EmailConfirmadoEm = DateTime.UtcNow;
        usuario.TokenConfirmacaoHash = null;
        usuario.TokenConfirmacaoExpiraEm = null;
        await db.SaveChangesAsync(ct);
    }
}
