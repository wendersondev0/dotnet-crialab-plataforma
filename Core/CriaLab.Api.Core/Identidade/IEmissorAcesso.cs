namespace CriaLab.Api.Core.Identidade;

public interface IEmissorAcesso
{
    (string Token, DateTime ExpiraEm) Emitir(Guid usuarioId, string nome, string email, IReadOnlyCollection<string> papeis);
    DateTime RefreshExpiraEm();
}
