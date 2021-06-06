using Microsoft.EntityFrameworkCore;
using OrchestratR.ServerManager.Persistence.Entities;

namespace OrchestratR.ServerManager.Persistence
{
    public class OrchestratorDbContext : DbContext
    {
        public OrchestratorDbContext(DbContextOptions<OrchestratorDbContext> options) : base(options)
        {
        }

        public DbSet<OrchestratedJob> OrchestratedJobs { get; set; }
        public DbSet<Server> Servers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("orchestrator");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrchestratorDbContext).Assembly);
        }
    }
}