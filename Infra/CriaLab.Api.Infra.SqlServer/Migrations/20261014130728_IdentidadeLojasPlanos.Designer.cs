using System;
using CriaLab.Api.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CriaLab.Api.Infra.SqlServer.Migrations
{
    [DbContext(typeof(CriaLabDbContext))]
    [Migration("20261014130728_IdentidadeLojasPlanos")]
    partial class IdentidadeLojasPlanos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CriaLab.Api.Core.Identidade.Domain.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiraEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RevogadoEm")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("SubstituidoPor")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TokenHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TokenHash")
                        .IsUnique();

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Identidade.Domain.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(180)
                        .HasColumnType("nvarchar(180)");

                    b.Property<DateTime?>("EmailConfirmadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("nvarchar(160)");

                    b.Property<string>("SenhaHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("TokenConfirmacaoExpiraEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("TokenConfirmacaoHash")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime?>("TokenRecuperacaoExpiraEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("TokenRecuperacaoHash")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[ExcluidoEm] IS NULL");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Identidade.Domain.UsuarioPapel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Papel")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId", "Papel")
                        .IsUnique()
                        .HasFilter("[ExcluidoEm] IS NULL");

                    b.ToTable("UsuarioPapeis", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Lojas.Domain.ConviteColaborador", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AceitoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(180)
                        .HasColumnType("nvarchar(180)");

                    b.Property<bool>("Estoque")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiraEm")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Financeiro")
                        .HasColumnType("bit");

                    b.Property<Guid>("LojaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Orcamento")
                        .HasColumnType("bit");

                    b.Property<bool>("Patrimonio")
                        .HasColumnType("bit");

                    b.Property<bool>("Pedidos")
                        .HasColumnType("bit");

                    b.Property<string>("TokenHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("TokenHash")
                        .IsUnique();

                    b.ToTable("ConvitesColaborador", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Lojas.Domain.Loja", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativa")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Capa")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Complemento")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<Guid?>("DonoUsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(180)
                        .HasColumnType("nvarchar(180)");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsentaMensalidade")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("nvarchar(160)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("nvarchar(160)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid>("PlanoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("SeloOficial")
                        .HasColumnType("bit");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Uf")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("Whatsapp")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("PlanoId");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[ExcluidoEm] IS NULL");

                    b.ToTable("Lojas", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Lojas.Domain.LojaMembro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Dono")
                        .HasColumnType("bit");

                    b.Property<bool>("Estoque")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Financeiro")
                        .HasColumnType("bit");

                    b.Property<Guid>("LojaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Orcamento")
                        .HasColumnType("bit");

                    b.Property<bool>("Patrimonio")
                        .HasColumnType("bit");

                    b.Property<bool>("Pedidos")
                        .HasColumnType("bit");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LojaId", "UsuarioId")
                        .IsUnique()
                        .HasFilter("[ExcluidoEm] IS NULL");

                    b.ToTable("LojaMembros", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Lojas.Domain.RedeSocialLoja", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LojaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Rede")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("Visivel")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("LojaId");

                    b.ToTable("RedesSociaisLoja", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Planos.Domain.AssinaturaLoja", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InicioEm")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LojaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlanoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AssinaturasLoja", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Planos.Domain.ConfiguracaoPlataforma", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DescontoAVistaPercentual")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("LimiarFreteGratis")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TaxaVendaPercentual")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TetoFaturamentoDispensaMensalidade")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("ConfiguracaoPlataforma", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Planos.Domain.FaturaMensalidade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("Competencia")
                        .HasColumnType("date");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LojaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("Valor")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("LojaId", "Competencia")
                        .IsUnique()
                        .HasFilter("[ExcluidoEm] IS NULL");

                    b.ToTable("FaturasMensalidade", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Planos.Domain.Plano", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExcluidoEm")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Mensalidade")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<bool>("PublicaVitrine")
                        .HasColumnType("bit");

                    b.Property<bool>("VendeArquivo3d")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("Codigo")
                        .IsUnique()
                        .HasFilter("[ExcluidoEm] IS NULL");

                    b.ToTable("Planos", (string)null);
                });

            modelBuilder.Entity("CriaLab.Api.Core.Identidade.Domain.UsuarioPapel", b =>
                {
                    b.HasOne("CriaLab.Api.Core.Identidade.Domain.Usuario", "Usuario")
                        .WithMany("Papeis")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CriaLab.Api.Core.Lojas.Domain.Loja", b =>
                {
                    b.HasOne("CriaLab.Api.Core.Planos.Domain.Plano", "Plano")
                        .WithMany()
                        .HasForeignKey("PlanoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Plano");
                });

            modelBuilder.Entity("CriaLab.Api.Core.Lojas.Domain.LojaMembro", b =>
                {
                    b.HasOne("CriaLab.Api.Core.Lojas.Domain.Loja", null)
                        .WithMany("Membros")
                        .HasForeignKey("LojaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CriaLab.Api.Core.Lojas.Domain.RedeSocialLoja", b =>
                {
                    b.HasOne("CriaLab.Api.Core.Lojas.Domain.Loja", null)
                        .WithMany("Redes")
                        .HasForeignKey("LojaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CriaLab.Api.Core.Identidade.Domain.Usuario", b =>
                {
                    b.Navigation("Papeis");
                });

            modelBuilder.Entity("CriaLab.Api.Core.Lojas.Domain.Loja", b =>
                {
                    b.Navigation("Membros");

                    b.Navigation("Redes");
                });
#pragma warning restore 612, 618
        }
    }
}
