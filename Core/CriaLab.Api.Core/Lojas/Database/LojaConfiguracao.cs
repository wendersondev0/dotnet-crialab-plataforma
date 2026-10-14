using CriaLab.Api.Core.Lojas.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriaLab.Api.Core.Lojas.Database;

public class LojaConfiguracao : IEntityTypeConfiguration<Loja>
{
    public void Configure(EntityTypeBuilder<Loja> builder)
    {
        builder.ToTable("Lojas");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nome).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(60).IsRequired();
        builder.Property(x => x.Descricao).HasMaxLength(2000);
        builder.Property(x => x.Logo).HasMaxLength(300);
        builder.Property(x => x.Capa).HasMaxLength(300);
        builder.Property(x => x.Cnpj).HasMaxLength(20);
        builder.Property(x => x.Telefone).HasMaxLength(30);
        builder.Property(x => x.Whatsapp).HasMaxLength(30);
        builder.Property(x => x.Email).HasMaxLength(180);
        builder.Property(x => x.Logradouro).HasMaxLength(160);
        builder.Property(x => x.Numero).HasMaxLength(20);
        builder.Property(x => x.Complemento).HasMaxLength(80);
        builder.Property(x => x.Bairro).HasMaxLength(80);
        builder.Property(x => x.Cidade).HasMaxLength(80);
        builder.Property(x => x.Uf).HasMaxLength(2);
        builder.Property(x => x.Cep).HasMaxLength(9);
        builder.HasIndex(x => x.Slug).IsUnique().HasFilter("[ExcluidoEm] IS NULL");
        builder.HasOne(x => x.Plano).WithMany().HasForeignKey(x => x.PlanoId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Redes).WithOne().HasForeignKey(x => x.LojaId);
        builder.HasMany(x => x.Membros).WithOne().HasForeignKey(x => x.LojaId);
        builder.HasQueryFilter(x => x.ExcluidoEm == null && x.Plano.ExcluidoEm == null);
    }
}

public class LojaMembroConfiguracao : IEntityTypeConfiguration<LojaMembro>
{
    public void Configure(EntityTypeBuilder<LojaMembro> builder)
    {
        builder.ToTable("LojaMembros");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.LojaId, x.UsuarioId }).IsUnique().HasFilter("[ExcluidoEm] IS NULL");
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
    }
}

public class RedeSocialLojaConfiguracao : IEntityTypeConfiguration<RedeSocialLoja>
{
    public void Configure(EntityTypeBuilder<RedeSocialLoja> builder)
    {
        builder.ToTable("RedesSociaisLoja");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Rede).HasMaxLength(40).IsRequired();
        builder.Property(x => x.Url).HasMaxLength(300).IsRequired();
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
    }
}

public class ConviteColaboradorConfiguracao : IEntityTypeConfiguration<ConviteColaborador>
{
    public void Configure(EntityTypeBuilder<ConviteColaborador> builder)
    {
        builder.ToTable("ConvitesColaborador");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).HasMaxLength(180).IsRequired();
        builder.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
    }
}
