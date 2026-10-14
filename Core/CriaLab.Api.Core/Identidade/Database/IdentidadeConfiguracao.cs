using CriaLab.Api.Core.Identidade.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriaLab.Api.Core.Identidade.Database;

public class UsuarioConfiguracao : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nome).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(180).IsRequired();
        builder.Property(x => x.SenhaHash).HasMaxLength(500).IsRequired();
        builder.Property(x => x.TokenConfirmacaoHash).HasMaxLength(128);
        builder.Property(x => x.TokenRecuperacaoHash).HasMaxLength(128);
        builder.HasIndex(x => x.Email).IsUnique().HasFilter("[ExcluidoEm] IS NULL");
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
        builder.HasMany(x => x.Papeis).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId);
    }
}

public class UsuarioPapelConfiguracao : IEntityTypeConfiguration<UsuarioPapel>
{
    public void Configure(EntityTypeBuilder<UsuarioPapel> builder)
    {
        builder.ToTable("UsuarioPapeis");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Papel).HasMaxLength(40).IsRequired();
        builder.HasIndex(x => new { x.UsuarioId, x.Papel }).IsUnique().HasFilter("[ExcluidoEm] IS NULL");
        builder.HasQueryFilter(x => x.ExcluidoEm == null && x.Usuario!.ExcluidoEm == null);
    }
}

public class RefreshTokenConfiguracao : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
    }
}
