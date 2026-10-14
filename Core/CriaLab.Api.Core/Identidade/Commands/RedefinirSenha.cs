using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Identidade.Commands;

public class RedefinirSenha(CriaLabDbContext db)
{
    public async Task HandleAsync(string token, string senha, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(senha) || senha.Length < 8)
            throw new RegraNegocioException("A senha precisa ter pelo menos 8 caracteres.");
        if (string.IsNullOrWhiteSpace(token))
            throw new RegraNegocioException("Token de redefinição inválido.");

        var hash = TokenOpaco.Hash(token);
        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.TokenRecuperacaoHash == hash, ct);
        if (usuario is null || usuario.TokenRecuperacaoExpiraEm < DateTime.UtcNow)
            throw new RegraNegocioException("Token de redefinição inválido ou vencido.");

        usuario.SenhaHash = SenhaHasher.Gerar(senha);
        usuario.TokenRecuperacaoHash = null;
        usuario.TokenRecuperacaoExpiraEm = null;
        if (usuario.EmailConfirmadoEm is null)
            usuario.EmailConfirmadoEm = DateTime.UtcNow;

        var ativos = await db.RefreshTokens.Where(t => t.UsuarioId == usuario.Id && t.RevogadoEm == null).ToListAsync(ct);
        foreach (var refresh in ativos)
            refresh.RevogadoEm = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
    }
}
