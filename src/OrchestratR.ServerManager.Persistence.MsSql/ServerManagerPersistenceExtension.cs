using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;

namespace OrchestratR.ServerManager.Persistence.MsSql
{
    public static class ServerManagerPersistenceExtension
    {
        public static IServerManagerTransportConfigurator UseSqlServerStorage(this IServerManagerPersistenceConfigurator configurator,
            string connectionString)
        {
            configurator.Services.AddDbContext<OrchestratorDbContext>(
                (_, builder) => builder.UseSqlServer(connectionString, innerBuilder =>
                {
                    innerBuilder.MigrationsAssembly("OrchestratR.ServerManager.Persistence.MsSql");
                    innerBuilder.MigrationsHistoryTable("__MyMigrationsHistory", "orchestrator");
                }));

            return configurator.TransportConfigurator;
        }
    }
}