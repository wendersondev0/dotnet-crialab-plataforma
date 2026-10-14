using CriaLab.Api.Core.Identidade;
using CriaLab.Api.Core.Identidade.Commands;
using CriaLab.Api.Core.Identidade.Queries;
using CriaLab.Api.Core.Lojas.Commands;
using CriaLab.Api.Core.Lojas.Queries;
using CriaLab.Api.Core.Planos.Commands;
using CriaLab.Api.Presentation.WebApi.Identidade;
using CriaLab.Api.Presentation.WebApi.Mensalidades;

namespace CriaLab.Api.Presentation.WebApi;

public static class CriaLabServicos
{
    public static IServiceCollection AddCriaLab(this IServiceCollection services)
    {
        services.AddScoped<IEmissorAcesso, EmissorAcesso>();
        services.AddScoped<CadastrarUsuario>();
        services.AddScoped<Entrar>();
        services.AddScoped<AtualizarSessao>();
        services.AddScoped<ConfirmarEmail>();
        services.AddScoped<EsquecerSenha>();
        services.AddScoped<RedefinirSenha>();
        services.AddScoped<ObterUsuarioAtual>();
        services.AddScoped<CriarLoja>();
        services.AddScoped<ConvidarColaborador>();
        services.AddScoped<AceitarConvite>();
        services.AddScoped<ListarVitrine>();
        services.AddScoped<ObterLojaPorSlug>();
        services.AddScoped<ObterMinhaLoja>();
        services.AddScoped<GerarMensalidades>();
        services.AddScoped<ObterConfiguracaoPlataforma>();
        services.AddScoped<AlterarConfiguracaoPlataforma>();
        services.AddScoped<AlterarPlanoLoja>();
        services.AddHostedService<MensalidadeJob>();
        return services;
    }
}
