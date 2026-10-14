namespace CriaLab.Api.Core.Identidade.Domain;

public static class Papeis
{
    public const string Cliente = "Cliente";
    public const string Revendedor = "Revendedor";
    public const string Vendedor = "Vendedor";
    public const string ColaboradorLoja = "ColaboradorLoja";
    public const string AdministradorGeral = "AdministradorGeral";

    public static readonly string[] Todos = [Cliente, Revendedor, Vendedor, ColaboradorLoja, AdministradorGeral];
}
