using MassTransit.ExtensionsDependencyInjectionIntegration;
using OrchestratR.Core.Configurators;

namespace OrchestratR.Server.Configurators
{
    internal class ServerTransportConfigurator : IServerTransportConfigurator
    {
        public ServerTransportConfigurator(string orchestratorServerName, int maxWorkersCount, IServiceCollectionBusConfigurator busConfigurator)
        {
            OrchestratorServerName = orchestratorServerName;
            MaxWorkersCount = maxWorkersCount;
            BusConfigurator = busConfigurator;
        }

        public string OrchestratorServerName { get; }
        public int MaxWorkersCount { get; }
        public IServiceCollectionBusConfigurator BusConfigurator { get; }
    }
}