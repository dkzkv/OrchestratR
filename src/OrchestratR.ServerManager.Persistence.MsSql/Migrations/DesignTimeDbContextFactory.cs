using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrchestratR.ServerManager.Persistence.MsSql.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OrchestratorDbContext>
    {
        public OrchestratorDbContext CreateDbContext(string[] args)
        {
            return new OrchestratorDbContext(new DbContextOptionsBuilder<OrchestratorDbContext>()
                .UseSqlServer("connection", b => b.MigrationsAssembly(this.GetType().Assembly.FullName))
                .Options);
        }
    }
}