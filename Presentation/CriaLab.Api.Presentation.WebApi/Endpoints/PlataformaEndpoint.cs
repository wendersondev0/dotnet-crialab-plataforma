using Carter;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Planos.Commands;

namespace CriaLab.Api.Presentation.WebApi.Endpoints;

public class PlataformaEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/plataforma/configuracao", async (ObterConfiguracaoPlataforma consulta, CancellationToken ct) =>
            Results.Ok(await consulta.HandleAsync(ct))).RequireAuthorization(Papeis.AdministradorGeral).WithTags("Plataforma");

        app.MapPut("/v1/plataforma/configuracao", async (ConfiguracaoPlataformaDto body, AlterarConfiguracaoPlataforma comando, CancellationToken ct) =>
            Results.Ok(await comando.HandleAsync(body, ct))).RequireAuthorization(Papeis.AdministradorGeral).WithTags("Plataforma");
    }
}
