namespace CriaLab.Api.Core.Comum;

public sealed class RegraNegocioException : Exception
{
    public int Status { get; }

    public RegraNegocioException(string mensagem, int status = 400) : base(mensagem)
    {
        Status = status;
    }
}
