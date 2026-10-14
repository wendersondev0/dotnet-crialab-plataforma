using CriaLab.Api.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CriaLab.Api.Infra.SqlServer;

public class CriaLabDbContextFactory : IDesignTimeDbContextFactory<CriaLabDbContext>
{
    public CriaLabDbContext CreateDbContext(string[] args)
    {
        var conexao = Environment.GetEnvironmentVariable("CRIALAB_CONNECTION");
        if (string.IsNullOrWhiteSpace(conexao))
            throw new InvalidOperationException("Defina a variável CRIALAB_CONNECTION para criar ou aplicar migrações.");

        var opcoes = new DbContextOptionsBuilder<CriaLabDbContext>()
            .UseSqlServer(conexao, sql => sql.MigrationsAssembly(typeof(CriaLabDbContextFactory).Assembly.GetName().Name))
            .Options;
        return new CriaLabDbContext(opcoes);
    }
}
