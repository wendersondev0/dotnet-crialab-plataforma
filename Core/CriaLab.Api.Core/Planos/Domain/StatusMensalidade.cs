namespace CriaLab.Api.Core.Planos.Domain;

public static class StatusMensalidade
{
    public const string Pago = "Pago";
    public const string Pendente = "Pendente";
    public const string Dispensado = "Dispensado";
    public const string Atrasado = "Atrasado";

    public static string Calcular(decimal faturamento, decimal teto) =>
        teto > 0 && faturamento >= teto ? Dispensado : Pendente;
}
