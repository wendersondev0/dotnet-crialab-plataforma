using Carter;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Lojas.Commands;
using CriaLab.Api.Core.Lojas.Dtos;
using CriaLab.Api.Core.Lojas.Queries;
using CriaLab.Api.Core.Planos.Commands;

namespace CriaLab.Api.Presentation.WebApi.Endpoints;

public class LojasEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/lojas/vitrine", async (ListarVitrine consulta, CancellationToken ct) =>
            Results.Ok(await consulta.HandleAsync(ct))).AllowAnonymous().WithTags("Lojas");

        app.MapGet("/v1/lojas/por-slug/{slug}", async (string slug, ObterLojaPorSlug consulta, CancellationToken ct) =>
        {
            var loja = await consulta.HandleAsync(slug, ct);
            return loja is null ? Results.NotFound(new { title = "Loja não encontrada." }) : Results.Ok(loja);
        }).AllowAnonymous().WithTags("Lojas");

        app.MapGet("/v1/lojas/minha", async (HttpContext http, ObterMinhaLoja consulta, CancellationToken ct) =>
        {
            var loja = await consulta.HandleAsync(UsuarioAtual.Id(http), ct);
            return loja is null ? Results.NoContent() : Results.Ok(loja);
        }).RequireAuthorization().WithTags("Lojas");

        app.MapPost("/v1/lojas", async (CriarLojaBody body, HttpContext http, CriarLoja comando, CancellationToken ct) =>
        {
            var id = await comando.HandleAsync(UsuarioAtual.Id(http), body.Nome, body.Slug, ct);
            return Results.Ok(new { id });
        }).RequireAuthorization().WithTags("Lojas");

        app.MapPost("/v1/lojas/minha/plano", async (PlanoBody body, HttpContext http, AlterarPlanoLoja comando, CancellationToken ct) =>
        {
            await comando.HandleAsync(UsuarioAtual.Id(http), body.Codigo, ct);
            return Results.NoContent();
        }).RequireAuthorization(Papeis.Vendedor).WithTags("Lojas");

        app.MapPost("/v1/lojas/minha/convites", async (ConviteBody body, HttpContext http, ConvidarColaborador comando, CancellationToken ct) =>
        {
            var criado = await comando.HandleAsync(UsuarioAtual.Id(http), body.Permissoes, body.Email, ct);
            var dev = http.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment();
            return Results.Ok(dev ? criado : criado with { Token = null });
        }).RequireAuthorization(Papeis.Vendedor).WithTags("Lojas");

        app.MapPost("/v1/convites/aceitar", async (TokenBody body, HttpContext http, AceitarConvite comando, CancellationToken ct) =>
        {
            await comando.HandleAsync(UsuarioAtual.Id(http), body.Token, ct);
            return Results.NoContent();
        }).RequireAuthorization().WithTags("Lojas");
    }
}

public sealed record CriarLojaBody(string Nome, string Slug);
public sealed record PlanoBody(string Codigo);
public sealed record ConviteBody(string Email, PermissoesColaborador Permissoes);
