using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Lojas.Domain;
using CriaLab.Api.Core.Lojas.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Lojas.Commands;

public class ConvidarColaborador(CriaLabDbContext db)
{
    public async Task<ConviteCriadoDto> HandleAsync(Guid usuarioId, PermissoesColaborador permissoes, string email, CancellationToken ct)
    {
        email = EmailUsuario.Normalizar(email ?? "");
        if (!EmailUsuario.Valido(email))
            throw new RegraNegocioException("Informe um e-mail válido.");

        var loja = await LojaDoDono(db, usuarioId, ct);
        var token = TokenOpaco.Gerar();
        var convite = new ConviteColaborador
        {
            LojaId = loja.Id,
            Email = email,
            TokenHash = TokenOpaco.Hash(token),
            ExpiraEm = DateTime.UtcNow.AddDays(7),
            Pedidos = permissoes.Pedidos,
            Estoque = permissoes.Estoque,
            Orcamento = permissoes.Orcamento,
            Financeiro = permissoes.Financeiro,
            Patrimonio = permissoes.Patrimonio
        };
        db.Convites.Add(convite);
        await db.SaveChangesAsync(ct);
        return new ConviteCriadoDto(convite.Id, convite.Email, token);
    }

    internal static async Task<Loja> LojaDoDono(CriaLabDbContext db, Guid usuarioId, CancellationToken ct)
    {
        var membro = await db.LojaMembros.FirstOrDefaultAsync(m => m.UsuarioId == usuarioId && m.Dono, ct);
        if (membro is null)
            throw new RegraNegocioException("Só o dono da loja convida colaboradores.", 403);
        return await db.Lojas.FirstAsync(l => l.Id == membro.LojaId, ct);
    }
}
