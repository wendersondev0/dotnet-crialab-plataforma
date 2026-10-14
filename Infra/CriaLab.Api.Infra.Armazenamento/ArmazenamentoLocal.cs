using CriaLab.Api.Core.Armazenamento;

namespace CriaLab.Api.Infra.Armazenamento;

public sealed class ArmazenamentoLocal(string pastaRaiz) : IArmazenamentoArquivo
{
    public async Task<string> SalvarAsync(string nome, Stream conteudo, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(pastaRaiz);
        var arquivo = Path.Combine(pastaRaiz, $"{Guid.NewGuid():N}{Path.GetExtension(nome)}");
        await using var destino = File.Create(arquivo);
        await conteudo.CopyToAsync(destino, cancellationToken);
        return arquivo;
    }
}
