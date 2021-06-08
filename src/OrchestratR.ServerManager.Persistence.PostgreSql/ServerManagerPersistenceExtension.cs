using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;

namespace OrchestratR.ServerManager.Persistence.PostgreSql
{
    public static class ServerManagerPersistenceExtension
    {
        public static IServerManagerTransportConfigurator UsePostgreSqlStorage(this IServerManagerPersistenceConfigurator configurator,
            string connectionString)
        {
            configurator.Services.AddDbContext<OrchestratorDbContext>(
                (_, builder) => builder.UseNpgsql(connectionString, innerBuilder =>
                {
                    innerBuilder.MigrationsAssembly("OrchestratR.ServerManager.Persistence.PostgreSql");
                    innerBuilder.MigrationsHistoryTable("__MyMigrationsHistory", "orchestrator");
                }));

            return configurator.TransportConfigurator;
        }
    }
}