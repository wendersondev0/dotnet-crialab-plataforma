using CriaLab.Api.Core.Planos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriaLab.Api.Core.Planos.Database;

public class PlanoConfiguracao : IEntityTypeConfiguration<Plano>
{
    public void Configure(EntityTypeBuilder<Plano> builder)
    {
        builder.ToTable("Planos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Codigo).HasMaxLength(40).IsRequired();
        builder.Property(x => x.Nome).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Mensalidade).HasPrecision(18, 2);
        builder.HasIndex(x => x.Codigo).IsUnique().HasFilter("[ExcluidoEm] IS NULL");
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
    }
}

public class AssinaturaLojaConfiguracao : IEntityTypeConfiguration<AssinaturaLoja>
{
    public void Configure(EntityTypeBuilder<AssinaturaLoja> builder)
    {
        builder.ToTable("AssinaturasLoja");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
    }
}

public class FaturaMensalidadeConfiguracao : IEntityTypeConfiguration<FaturaMensalidade>
{
    public void Configure(EntityTypeBuilder<FaturaMensalidade> builder)
    {
        builder.ToTable("FaturasMensalidade");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Valor).HasPrecision(18, 2);
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => new { x.LojaId, x.Competencia }).IsUnique().HasFilter("[ExcluidoEm] IS NULL");
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
    }
}

public class ConfiguracaoPlataformaConfiguracao : IEntityTypeConfiguration<ConfiguracaoPlataforma>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoPlataforma> builder)
    {
        builder.ToTable("ConfiguracaoPlataforma");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TaxaVendaPercentual).HasPrecision(18, 2);
        builder.Property(x => x.LimiarFreteGratis).HasPrecision(18, 2);
        builder.Property(x => x.DescontoAVistaPercentual).HasPrecision(18, 2);
        builder.Property(x => x.TetoFaturamentoDispensaMensalidade).HasPrecision(18, 2);
        builder.HasQueryFilter(x => x.ExcluidoEm == null);
    }
}
