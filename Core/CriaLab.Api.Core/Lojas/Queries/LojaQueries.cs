using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Lojas.Domain;
using CriaLab.Api.Core.Lojas.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Lojas.Queries;

public class ListarVitrine(CriaLabDbContext db)
{
    public async Task<IReadOnlyList<LojaPublicaDto>> HandleAsync(CancellationToken ct)
    {
        var lojas = await db.Lojas.AsNoTracking()
            .Include(l => l.Plano)
            .Include(l => l.Redes)
            .Where(l => l.Ativa && l.Plano.PublicaVitrine)
            .OrderByDescending(l => l.SeloOficial)
            .ThenBy(l => l.Nome)
            .ToListAsync(ct);
        return lojas.Where(l => VitrineRegra.Aparece(l.Ativa, l.Plano.PublicaVitrine)).Select(Mapa.Publica).ToList();
    }
}

public class ObterLojaPorSlug(CriaLabDbContext db)
{
    public async Task<LojaPublicaDto?> HandleAsync(string slug, CancellationToken ct)
    {
        var normalizado = SlugLoja.Normalizar(slug ?? "");
        var loja = await db.Lojas.AsNoTracking()
            .Include(l => l.Plano)
            .Include(l => l.Redes)
            .FirstOrDefaultAsync(l => l.Slug == normalizado && l.Ativa, ct);
        return loja is null ? null : Mapa.Publica(loja);
    }
}

public static class Mapa
{
    public static LojaPublicaDto Publica(Loja loja) => new(
        loja.Id,
        loja.Nome,
        loja.Slug,
        loja.Descricao,
        loja.Logo,
        loja.Capa,
        loja.Cnpj,
        loja.Telefone,
        loja.Whatsapp,
        loja.Email,
        loja.SeloOficial,
        loja.Plano.Nome,
        loja.Redes.Where(r => r.Visivel && r.Url != "").Select(r => new RedePublicaDto(r.Rede, r.Url)).ToList());
}
