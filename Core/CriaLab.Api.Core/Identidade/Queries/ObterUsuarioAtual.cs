using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Identidade.Queries;

public class ObterUsuarioAtual(CriaLabDbContext db)
{
    public async Task<UsuarioDto> HandleAsync(Guid usuarioId, CancellationToken ct)
    {
        var usuario = await db.Usuarios.Include(u => u.Papeis).FirstOrDefaultAsync(u => u.Id == usuarioId, ct);
        if (usuario is null)
            throw new RegraNegocioException("Conta não encontrada.", 401);
        var lojaId = await db.LojaMembros.Where(m => m.UsuarioId == usuarioId).Select(m => (Guid?)m.LojaId).FirstOrDefaultAsync(ct);
        return UsuarioDto.De(usuario, lojaId);
    }
}
