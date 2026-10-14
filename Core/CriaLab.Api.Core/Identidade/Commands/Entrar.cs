using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Identidade.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Identidade.Commands;

public class Entrar(CriaLabDbContext db, IEmissorAcesso emissor)
{
    public async Task<SessaoDto> HandleAsync(string email, string senha, CancellationToken ct)
    {
        email = EmailUsuario.Normalizar(email ?? "");
        var usuario = await db.Usuarios.Include(u => u.Papeis).FirstOrDefaultAsync(u => u.Email == email, ct);
        if (usuario is null || !SenhaHasher.Conferir(senha ?? "", usuario.SenhaHash))
            throw new RegraNegocioException("E-mail ou senha inválidos.", 401);
        if (usuario.EmailConfirmadoEm is null)
            throw new RegraNegocioException("Confirme o e-mail antes de entrar.", 403);

        return await Sessao.Emitir(db, emissor, usuario, ct);
    }
}

public static class Sessao
{
    public static async Task<SessaoDto> Emitir(CriaLabDbContext db, IEmissorAcesso emissor, Usuario usuario, CancellationToken ct)
    {
        var papeis = usuario.Papeis.Select(p => p.Papel).ToArray();
        var (acesso, acessoExpira) = emissor.Emitir(usuario.Id, usuario.Nome, usuario.Email, papeis);
        var refresh = TokenOpaco.Gerar();
        db.RefreshTokens.Add(new RefreshToken
        {
            UsuarioId = usuario.Id,
            TokenHash = TokenOpaco.Hash(refresh),
            ExpiraEm = emissor.RefreshExpiraEm()
        });
        await db.SaveChangesAsync(ct);
        var lojaId = await db.LojaMembros.Where(m => m.UsuarioId == usuario.Id).Select(m => (Guid?)m.LojaId).FirstOrDefaultAsync(ct);
        return new SessaoDto(UsuarioDto.De(usuario, lojaId), acesso, acessoExpira, refresh, emissor.RefreshExpiraEm());
    }
}
