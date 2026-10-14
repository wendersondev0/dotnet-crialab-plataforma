using System.Security.Claims;
using Carter;
using CriaLab.Api.Core.Identidade.Commands;
using CriaLab.Api.Core.Identidade.Queries;

namespace CriaLab.Api.Presentation.WebApi.Endpoints;

public class AuthEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/auth/cadastro", async (CadastroBody body, CadastrarUsuario comando, HttpContext http, CancellationToken ct) =>
        {
            var resultado = await comando.HandleAsync(body.Nome, body.Email, body.Senha, ct);
            var dev = http.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment();
            return Results.Ok(new
            {
                resultado.Usuario,
                tokenConfirmacao = dev ? resultado.TokenConfirmacao : null
            });
        }).AllowAnonymous().WithTags("Identidade");

        app.MapPost("/v1/auth/entrar", async (EntrarBody body, Entrar comando, CancellationToken ct) =>
            Results.Ok(await comando.HandleAsync(body.Email, body.Senha, ct))).AllowAnonymous().WithTags("Identidade");

        app.MapPost("/v1/auth/atualizar", async (RefreshBody body, AtualizarSessao comando, CancellationToken ct) =>
            Results.Ok(await comando.HandleAsync(body.RefreshToken, ct))).AllowAnonymous().WithTags("Identidade");

        app.MapPost("/v1/auth/confirmar-email", async (TokenBody body, ConfirmarEmail comando, CancellationToken ct) =>
        {
            await comando.HandleAsync(body.Token, ct);
            return Results.NoContent();
        }).AllowAnonymous().WithTags("Identidade");

        app.MapPost("/v1/auth/esqueci-senha", async (EmailBody body, EsquecerSenha comando, HttpContext http, CancellationToken ct) =>
        {
            var resultado = await comando.HandleAsync(body.Email, ct);
            var dev = http.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment();
            return Results.Ok(new { tokenRedefinicao = dev ? resultado?.TokenRedefinicao : null });
        }).AllowAnonymous().WithTags("Identidade");

        app.MapPost("/v1/auth/redefinir-senha", async (RedefinirBody body, RedefinirSenha comando, CancellationToken ct) =>
        {
            await comando.HandleAsync(body.Token, body.Senha, ct);
            return Results.NoContent();
        }).AllowAnonymous().WithTags("Identidade");
    }
}

public class UsuariosEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/usuarios/eu", async (HttpContext http, ObterUsuarioAtual consulta, CancellationToken ct) =>
        {
            var id = UsuarioAtual.Id(http);
            return Results.Ok(await consulta.HandleAsync(id, ct));
        }).RequireAuthorization().WithTags("Identidade");
    }
}

public static class UsuarioAtual
{
    public static Guid Id(HttpContext http)
    {
        var valor = http.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? http.User.FindFirstValue(JwtClaimSub);
        return Guid.Parse(valor!);
    }

    private const string JwtClaimSub = "sub";
}

public sealed record CadastroBody(string Nome, string Email, string Senha);
public sealed record EntrarBody(string Email, string Senha);
public sealed record RefreshBody(string RefreshToken);
public sealed record TokenBody(string Token);
public sealed record EmailBody(string Email);
public sealed record RedefinirBody(string Token, string Senha);
