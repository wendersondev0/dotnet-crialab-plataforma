using Microsoft.EntityFrameworkCore;

namespace CriaLab.Api.Infra.SqlServer;

public class CriaLabDbContext(DbContextOptions<CriaLabDbContext> options) : DbContext(options)
{
}
