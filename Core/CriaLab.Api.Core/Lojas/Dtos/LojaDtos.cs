namespace CriaLab.Api.Core.Lojas.Dtos;

public sealed record PermissoesColaborador(bool Pedidos, bool Estoque, bool Orcamento, bool Financeiro, bool Patrimonio);

public sealed record RedePublicaDto(string Rede, string Url);

public sealed record LojaPublicaDto(
    Guid Id,
    string Nome,
    string Slug,
    string Descricao,
    string Logo,
    string Capa,
    string Cnpj,
    string Telefone,
    string Whatsapp,
    string Email,
    bool SeloOficial,
    string Plano,
    IReadOnlyList<RedePublicaDto> Redes);

public sealed record FaturaDto(Guid Id, DateOnly Competencia, decimal Valor, string Status);

public sealed record ConviteDto(Guid Id, string Email, bool Aceito, PermissoesColaborador Permissoes);

public sealed record MinhaLojaDto(
    Guid Id,
    string Nome,
    string Slug,
    string Plano,
    string PlanoCodigo,
    decimal Mensalidade,
    bool PodePublicar,
    bool PodeVenderArquivo,
    bool Dono,
    PermissoesColaborador Permissoes,
    IReadOnlyList<FaturaDto> Faturas,
    IReadOnlyList<ConviteDto> Convites);

public sealed record ConviteCriadoDto(Guid Id, string Email, string? Token);
