using CriaLab.Api.Core.Planos.Commands;

namespace CriaLab.Api.Presentation.WebApi.Mensalidades;

public sealed class MensalidadeJob(IServiceScopeFactory escopos, ILogger<MensalidadeJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Gerar(stoppingToken);
        using var timer = new PeriodicTimer(TimeSpan.FromHours(24));
        while (await timer.WaitForNextTickAsync(stoppingToken))
            await Gerar(stoppingToken);
    }

    private async Task Gerar(CancellationToken ct)
    {
        try
        {
            await using var escopo = escopos.CreateAsyncScope();
            var comando = escopo.ServiceProvider.GetRequiredService<GerarMensalidades>();
            await comando.HandleAsync(DateTime.UtcNow, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Falha ao gerar mensalidades.");
        }
    }
}
