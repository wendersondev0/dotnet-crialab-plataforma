using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CriaLab.Api.Core.Identidade;
using Microsoft.IdentityModel.Tokens;

namespace CriaLab.Api.Presentation.WebApi.Identidade;

public sealed class EmissorAcesso(IConfiguration configuracao) : IEmissorAcesso
{
    public (string Token, DateTime ExpiraEm) Emitir(Guid usuarioId, string nome, string email, IReadOnlyCollection<string> papeis)
    {
        var minutos = configuracao.GetValue("Jwt:AcessoMinutos", 15);
        var expira = DateTime.UtcNow.AddMinutes(minutos);
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracao["Jwt:Key"]!));
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Name, nome)
        };
        claims.AddRange(papeis.Select(papel => new Claim(ClaimTypes.Role, papel)));
        var token = new JwtSecurityToken(
            configuracao["Jwt:Issuer"],
            configuracao["Jwt:Audience"],
            claims,
            expires: expira,
            signingCredentials: new SigningCredentials(chave, SecurityAlgorithms.HmacSha256));
        return (new JwtSecurityTokenHandler().WriteToken(token), expira);
    }

    public DateTime RefreshExpiraEm() => DateTime.UtcNow.AddDays(configuracao.GetValue("Jwt:RefreshDias", 14));
}
