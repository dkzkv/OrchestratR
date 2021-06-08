using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrchestratR.ServerManager.Persistence.MySql.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OrchestratorDbContext>
    {
        public OrchestratorDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrchestratorDbContext>().UseMySql(
                "server=;user=;password=;database=;", 
                new MySqlServerVersion(new Version(8, 0, 11)),
                b => b.MigrationsAssembly(this.GetType().Assembly.FullName)
            ).Options;

            return new OrchestratorDbContext(optionsBuilder);
        }
    }
}