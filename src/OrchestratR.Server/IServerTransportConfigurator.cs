using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace OrchestratR.Server
{
    public interface IServerTransportConfigurator
    {
        string OrchestratorServerName { get; }
        int MaxWorkersCount { get; }
        IServiceCollectionBusConfigurator BusConfigurator { get; }
    }
}