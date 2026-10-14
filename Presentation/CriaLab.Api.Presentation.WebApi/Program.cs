using System.Globalization;
using System.Text;
using Carter;
using CriaLab.Api.Core.Armazenamento;
using CriaLab.Api.Infra.Armazenamento;
using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Infra.SqlServer;
using CriaLab.Api.Presentation.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var cultura = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;

builder.Services.ConfigureHttpJsonOptions(opcoes =>
{
    opcoes.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddCriaLab();

var conexao = builder.Configuration.GetConnectionString("CriaLab");
builder.Services.AddDbContext<CriaLabDbContext>(opcoes =>
    opcoes.UseSqlServer(conexao, sql => sql.MigrationsAssembly(typeof(CriaLabDbContextFactory).Assembly.GetName().Name)));

var pasta = builder.Configuration["Armazenamento:Pasta"];
if (string.IsNullOrWhiteSpace(pasta))
    pasta = Path.Combine(builder.Environment.ContentRootPath, "arquivos");
builder.Services.AddSingleton<IArmazenamentoArquivo>(_ => new ArmazenamentoLocal(pasta));

var chaveJwt = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(chaveJwt) || Encoding.UTF8.GetByteCount(chaveJwt) < 32)
    throw new InvalidOperationException("Configure Jwt:Key com pelo menos 32 caracteres via user-secrets.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt))
        };
    });
builder.Services.AddAuthorization(opcoes =>
{
    foreach (var papel in CriaLab.Api.Core.Identidade.Domain.Papeis.Todos)
        opcoes.AddPolicy(papel, politica => politica.RequireRole(papel));
});

builder.Services.AddCors(opcoes =>
{
    opcoes.AddPolicy("Web", politica =>
        politica.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseExceptionHandler(pipeline =>
{
    pipeline.Run(async contexto =>
    {
        var erro = contexto.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (erro is RegraNegocioException regra)
        {
            contexto.Response.StatusCode = regra.Status;
            await contexto.Response.WriteAsJsonAsync(new { title = regra.Message, status = regra.Status });
            return;
        }
        var desenvolvimento = app.Environment.IsDevelopment();
        contexto.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await contexto.Response.WriteAsJsonAsync(new
        {
            title = "Não foi possível concluir a operação.",
            detail = desenvolvimento ? erro?.Message : null,
            status = StatusCodes.Status500InternalServerError
        });
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Web");
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

await using (var escopo = app.Services.CreateAsyncScope())
{
    var banco = escopo.ServiceProvider.GetRequiredService<CriaLabDbContext>();
    await banco.Database.MigrateAsync();
    await new CriaLabSeed(banco, app.Configuration).AplicarAsync();
}

app.Run();
