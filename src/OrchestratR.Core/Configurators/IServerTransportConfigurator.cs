using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace OrchestratR.Core.Configurators
{
    public interface IServerTransportConfigurator
    {
        string OrchestratorServerName { get; }
        int MaxWorkersCount { get; }
        IServiceCollectionBusConfigurator BusConfigurator { get; }
        IServiceCollection ServiceCollection { get; }
    }
}