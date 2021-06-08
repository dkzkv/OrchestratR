using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;

namespace OrchestratR.ServerManager.Persistence.InMemory
{
    public static class ServerManagerPersistenceExtension
    {
        public static IServerManagerTransportConfigurator UseInMemoryStorage(this IServerManagerPersistenceConfigurator configurator,
            string databaseName)
        {
            configurator.Services.AddDbContext<OrchestratorDbContext>(
                (_, builder) => builder.UseInMemoryDatabase(databaseName));

            return configurator.TransportConfigurator;
        }
    }
}