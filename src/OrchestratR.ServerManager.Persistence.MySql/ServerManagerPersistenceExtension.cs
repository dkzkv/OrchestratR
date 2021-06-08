using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;

namespace OrchestratR.ServerManager.Persistence.MySql
{
    public static class ServerManagerPersistenceExtension
    {
        public static IServerManagerTransportConfigurator UseMySqlStorage(this IServerManagerPersistenceConfigurator configurator,
            string connectionString)
        {
            configurator.Services.AddDbContext<OrchestratorDbContext>(
                (_, builder) => builder.UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 11)),
                    innerBuilder =>
                {
                    innerBuilder.MigrationsAssembly("OrchestratR.ServerManager.Persistence.MySql");
                    innerBuilder.MigrationsHistoryTable("__MyMigrationsHistory", "orchestrator");
                }));

            return configurator.TransportConfigurator;
        }
    }
}