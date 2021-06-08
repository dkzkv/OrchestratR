using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;

namespace OrchestratR.Server.Configurators
{
    internal class ServerTransportConfigurator : IServerTransportConfigurator
    {
        public ServerTransportConfigurator(string orchestratorServerName, int maxWorkersCount, IServiceCollectionBusConfigurator busConfigurator,
            IServiceCollection serviceCollection)
        {
            OrchestratorServerName = orchestratorServerName;
            MaxWorkersCount = maxWorkersCount;
            BusConfigurator = busConfigurator;
            ServiceCollection = serviceCollection;
        }

        public string OrchestratorServerName { get; }
        public int MaxWorkersCount { get; }
        public IServiceCollectionBusConfigurator BusConfigurator { get; }
        public IServiceCollection ServiceCollection { get; }
    }
}