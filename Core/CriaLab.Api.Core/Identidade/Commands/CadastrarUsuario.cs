using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Database;
using CriaLab.Api.Core.Identidade.Domain;
using CriaLab.Api.Core.Identidade.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Core.Identidade.Commands;

public class CadastrarUsuario(CriaLabDbContext db)
{
    public async Task<CadastroResultado> HandleAsync(string nome, string email, string senha, CancellationToken ct)
    {
        nome = (nome ?? "").Trim();
        email = EmailUsuario.Normalizar(email ?? "");
        if (nome.Length < 2)
            throw new RegraNegocioException("Informe o nome.");
        if (!EmailUsuario.Valido(email))
            throw new RegraNegocioException("Informe um e-mail válido.");
        if (string.IsNullOrWhiteSpace(senha) || senha.Length < 8)
            throw new RegraNegocioException("A senha precisa ter pelo menos 8 caracteres.");

        if (await db.Usuarios.AnyAsync(u => u.Email == email, ct))
            throw new RegraNegocioException("Já existe uma conta com este e-mail.", 409);

        var token = TokenOpaco.Gerar();
        var usuario = new Usuario
        {
            Nome = nome,
            Email = email,
            SenhaHash = SenhaHasher.Gerar(senha),
            TokenConfirmacaoHash = TokenOpaco.Hash(token),
            TokenConfirmacaoExpiraEm = DateTime.UtcNow.AddDays(2),
            Papeis = [new UsuarioPapel { Papel = Papeis.Cliente }]
        };
        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync(ct);
        return new CadastroResultado(UsuarioDto.De(usuario, null), token);
    }
}
