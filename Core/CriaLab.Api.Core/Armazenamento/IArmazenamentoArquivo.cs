namespace CriaLab.Api.Core.Armazenamento;

public interface IArmazenamentoArquivo
{
    Task<string> SalvarAsync(string nome, Stream conteudo, CancellationToken cancellationToken);
}
