using Carter;

namespace CriaLab.Api.Presentation.WebApi.Endpoints;

public class HealthEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
            .WithTags("Infraestrutura")
            .AllowAnonymous();
    }
}
