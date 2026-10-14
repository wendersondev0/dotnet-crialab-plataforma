using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Lojas.Domain;
using CriaLab.Api.Core.Planos.Domain;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Database;

public class CriaLabDbContext(DbContextOptions<CriaLabDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UsuarioPapel> UsuarioPapeis => Set<UsuarioPapel>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Loja> Lojas => Set<Loja>();
    public DbSet<LojaMembro> LojaMembros => Set<LojaMembro>();
    public DbSet<RedeSocialLoja> RedesSociais => Set<RedeSocialLoja>();
    public DbSet<ConviteColaborador> Convites => Set<ConviteColaborador>();
    public DbSet<Plano> Planos => Set<Plano>();
    public DbSet<AssinaturaLoja> Assinaturas => Set<AssinaturaLoja>();
    public DbSet<FaturaMensalidade> Faturas => Set<FaturaMensalidade>();
    public DbSet<ConfiguracaoPlataforma> Configuracoes => Set<ConfiguracaoPlataforma>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CriaLabDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var agora = DateTime.UtcNow;
        foreach (var entrada in ChangeTracker.Entries<Comum.Entidade>())
        {
            if (entrada.State == EntityState.Added)
            {
                if (entrada.Entity.Id == Guid.Empty)
                    entrada.Entity.Id = Guid.NewGuid();
                if (entrada.Entity.CriadoEm == default)
                    entrada.Entity.CriadoEm = agora;
            }
            else if (entrada.State == EntityState.Modified)
            {
                entrada.Entity.AtualizadoEm = agora;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
