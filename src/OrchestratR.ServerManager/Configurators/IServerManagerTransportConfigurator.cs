using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace OrchestratR.ServerManager.Configurators
{
    public interface IServerManagerTransportConfigurator
    {
        IServiceCollectionBusConfigurator BusConfigurator { get; }
    }
}