using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace OrchestratR.Core.Configurators
{
    public interface IServerTransportConfigurator
    {
        string OrchestratorServerName { get; }
        int MaxWorkersCount { get; }
        IServiceCollectionBusConfigurator BusConfigurator { get; }
    }
}