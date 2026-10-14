using CriaLab.Api.Core.Comum;
using CriaLab.Api.Core.Planos.Domain;

namespace CriaLab.Api.Core.Lojas.Domain;

public class Loja : Entidade
{
    public string Nome { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Descricao { get; set; } = "";
    public string Logo { get; set; } = "";
    public string Capa { get; set; } = "";
    public string Cnpj { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string Whatsapp { get; set; } = "";
    public string Email { get; set; } = "";
    public string Logradouro { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Complemento { get; set; } = "";
    public string Bairro { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Uf { get; set; } = "";
    public string Cep { get; set; } = "";
    public bool SeloOficial { get; set; }
    public bool Ativa { get; set; } = true;
    public bool IsentaMensalidade { get; set; }
    public Guid PlanoId { get; set; }
    public Plano Plano { get; set; } = null!;
    public Guid? DonoUsuarioId { get; set; }
    public List<RedeSocialLoja> Redes { get; set; } = [];
    public List<LojaMembro> Membros { get; set; } = [];
}
