using CriaLab.Api.Core.Identidade.Domain;

namespace CriaLab.Api.Core.Identidade.Dtos;

public sealed record UsuarioDto(Guid Id, string Nome, string Email, bool EmailConfirmado, IReadOnlyList<string> Papeis, Guid? LojaId)
{
    public static UsuarioDto De(Usuario usuario, Guid? lojaId) => new(
        usuario.Id,
        usuario.Nome,
        usuario.Email,
        usuario.EmailConfirmadoEm != null,
        usuario.Papeis.Select(p => p.Papel).ToArray(),
        lojaId);
}

public sealed record CadastroResultado(UsuarioDto Usuario, string TokenConfirmacao);

public sealed record RecuperacaoResultado(string TokenRedefinicao);

public sealed record SessaoDto(UsuarioDto Usuario, string Acesso, DateTime AcessoExpiraEm, string RefreshToken, DateTime RefreshExpiraEm);
