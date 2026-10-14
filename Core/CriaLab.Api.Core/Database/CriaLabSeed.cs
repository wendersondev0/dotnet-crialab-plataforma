using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Lojas.Domain;
using CriaLab.Api.Core.Planos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CriaLab.Api.Core.Database;

public class CriaLabSeed(CriaLabDbContext db, IConfiguration configuracao)
{
    public static readonly Guid PlanoAtelie = Guid.Parse("11111111-1111-4111-8111-111111111101");
    public static readonly Guid PlanoLoja = Guid.Parse("11111111-1111-4111-8111-111111111102");
    public static readonly Guid ConfiguracaoId = Guid.Parse("11111111-1111-4111-8111-111111111201");
    public static readonly Guid LojaOficial = Guid.Parse("11111111-1111-4111-8111-111111111301");

    public async Task AplicarAsync(CancellationToken ct = default)
    {
        if (!await db.Planos.AnyAsync(ct))
        {
            var atelie = Plano.NovoAtelie(49m);
            atelie.Id = PlanoAtelie;
            var loja = Plano.NovoLoja(99m);
            loja.Id = PlanoLoja;
            db.Planos.AddRange(atelie, loja);
        }

        if (!await db.Configuracoes.AnyAsync(ct))
        {
            db.Configuracoes.Add(new ConfiguracaoPlataforma
            {
                Id = ConfiguracaoId,
                TaxaVendaPercentual = 10m,
                LimiarFreteGratis = 200m,
                DescontoAVistaPercentual = 10m,
                TetoFaturamentoDispensaMensalidade = 2000m
            });
        }

        if (!await db.Lojas.IgnoreQueryFilters().AnyAsync(l => l.Id == LojaOficial, ct))
        {
            db.Lojas.Add(new Loja
            {
                Id = LojaOficial,
                Nome = "Cria Lab DM",
                Slug = "crialabdm",
                Descricao = "Impressão 3D personalizada, do orçamento à peça pronta.",
                Logo = "/marca/logo.png",
                Capa = "/marca/template-postagem.png",
                Cnpj = "68.914.190/0001-96",
                Telefone = "(27) 99653-0954",
                Whatsapp = "(27) 99653-0954",
                Email = "crialab.dm@gmail.com",
                SeloOficial = true,
                Ativa = true,
                IsentaMensalidade = true,
                PlanoId = PlanoLoja,
                Redes =
                [
                    Rede("Instagram", "https://www.instagram.com/crialab.dm"),
                    Rede("TikTok", "https://www.tiktok.com/@crialab.dm"),
                    Rede("YouTube", "https://www.youtube.com/@crialabdm"),
                    Rede("Shopee", "https://shopee.com.br/shop/1648584425")
                ]
            });
        }

        await db.SaveChangesAsync(ct);
        await GarantirAdministrador(ct);
    }

    private async Task GarantirAdministrador(CancellationToken ct)
    {
        var email = EmailUsuario.Normalizar(configuracao["Admin:Email"] ?? "");
        var senha = configuracao["Admin:Senha"];
        if (!EmailUsuario.Valido(email) || string.IsNullOrWhiteSpace(senha))
            return;

        var usuario = await db.Usuarios.Include(u => u.Papeis).FirstOrDefaultAsync(u => u.Email == email, ct);
        if (usuario is not null)
            return;

        db.Usuarios.Add(new Usuario
        {
            Nome = "Cria Lab DM",
            Email = email,
            SenhaHash = SenhaHasher.Gerar(senha),
            EmailConfirmadoEm = DateTime.UtcNow,
            Papeis = [new UsuarioPapel { Papel = Papeis.AdministradorGeral }, new UsuarioPapel { Papel = Papeis.Cliente }]
        });
        await db.SaveChangesAsync(ct);
    }

    private static RedeSocialLoja Rede(string nome, string url) => new() { Rede = nome, Url = url, Visivel = true };
}
