using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Lojas.Domain;
using CriaLab.Api.Core.Planos.Commands;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Lojas.Commands;

public class CriarLoja(CriaLabDbContext db, GerarMensalidades mensalidades)
{
    public async Task<Guid> HandleAsync(Guid usuarioId, string nome, string slugInformado, CancellationToken ct)
    {
        nome = (nome ?? "").Trim();
        if (nome.Length < 2)
            throw new RegraNegocioException("Informe o nome da loja.");

        var slug = SlugLoja.Normalizar(slugInformado ?? "");
        SlugLoja.Validar(slug);

        if (await db.LojaMembros.AnyAsync(m => m.UsuarioId == usuarioId && m.Dono, ct))
            throw new RegraNegocioException("Esta conta já tem uma loja.");
        if (await db.Lojas.AnyAsync(l => l.Slug == slug, ct))
            throw new RegraNegocioException("Este endereço de loja já está em uso.", 409);

        var plano = await db.Planos.FirstAsync(p => p.Codigo == "Atelie" && p.Ativo, ct);
        var loja = new Loja
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Slug = slug,
            PlanoId = plano.Id,
            DonoUsuarioId = usuarioId,
            Ativa = true,
            Descricao = "Ateliê de impressão 3D."
        };
        loja.Membros.Add(new LojaMembro
        {
            UsuarioId = usuarioId,
            Dono = true,
            Pedidos = true,
            Estoque = true,
            Orcamento = true,
            Financeiro = true,
            Patrimonio = true
        });
        db.Lojas.Add(loja);

        var usuario = await db.Usuarios.Include(u => u.Papeis).FirstAsync(u => u.Id == usuarioId, ct);
        if (usuario.Papeis.All(p => p.Papel != Papeis.Vendedor))
            usuario.Papeis.Add(new UsuarioPapel { Papel = Papeis.Vendedor });

        db.Assinaturas.Add(new Planos.Domain.AssinaturaLoja
        {
            LojaId = loja.Id,
            PlanoId = plano.Id,
            InicioEm = DateTime.UtcNow
        });
        await db.SaveChangesAsync(ct);
        await mensalidades.HandleAsync(DateTime.UtcNow, ct);
        return loja.Id;
    }
}
