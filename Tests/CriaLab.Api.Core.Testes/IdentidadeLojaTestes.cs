using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Lojas.Domain;
using CriaLab.Api.Core.Planos.Domain;

namespace CriaLab.Api.Core.Testes;

public class IdentidadeLojaTestes
{
    [Fact]
    public void SenhaNaoFicaGuardadaEmClaro()
    {
        var hash = SenhaHasher.Gerar("segredo-local");
        Assert.DoesNotContain("segredo-local", hash);
        Assert.True(SenhaHasher.Conferir("segredo-local", hash));
        Assert.False(SenhaHasher.Conferir("outra-senha", hash));
    }

    [Fact]
    public void SlugRemoveAcentoERecusaReservado()
    {
        Assert.Equal("oficina-teste", SlugLoja.Normalizar("Oficina Teste"));
        Assert.Equal("sao-paulo", SlugLoja.Normalizar("São Paulo"));
        Assert.Throws<RegraNegocioException>(() => SlugLoja.Validar("admin"));
        Assert.Throws<RegraNegocioException>(() => SlugLoja.Validar("crialabdm"));
    }

    [Fact]
    public void AtelieNaoEntraNaVitrineELojaEntra()
    {
        var atelie = Plano.NovoAtelie(49m);
        var loja = Plano.NovoLoja(99m);
        Assert.False(VitrineRegra.Aparece(true, atelie.PublicaVitrine));
        Assert.False(atelie.VendeArquivo3d);
        Assert.True(VitrineRegra.Aparece(true, loja.PublicaVitrine));
        Assert.True(loja.VendeArquivo3d);
        Assert.False(VitrineRegra.Aparece(false, loja.PublicaVitrine));
    }

    [Fact]
    public void MensalidadeDispensaQuandoFaturamentoAtingeOTeto()
    {
        Assert.Equal(StatusMensalidade.Pendente, StatusMensalidade.Calcular(1999.99m, 2000m));
        Assert.Equal(StatusMensalidade.Dispensado, StatusMensalidade.Calcular(2000m, 2000m));
    }
}
