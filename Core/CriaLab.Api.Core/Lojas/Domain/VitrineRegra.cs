namespace CriaLab.Api.Core.Lojas.Domain;

public static class VitrineRegra
{
    public static bool Aparece(bool ativa, bool publicaVitrine) => ativa && publicaVitrine;
}
