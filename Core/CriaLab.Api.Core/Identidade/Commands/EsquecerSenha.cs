using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Identidade.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Identidade.Commands;

public class EsquecerSenha(CriaLabDbContext db)
{
    public async Task<RecuperacaoResultado?> HandleAsync(string email, CancellationToken ct)
    {
        email = EmailUsuario.Normalizar(email ?? "");
        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (usuario is null)
            return null;

        var token = TokenOpaco.Gerar();
        usuario.TokenRecuperacaoHash = TokenOpaco.Hash(token);
        usuario.TokenRecuperacaoExpiraEm = DateTime.UtcNow.AddHours(2);
        await db.SaveChangesAsync(ct);
        return new RecuperacaoResultado(token);
    }
}
